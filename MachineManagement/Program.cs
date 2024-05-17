using Microsoft.EntityFrameworkCore;
using Models;
using Services.Context;
using Services.IServices;
using Services.Services;

var builder = WebApplication.CreateBuilder(args);

IConfigurationRoot Configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build(); ;

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<MachineManagementDbContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:conStr"]));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddScoped<IMachinesService, MachinesService>();
builder.Services.AddScoped<IMessagesService, MessagesService>();
builder.Services.AddScoped<DtoConverter, DtoConverter>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options => {
    options.AddPolicy("CORSPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CORSPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
