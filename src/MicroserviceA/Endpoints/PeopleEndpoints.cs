using Microsoft.AspNetCore.Http.HttpResults;
using OpenTelemetry;
using OpenTelemetry.Trace;
using MicroserviceA.Layers.Models;
using MicroserviceA.Layers.Application;

namespace MicroserviceA.Endpoints;

public static class PeopleEndpoints
{
    public static WebApplication AddPeopleEndpoints(this WebApplication app)
    {
        var peopleApi = app.MapGroup("/people");
        peopleApi.WithDisplayName("Microservice A - People Api");
        peopleApi.MapGet("/", GetPeople);
        peopleApi.MapGet("/{personId}", GetPersonById);
        return app;
    }

    /// <summary>
    /// Retrieves the list of people.
    /// </summary>
    /// <param name="getPeopleUseCase">Service that provides methods to fetch people.</param>
    /// <param name="tracerProvider">OpenTelemetry tracer provider for distributed tracing.</param>
    /// <param name="cancellation">Token to observe while waiting for the operation to complete.</param>
    /// <returns>The list of <see cref="Person"/> returned by the use case.</returns>
    public static async Task<List<Person>> GetPeople(
        GetPeopleUseCase getPeopleUseCase,
        TracerProvider tracerProvider,
        CancellationToken cancellation)
    {
        try
        {
            return getPeopleUseCase.GetPeople();    
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Fetches the person with the specified identifier.
    /// </summary>
    /// <returns>The Person matching the provided `personId`.</returns>
    public static async Task<Person> GetPersonById(
        int personId,
        GetPeopleUseCase getPeopleUseCase,
        TracerProvider tracerProvider,
        CancellationToken cancellation)
    {
        return await Task.Run(() => getPeopleUseCase.GetPersonById(personId), cancellation);
    }

}