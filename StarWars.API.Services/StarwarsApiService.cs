using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StarWars.API.Entity.Configuration;
using StarWars.API.Entity.Dto;
using StarWars.API.Entity.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace StarWars.API.Services
{
    public class StarwarsApiService :IStarwarsApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly CacheSettings _cacheSettings;
        private readonly ILogger<StarwarsApiService> _logger;
        private const string BaseUrl = "https://swapi.dev";
        private const string StarshipsCacheKey = "starships";
        private const string ManufacturersCacheKey = "manufacturers";

        public StarwarsApiService(
            HttpClient httpClient,
            IMemoryCache cache,
            IOptions<CacheSettings> cacheSettings,
            ILogger<StarwarsApiService> logger)
        {
            _httpClient = httpClient;
            _cache = cache;
            _cacheSettings = cacheSettings.Value;
            _logger = logger;
            _httpClient.BaseAddress = new Uri(BaseUrl);
        }

        public async Task<IEnumerable<StarshipResponse>> GetStarshipsAsync()
        {
            if (_cache.TryGetValue(StarshipsCacheKey, out List<StarshipResponse>? cachedStarships))
            {
                return cachedStarships;
            }

            try
            {
                var starships = new List<StarshipResponse>();
                string? nextUrl = "/api/starships/";

                while (!string.IsNullOrEmpty(nextUrl))
                {
                    var response = await _httpClient.GetFromJsonAsync<StarwarsApiResponse<StarshipResponse>>(nextUrl);
                    if (response?.Results != null)
                    {
                        starships.AddRange(response.Results);
                        nextUrl = response.Next?.Replace(BaseUrl, "");
                    }
                    else
                    {
                        break;
                    }
                }

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(_cacheSettings.StarshipsCacheMinutes));

                _cache.Set(StarshipsCacheKey, starships, cacheEntryOptions);
                return starships;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching starships from SWAPI");
                throw;
            }
        }

        public async Task<IEnumerable<string>> GetManufacturersAsync()
        {
            if (_cache.TryGetValue(ManufacturersCacheKey, out List<string>? cachedManufacturers))
            {
                return cachedManufacturers;
            }

            var starships = await GetStarshipsAsync();
            var manufacturers = starships
                .SelectMany(s => s.Manufacturer.Split(','))
                .Select(m => m.Trim())
                .Where(m => !string.IsNullOrWhiteSpace(m))
                .Distinct()
                .OrderBy(m => m)
                .ToList();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(_cacheSettings.ManufacturersCacheMinutes));

            _cache.Set(ManufacturersCacheKey, manufacturers, cacheEntryOptions);
            return manufacturers;
        }

    }
}
