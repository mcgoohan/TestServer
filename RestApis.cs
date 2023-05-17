using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace TestServer;

public static class RestApis
{
    public static void AddRestApis(this WebApplication app)
    {
        app.MapGet("/responses/{statusCode}", Results.StatusCode).WithTags("HTTP Status Codes").WithOpenApi();
        app.MapPost("/responses/{statusCode}", Results.StatusCode).WithTags("HTTP Status Codes").WithOpenApi();
        app.MapPut("/responses/{statusCode}", Results.StatusCode).WithTags("HTTP Status Codes").WithOpenApi();
        app.MapDelete("/responses/{statusCode}", Results.StatusCode).WithTags("HTTP Status Codes").WithOpenApi();

        app.MapPost("/{method}", async ([FromServices]ILogger<Program> logger, HttpContext context, string method, JsonDocument message) =>
           {
               logger.LogInformation($"Sending to hub method: {method}");
               logger.LogInformation($"Sending the following data: {message.RootElement.ToString()}");
               var hubContext = context.RequestServices.GetRequiredService<IHubContext<SignalRHub>>();
               await hubContext.Clients.All.SendAsync(method, message.RootElement.ToString());

               return Results.StatusCode(200);
           })
           .WithTags("Signalr Message Sender")
           .WithOpenApi();
    }
}