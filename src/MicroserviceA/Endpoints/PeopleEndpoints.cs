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

    public static async Task<List<Person>> GetPeople(
        GetPeopleUseCase getPeopleUseCase,
        TracerProvider tracerProvider,
        CancellationToken cancellation)
    {
        return await Task.Run(() => getPeopleUseCase.GetPeople(), cancellation);
    }

    public static async Task<Person> GetPersonById(
        int personId,
        GetPeopleUseCase getPeopleUseCase,
        TracerProvider tracerProvider,
        CancellationToken cancellation)
    {
        return await Task.Run(() => getPeopleUseCase.GetPersonById(personId), cancellation);
    }
}
