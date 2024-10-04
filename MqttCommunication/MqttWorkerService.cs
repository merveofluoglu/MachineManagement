using Microsoft.IdentityModel.Tokens;
using Models.entities;
using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json.Linq;
using Services.IServices;
using System.Net.Http;
using System.Text;

namespace MqttCommunication
{
    public class MqttWorkerService : BackgroundService
    {
        private readonly ILogger<MqttWorkerService> _Logger;
        private IMqttClient? _mqttClient;
        private readonly IConfiguration _Configuration;
        private readonly IServiceProvider _ServiceProvider;
        private readonly IHttpClientFactory _HttpClientFactory;


        public MqttWorkerService(ILogger<MqttWorkerService> logger,
                                IConfiguration config,
                                IServiceProvider serviceProvider,
                                 IHttpClientFactory httpClientFactory)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _Configuration = config ?? throw new ArgumentNullException(nameof(_Configuration));
            _ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _HttpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));

            _Logger.LogInformation("MqttWorkerService constructed.");
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
            try
            {
                _Logger.LogInformation("StartAsync called.");

                var mqttFactory = new MqttFactory();
                _mqttClient = mqttFactory.CreateMqttClient();
                
                _mqttClient.ApplicationMessageReceivedAsync += async (_e) =>
                {
                    Console.WriteLine("Message: \n" + _e.ApplicationMessage.PayloadSegment);
                    _Logger.LogInformation($"Received message: {Encoding.UTF8.GetString(_e.ApplicationMessage.PayloadSegment)}");
                    await ProcessMessageAsync(_e);
                };

                var _section = _Configuration.GetSection("appSettings");

                if (_section == null)
                {
                    _Logger.LogError("Configuration section 'configuration:appSettings' not found.");
                    throw new ArgumentNullException(nameof(_section));
                }

                var _options = new MqttClientOptionsBuilder()
                    .WithTcpServer(_section.GetSection("server").Value, int.Parse(_section.GetSection("port").Value))
                    .WithKeepAlivePeriod(TimeSpan.FromSeconds(60))
                    .Build();                

                _mqttClient.ConnectAsync(_options, cancellationToken).Wait();

                _Logger.LogInformation("Connected successfully to the Broker");

                // Subscribe to the topic.
                _mqttClient.SubscribeAsync(_section.GetSection("Topics:0").Value, MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce).Wait();
                
                _Logger.LogInformation("Subscribed to topic successfully.");

                Console.ReadLine();
                _mqttClient.DisconnectAsync().Wait();                
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "An error occurred in StartAsync.");
                throw;
            }
        }

        private async Task ProcessMessageAsync(MqttApplicationMessageReceivedEventArgs _e)
        {
            using (var scope = _ServiceProvider.CreateScope())
            {
                var topicName = _e.ApplicationMessage.Topic;
                var section = _Configuration.GetSection("appSettings:Topics");

                if (IsTopicValid(topicName, section))
                {
                    try
                    {
                        var _MessagesService = scope.ServiceProvider.GetRequiredService<IMessagesService>();
                        var _MachinesService = scope.ServiceProvider.GetRequiredService<IMachinesService>();
                        var _htpp = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>();

                        var _message = Encoding.UTF8.GetString(_e.ApplicationMessage.PayloadSegment);
                        var obj = JObject.Parse(_message);

                        if (IsMessageValid(obj))
                        {
                            var msg = new Messages
                            {
                                Client_Id = (long)obj.GetValue("client_id"),
                                Message = obj.GetValue("message").ToString(),
                                Topic = topicName.Split("/").Last(),
                                StatusCode = 200,
                                ErrorCode = 0,
                                ErrorType = null,
                                ErrorMessage = null,
                                IsReceived = true,
                                IsRead = false,
                                Date = DateTime.Now
                            };

                            await _MessagesService.CreateMessageAsync(msg);
                            await _MachinesService.IncrementMessageCountAsync((long)obj.GetValue("client_id"));
                            _Logger.LogInformation($"Received message added to the database. ");

                            // Notify WebSocketApi server
                            var httpClient = _HttpClientFactory.CreateClient();
                            var jsonMessage = JObject.FromObject(msg).ToString();
                            var content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");
                            await httpClient.PostAsync("http://localhost:7040/Notification/notify", content);
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
        }

        private bool IsTopicValid(string _topicName, IConfigurationSection _section)
        {
            return _section.GetChildren().Any(x => x.Value == _topicName);
        }

        private bool IsMessageValid(JObject obj)
        {
            if (obj.GetValue("client_id").ToString().IsNullOrEmpty() || obj.GetValue("message").ToString().IsNullOrEmpty())
            {
                return false;
            }
            return true;
        }
    }
}
