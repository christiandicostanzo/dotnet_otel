using MicroserviceB.Endpoints;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();
builder.Services.AddScoped<MicroserviceB.Layers.Infrastructure.CountryRespository>();
builder.Services.AddScoped<MicroserviceB.Layers.Application.GetCountriesUseCase>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("MicroserviceB"))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddOtlpExporter()
            .AddSource("MicroserviceB")
            .AddJaegerExporter(options =>
            {
                options.AgentHost = "localhost";
                options.AgentPort = 6831;
            })
            .AddConsoleExporter();
    });


builder.Services.AddOpenApi();

var app = builder.Build();

app.AddCountryEndpoints();

app.Run();
