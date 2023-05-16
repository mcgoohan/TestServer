using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", corsBuilder =>
    {
        var allowedOriginals = builder.Configuration["AllowedOrigins"] ?? "http://localhost:8100";
        corsBuilder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins(allowedOriginals)
            .AllowCredentials();
    });
});
var app = builder.Build();
app.UseSwagger();

app.UseRouting();
app.UseCors("CorsPolicy");
var hubUri = builder.Configuration["SignalRHubUri"] ?? "hub/v1/notifications";
app.MapHub<MySignalRHub>(hubUri);

app.MapGet("/responses/{statusCode}", Results.StatusCode).WithTags("HTTP Status Codes").WithOpenApi();
app.MapPost("/responses/{statusCode}", Results.StatusCode).WithTags("HTTP Status Codes").WithOpenApi();
app.MapPut("/responses/{statusCode}", Results.StatusCode).WithTags("HTTP Status Codes").WithOpenApi();
app.MapDelete("/responses/{statusCode}", Results.StatusCode).WithTags("HTTP Status Codes").WithOpenApi();

app.MapPost("/{method}", async ([FromServices]ILogger<Program> logger, HttpContext context, string method, JsonDocument message) =>
   {
       logger.LogInformation($"Sending to hub method: {method}");
       logger.LogInformation($"Sending the following data: {message.RootElement.ToString()}");
       var hubContext = context.RequestServices.GetRequiredService<IHubContext<MySignalRHub>>();
       await hubContext.Clients.All.SendAsync(method, message.RootElement.ToString());

       return Results.StatusCode(200);
   })
   .WithTags("Signalr Message Sender")
   .WithOpenApi();
app.UseSwaggerUI();
app.Run();

public class MySignalRHub : Hub
{
}