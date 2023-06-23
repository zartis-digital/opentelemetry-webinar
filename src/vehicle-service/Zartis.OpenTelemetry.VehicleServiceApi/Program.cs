using Clients;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using System.Diagnostics.Metrics;
using Zartis.OpenTelemetry.VehicleServiceApi.Persistence;
using Zartis.OpenTelemetry.VehicleServiceApi.Services;

const string signozBackendIpAddress = "172.25.25.64";
const string serviceVersion = "1.0.0";

var serviceName = AppDomain.CurrentDomain.FriendlyName;
var meter = new Meter(serviceName);

var builder = WebApplication.CreateBuilder(args);
var appResourceBuilder = ResourceBuilder.CreateDefault().AddService(serviceName: serviceName, serviceVersion: serviceVersion);

// Add services to the container.
builder.Services.AddTransient<VehicleService>();

// Persistence
builder.Services.AddDbContext<VehiclesContext>(opt => opt.UseInMemoryDatabase("VehiclesPersistence"));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Open telemetry
builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
    {
        tracerProviderBuilder
             .AddSource(serviceName)
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName: serviceName, serviceVersion: serviceVersion))
            .AddHttpClientInstrumentation()
            .AddAspNetCoreInstrumentation()
            .AddSqlClientInstrumentation()
            .AddEntityFrameworkCoreInstrumentation(opt =>
            {
                opt.SetDbStatementForText = true; // trace sql statements
                opt.SetDbStatementForStoredProcedure = true; // trace stored procedures
            })
            .AddConsoleExporter()
            .AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri($"http://{signozBackendIpAddress}:4317");
            });
    })
    .WithMetrics(metricProviderBuilder =>
    {
        metricProviderBuilder
            .AddConsoleExporter()
            .AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri($"http://{signozBackendIpAddress}:4317");
            })
            .AddMeter(meter.Name)
            .SetResourceBuilder(appResourceBuilder)
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation();
    });

// Http client
builder.Services.AddHttpClient();

// External dependencies
builder.Services.AddTransient<ComponentsApiClient>(); // Component micro service client

// Serilog 
var configuration = new ConfigurationBuilder()
    // Read from your appsettings.json.
    .AddJsonFile("appsettings.json")
    // Read from your secrets.
    .AddUserSecrets<Program>(optional: true)
    .AddEnvironmentVariables()
    .Build();

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

builder.Host
    .ConfigureLogging(confirguration =>
    {
        confirguration.AddSerilog(logger); // using serilog as the logger via Logging Abstractions
        confirguration.AddOpenTelemetry(options => // adds open telemetry log feature    
        {
            options.AddConsoleExporter();
            options.AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri($"http://{signozBackendIpAddress}:4317");
            });
        });
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
