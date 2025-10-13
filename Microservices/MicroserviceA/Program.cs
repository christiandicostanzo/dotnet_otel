using MicroserviceA.Endpoints;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();
builder.Services.AddScoped<MicroserviceA.Layers.Infrastructure.PeopleRespository>();
builder.Services.AddScoped<MicroserviceA.Layers.Application.GetPeopleUseCase>();
builder.Services.AddScoped<MicroserviceA.Layers.Application.GetPeopleUseCase>();
builder.Services.AddHttpClient();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("MicroserviceA"))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddOtlpExporter()
            .AddSource("MicroserviceA")
            .AddJaegerExporter(options =>
            {
                options.AgentHost = "localhost";
                options.AgentPort = 6831;
            })
            .AddConsoleExporter();
    });


builder.Services.AddOpenApi();

var app = builder.Build();

app.AddPeopleEndpoints();

app.Run();


