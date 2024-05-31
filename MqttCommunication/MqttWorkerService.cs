using Microsoft.IdentityModel.Tokens;
using Models.entities;
using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json.Linq;
using Services.Context;
using Services.IServices;
using System.Text;
using System.Text.Json.Nodes;

namespace MqttCommunication
{
    public class MqttWorkerService : BackgroundService
    {
        private readonly ILogger<MqttWorkerService> _Logger;
        private IMqttClient _mqttClient;
        private readonly IConfiguration _Configuration;
        private IMessagesService _MessagesService;

        public MqttWorkerService(ILogger<MqttWorkerService> logger,
                                IConfiguration config,
                                IMessagesService messagesService)
        {
            _Logger = logger;
            _Configuration = config;
            _MessagesService = messagesService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_Logger.IsEnabled(LogLevel.Information))
                {
                    _Logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }

                await Task.Delay(1000, stoppingToken);
            }
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            var mqttFactory = new MqttFactory();
            _mqttClient = mqttFactory.CreateMqttClient();

            var _section = _Configuration.GetSection("configuration:appSettings");

            var _options = new MqttClientOptionsBuilder()
                .WithTcpServer(_section.GetSection("server").Value, int.Parse(_section.GetSection("port").Value))
                .WithKeepAlivePeriod(TimeSpan.FromSeconds(60))
                .Build();

            await _mqttClient.ConnectAsync(_options, cancellationToken);

            _Logger.LogInformation("Connected successfully to the Broker");

            // Subscribe to a topic
            await _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("MACHINES/#").Build());

            _mqttClient.ApplicationMessageReceivedAsync += async (_e) =>
            {
                _Logger.LogInformation($"Received message: {Encoding.UTF8.GetString(_e.ApplicationMessage.PayloadSegment)}");
                await ProcessMessageAsync(_e);
            };
        }

        private async Task ProcessMessageAsync(MqttApplicationMessageReceivedEventArgs _e)
        {
            var topicName = _e.ApplicationMessage.Topic;
            var section = _Configuration.GetSection("Configuration:appSettings:Topics");

            if (IsTopicValid(topicName, section))
            {
                try
                {
                    var _message = Encoding.UTF8.GetString(_e.ApplicationMessage.PayloadSegment);
                    var obj = JObject.Parse(_message);

                    if (IsMessageValid(obj))
                    {
                        await _MessagesService.CreateMessageAsync(
                                                new Messages
                                                {
                                                    Client_Id = (long)obj.GetValue("client_id"),
                                                    Message = obj.GetValue("message").ToString(),
                                                    Topic = topicName,
                                                    StatusCode = 200,
                                                    ErrorCode = 0,
                                                    ErrorType = null,
                                                    ErrorMessage = null,
                                                    IsReceived = true,
                                                    IsRead = false,
                                                    Date = DateTime.Now
                                                }
                                            );
                        _Logger.LogInformation($"Received message added to the database.");
                    }
                    else
                    {
                        _Logger.LogInformation($"Received message: {Encoding.UTF8.GetString(_e.ApplicationMessage.PayloadSegment)} is not valid.");
                    }
                }
                catch (Exception _ex)
                {
                    _Logger.LogError(_ex.Message);
                }
            }
        }

        private bool IsTopicValid(string _topicName, IConfigurationSection _section)
        {
            return _section.GetChildren().Any(x => x.Value == _topicName);
        }

        private bool IsMessageValid(JObject obj)
        {
            if(obj.GetValue("client_id").IsNullOrEmpty() || obj.GetValue("message").IsNullOrEmpty())
            {
                return false;
            }
            return true;
        }

    }
}
