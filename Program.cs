using System.Xml.Schema;
using TestServer;

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
app.MapHub<SignalRHub>(hubUri);
app.AddRestApis();

app.UseSwaggerUI();
app.Run();