using Microsoft.EntityFrameworkCore;
using Models;
using MqttCommunication;
using Services.Context;
using Services.IServices;
using Services.Services;
using Microsoft.AspNetCore.Hosting;

var builder = Host.CreateApplicationBuilder(args);

IConfigurationRoot Configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

builder.Services.AddDbContext<MachineManagementDbContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:constr"]));
builder.Services.AddScoped<IMessagesService, MessagesService>();
builder.Services.AddScoped<IMachinesService, MachinesService>();
builder.Services.AddScoped<DtoConverter, DtoConverter>();

builder.Services.AddHttpClient();

builder.Services.AddHostedService<MqttWorkerService>();

var host = builder.Build();

host.Run();