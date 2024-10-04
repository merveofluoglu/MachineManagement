
using WebSocketApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<WebSocketHandler>();

var app = builder.Build();

app.UseWebSockets();

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/ws")
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            var webSocketHandler = app.Services.GetRequiredService<WebSocketHandler>();
            await webSocketHandler.HandleAsync(webSocket);
        }
        else
        {
            context.Response.StatusCode = 400;
        }
    }
    else
    {
        await next();
    }
});

app.Run();

