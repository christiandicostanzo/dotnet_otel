using MicroserviceA.Layers.Infrastructure;
using OpenTelemetry.Trace;
using MicroserviceA.Layers.Models;
using System.Diagnostics;

namespace MicroserviceA.Layers.Application;

public class GetPeopleUseCase
(
    TracerProvider tracerProvider,
    PeopleRespository peopleRespository,
    ActivitySource activitySource
)
{
    private readonly Tracer _tracer = tracerProvider.GetTracer("MicroserviceA.GetPeopleUseCase");

    public List<Person> GetPeople()
    {
        using var activity = activitySource.StartActivity("GetPeopleUseCase.GetPeople", ActivityKind.Internal);
        try
        {
            var people = peopleRespository.GetPeople();
            activity?.SetTag("people.count", people.Count);
            return people;
        }
        catch (Exception ex)
        {
            activity?.SetTag("otel.status_code", "ERROR");
            activity?.SetTag("otel.status_description", ex.Message);
            throw;
        }
    }

    public Person GetPersonById(int id)
    {
        using var activity = activitySource.StartActivity("GetPeopleUseCase.GetPersonById", ActivityKind.Internal);
        activity?.SetTag("person.id", id);
        try
        { 
            var person = peopleRespository.GetPersonById(id);
            activity?.SetTag("person.found", person != null);
            return person;
        }
        catch (Exception ex)
        {
            activity?.SetTag("otel.status_code", "ERROR");
            activity?.SetTag("otel.status_description", ex.Message);
            throw;
        }
    }

}
