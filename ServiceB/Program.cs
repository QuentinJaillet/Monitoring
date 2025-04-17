using MediatR;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using ServiceB.Application.Auhtor.AddAuthor;
using ServiceB.Application.Auhtor.GetAuthor;
using ServiceB.Application.Auhtor.GetAuthors;
using ServiceB.Application.Book.GetBooks;
using ServiceB.Application.ViewModels;
using ServiceB.Infrastructure;
using Serilog;
using Serilog.Formatting.Compact;

var builder = WebApplication.CreateBuilder(args);
const string serviceName = "service-b";
const string serviceVersion = "1.0.0";

// Configure Serilog for logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(
        new RenderedCompactJsonFormatter(),
        "Logs/serviceB_log.txt",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

// Log Information about application start
Log.Information("Starting {ServiceName} version {ServiceVersion}", serviceName, serviceVersion);

// Mediator        
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Configuration de la base de données SqlLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=books.db"));

// Add services to the container.
builder.Services.AddInfrastructure();

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

// DataBase seeding
using var scope = app.Services.CreateScope();
//var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
await DataSeedExtension.SeedDataAsync(scope.ServiceProvider);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Author API
app.MapGet("/authors", async (IMediator mediator) =>
    {
        var authors = await mediator.Send(new GetAuthorsQuery());
        return Results.Ok(authors);
    }).WithName("GetAuthors")
    .Produces<IList<AuthorViewModel>>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status500InternalServerError);

// Add a new author
app.MapPost("/authors", async (IMediator mediator, string firstname, string lastname, string picture) =>
    {
        var authorId = await mediator.Send(new AddAuthorCommand(firstname, lastname, picture));
        
        return authorId == Guid.Empty ? 
            Results.BadRequest("Author already exists") : 
            Results.Created($"/authors/{authorId}", authorId);
    }).WithName("AddAuthor")
    .Produces<Guid>(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status500InternalServerError);

// Get author by ID
app.MapGet("/authors/{id:guid}", async (IMediator mediator, Guid id) =>
    {
        var author = await mediator.Send(new GetAuthorQuery(id));
        return Results.Ok(author);
    }).WithName("GetAuthor")
    .Produces<AuthorViewModel>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status500InternalServerError);

// Book API
app.MapGet("/books", async (IMediator mediator) =>
    {
        var books = await mediator.Send(new GetBooksQuery());
        return Results.Ok(books);
    }).WithName("GetBooks")
    .Produces<IList<BookViewModel>>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status500InternalServerError);

app.Run();