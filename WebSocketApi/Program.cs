
using WebSocketApi;
using WebSocketApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddSingleton<WebSocketHandler>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials()
               .SetIsOriginAllowed(origin => true);
    });
});

builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting(); 

//app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<MessageHub>("/messagehub");
    endpoints.MapControllers();
});

//app.Use(async (context, next) =>
//{
//    if (context.Request.Path == "/ws")
//    {
//        if (context.WebSockets.IsWebSocketRequest)
//        {
//            var webSocket = await context.WebSockets.AcceptWebSocketAsync();
//            var webSocketHandler = app.Services.GetRequiredService<WebSocketHandler>();
//            await webSocketHandler.HandleAsync(webSocket);
//        }
//        else
//        {
//            context.Response.StatusCode = 400;
//        }
//    }
//    else
//    {
//        await next();
//    }
//});

app.Run();