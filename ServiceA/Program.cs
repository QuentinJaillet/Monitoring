using System.Diagnostics;
using System.Diagnostics.Metrics;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using ServiceA;

var builder = WebApplication.CreateBuilder(args);

const string serviceName = "service-a";
var serviceVersion = "1.0.0";

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add OpenTelemetry
builder.Logging.AddOpenTelemetry(options =>
{
    options.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName)).AddConsoleExporter();
});

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(serviceName))
    .WithTracing(tracing => tracing
        .AddSource(serviceName)
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddOtlpExporter())
    .WithMetrics(metrics => metrics
        .AddMeter(serviceName)
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddProcessInstrumentation()
        .AddRuntimeInstrumentation()
        .AddPrometheusExporter()
        .AddOtlpExporter());

builder.Logging.AddOpenTelemetry(options => options
    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(
        serviceName: serviceName,
        serviceVersion: serviceVersion))
    .AddOtlpExporter());

var app = builder.Build();

app.MapPrometheusScrapingEndpoint();
app.UseOpenTelemetryPrometheusScrapingEndpoint();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", (IMeterFactory meterFactory) =>
    {
        app.Logger.LogInformation("Weather forecast");
        
        /*var meter = meterFactory.Create(configuration["BookStoreMeterName"] ?? 
                                       throw new NullReferenceException("BookStore meter missing a name"));
        
        BooksAddedCounter = meter.CreateCounter<int>("books-added", "Book");*/

        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

// This is a sample of how to use the OpenTelemetry API to create a span
app.MapGet("/activity", (ActivitySource activitySource) =>
{
    using var myActivity1 = activitySource.StartActivity("MyActivity1", ActivityKind.Client);
    app.Logger.LogInformation("Activity1");
    
    myActivity1?.SetTag("user.id", Guid.NewGuid().ToString());
    myActivity1?.SetTag("commande.id", Guid.NewGuid().ToString());
    
    // Simulate some work
    Thread.Sleep(1000);
    myActivity1.Stop();
    
    using var myActivity2 = activitySource.StartActivity("MyActivity2", ActivityKind.Server);
    app.Logger.LogInformation("Activity2");
    
    myActivity2?.SetTag("user.id", Guid.NewGuid().ToString());
    myActivity2?.SetTag("commande.id", Guid.NewGuid().ToString());

    var forecast = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
            ))
        .ToArray();
    return forecast;
}).WithName("Activity").WithOpenApi();

// Failed Test
app.MapGet("/failed", () =>
{
    app.Logger.LogInformation("Failed");
    throw new Exception("Failed");
}).WithName("Failed").WithOpenApi();


app.Run();

namespace ServiceA
{
    record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}