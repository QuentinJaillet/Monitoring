using MediatR;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using ServiceB.Application.GetAuthors;
using ServiceB.Application.GetBooks;
using ServiceB.Application.ViewModels;
using ServiceB.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
const string serviceName = "service-b";

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
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation())
    .WithMetrics(metrics => metrics
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation());

var app = builder.Build();

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

// Book API
app.MapGet("/books", async (IMediator mediator) =>
    {
        var books = await mediator.Send(new GetBooksQuery());
        return Results.Ok(books);
    }).WithName("GetBooks")
    .Produces<IList<BookViewModel>>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status500InternalServerError);

app.Run();