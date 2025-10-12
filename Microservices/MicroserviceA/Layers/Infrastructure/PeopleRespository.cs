using MicroserviceA.Layers.Models;
using OpenTelemetry.Trace;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace MicroserviceA.Layers.Infrastructure;

public class PeopleRespository
{
    
    private readonly IMemoryCache _memoryCache;
    private const string PeopleCacheKey = "PeopleList";
    private readonly ActivitySource _activitySource;

    public PeopleRespository(ActivitySource activitySource, IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
        _activitySource = activitySource;
    }

    public List<Person> GetPeople()
    {
        using var activity = _activitySource.StartActivity("PeopleRespository.GetPeople", ActivityKind.Internal);
        try
        {
            if (_memoryCache.TryGetValue(PeopleCacheKey, out List<Person> cachedPeople))
            {
                activity.SetTag("cache.hit", true);
                activity.SetTag("people.count", cachedPeople.Count);
                return cachedPeople;
            }
            
            var initialPeople = new List<Person>
            {
                new Person { Id = 1, Name = "Alice", Age = 30 },
                new Person { Id = 2, Name = "Bob", Age = 25 },
                new Person { Id = 3, Name = "Charlie", Age = 35 }
            };

            _memoryCache.Set(PeopleCacheKey, initialPeople);

            activity.SetTag("cache.hit", false);
            activity.SetTag("people.count", initialPeople.Count);
            return initialPeople;
        }
        catch (Exception ex)
        {
            activity.SetTag("otel.status_code", "ERROR");
            activity.SetTag("otel.status_description", ex.Message);
            throw;
        }
    }

    public Person? GetPersonById(int id)
    {
        using var activity = _activitySource.StartActivity("PeopleRespository.GetPersonById", ActivityKind.Internal);
        activity.SetTag("person.id", id);
        try
        {
            if (_memoryCache.TryGetValue(PeopleCacheKey, out List<Person> cachedPeople))
            {
                var person = cachedPeople.FirstOrDefault(p => p.Id == id);
                activity.SetTag("cache.hit", true);
                activity.SetTag("person.found", person != null);
                return person;
            }

            activity.SetTag("cache.hit", false);
            return null;
        }
        catch (Exception ex)
        {
            activity.SetTag("otel.status_code", "ERROR");
            activity.SetTag("otel.status_description", ex.Message);
            throw;
        }
    }
}
