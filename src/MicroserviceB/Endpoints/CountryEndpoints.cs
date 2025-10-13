using MicroserviceB.Layers.Application;
using MicroserviceB.Layers.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using OpenTelemetry;
using OpenTelemetry.Trace;
using System;

namespace MicroserviceB.Endpoints;

public static class CountryEndpoints
{
    public static WebApplication AddCountryEndpoints(this WebApplication app)
    {
        var peopleApi = app.MapGroup("/countries");
        peopleApi.WithDisplayName("Microservice B - Countries and Addresses");
        peopleApi.MapGet("/", GetCountries);
        return app;
    }

    public static async Task<List<Country>> GetCountries(
        GetCountriesUseCase getCountriesUseCase,
        TracerProvider tracerProvider,
        CancellationToken cancellation)
    {
        return getCountriesUseCase.GetCountries();
    }

}
