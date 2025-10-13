using MicroserviceB.Layers.Models;
using Microsoft.Extensions.Caching.Memory;
using OpenTelemetry.Trace;
using System;
using System.Diagnostics;

namespace MicroserviceB.Layers.Infrastructure;

public class CountryRespository
{

    private readonly IMemoryCache _memoryCache;
    private const string CountriesCacheKey = "Countries";
    private readonly ActivitySource _activitySource;

    public CountryRespository(ActivitySource activitySource, IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
        _activitySource = activitySource;
    }

    public List<Country> GetCountries()
    {
        using var activity = _activitySource.StartActivity("CountryRespository.GetCountries", ActivityKind.Internal);
        try
        {
            if (_memoryCache.TryGetValue(CountriesCacheKey, out List<Country> cachedCountries))
            {
                activity.SetTag("cache.hit", true);
                activity.SetTag("country.count", cachedCountries.Count);
                return cachedCountries;
            }

            List<Country> initialCountries = new()
            {
                new Country { Name = "United States", CountryCode = "US" },
                new Country { Name = "United Kingdom", CountryCode = "GB" },
                new Country { Name = "Japan", CountryCode = "JP" },
                new Country { Name = "Germany", CountryCode = "DE" },
                new Country { Name = "France", CountryCode = "FR" },
                new Country { Name = "Italy", CountryCode = "IT" },
                new Country { Name = "Canada", CountryCode = "CA" },
                new Country { Name = "Australia", CountryCode = "AU" },
                new Country { Name = "Brazil", CountryCode = "BR" },
                new Country { Name = "Spain", CountryCode = "ES" }
            };

            _memoryCache.Set(CountriesCacheKey, initialCountries);

            activity.SetTag("cache.hit", false);
            activity.SetTag("country.count", initialCountries.Count);
            return initialCountries;
        }
        catch (Exception ex)
        {
            activity.SetTag("otel.status_code", "ERROR");
            activity.SetTag("otel.status_description", ex.Message);
            throw;
        }
    }

}
