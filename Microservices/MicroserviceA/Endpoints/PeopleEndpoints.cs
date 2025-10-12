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

        peopleApi
            .WithDisplayName("Microservice A - People Api");
            //.AddEndpointFilter(async (context, next) =>
            //{
            //    var tracer = context.HttpContext.RequestServices.GetService<TracerProvider>()?.GetTracer("MicroserviceA.PeopleEndpoints");
            //    var span = tracer?.StartSpan("PeopleEndpointRequest");
            //    try
            //    {
            //        span?.SetAttribute("http.method", context.HttpContext.Request.Method);
            //        span?.SetAttribute("http.path", context.HttpContext.Request.Path);
            //        span?.SetAttribute("christian.dicostanzo", "MY ATTRIBUTE");

            //        var result = await next(context);
            //        span?.SetAttribute("http.status_code", context.HttpContext.Response.StatusCode);
            //        return result;
            //    }
            //    catch (Exception ex)
            //    {
            //        span?.SetAttribute("otel.status_code", "ERROR");
            //        span?.SetAttribute("otel.status_description", ex.Message);
            //        throw;
            //    }
            //    finally
            //    {
            //        span?.End();
            //    }
            //});

        peopleApi.MapGet("/", GetPeople);
        peopleApi.MapGet("/{personId}", GetPersonById);

        return app;
    }

    public static async Task<List<Person>> GetPeople(
        GetPeopleUseCase getPeopleUseCase,
        TracerProvider tracerProvider,
        CancellationToken cancellation)
    {
        return getPeopleUseCase.GetPeople();
    }

    public static async Task<Person> GetPersonById(
        int personId,
         GetPeopleUseCase getPeopleUseCase,
        TracerProvider tracerProvider,
        CancellationToken cancellation)
    {
        return getPeopleUseCase.GetPersonById(personId);
    }

}
