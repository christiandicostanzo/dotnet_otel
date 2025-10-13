using MicroserviceB.Layers.Infrastructure;
using MicroserviceB.Layers.Models;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Trace;
using System;
using System.Diagnostics;

namespace MicroserviceB.Layers.Application;

public class GetCountriesUseCase
(
    TracerProvider tracerProvider,
    CountryRespository countryRespository,
    ActivitySource activitySource,
    IHttpContextAccessor _httpContextAccessor
)
{

    public List<Country> GetCountries()
    {

        var propagator = Propagators.DefaultTextMapPropagator;
        var parentContext = propagator.Extract(default, 
            _httpContextAccessor.HttpContext.Request, 
            (r, name) => r.Headers[name]);

        Baggage.Current = parentContext.Baggage;

        using var activity = activitySource.StartActivity("GetCountriesUseCase.GetCountries", ActivityKind.Server, parentContext.ActivityContext);
        try
        {
            var countries = countryRespository.GetCountries();
            Console.WriteLine($"Trace ID: {Activity.Current?.TraceId}");
            activity?.SetTag("country.count", countries.Count);
            return countries;
        }
        catch (Exception ex)
        {
            activity?.SetTag("otel.status_code", "ERROR");
            activity?.SetTag("otel.status_description", ex.Message);
            throw;
        }
    }

}
