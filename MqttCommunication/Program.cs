using Microsoft.EntityFrameworkCore;
using MqttCommunication;
using Services.Context;

var builder = Host.CreateApplicationBuilder(args);

IConfigurationRoot Configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

builder.Services.AddDbContext<MachineManagementDbContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:conStr"]));

builder.Services.AddHostedService<MqttWorkerService>();

var host = builder.Build();
host.Run();
