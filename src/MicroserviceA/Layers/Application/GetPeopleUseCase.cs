using MicroserviceA.Layers.Infrastructure;
using MicroserviceA.Layers.Models;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Trace;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace MicroserviceA.Layers.Application;

public class GetPeopleUseCase
(
    TracerProvider tracerProvider,
    PeopleRespository peopleRespository,
    ActivitySource activitySource,
    IHttpClientFactory _httpClientFactory
)
{
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

            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5229/countries");

            Propagators.DefaultTextMapPropagator.Inject(
                new PropagationContext(activity.Context, Baggage.Current), 
                request, 
                (r, name, value) => r.Headers.Add(name, value));

            var countries = client.SendAsync(request).Result;

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
