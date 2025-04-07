using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace FSPBook.Portal.Services.NewsService;

public class NewsService(HttpClient httpClient, IConfiguration configuration, IMemoryCache memoryCache)
{
    // Api information
    private readonly string _apiKey = configuration["NewsApi:ApiKey"];
    private readonly string _baseUrl = configuration["NewsApi:BaseUrl"];

    // cache configuration
    private const string CacheKey = "TechNewsCache";
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(double.Parse(configuration["NewsApi:ExpireInMinutes"]));      
    private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public async Task<List<NewsArticle>> GetTechNewsAsync()
    {
        if (memoryCache.TryGetValue(CacheKey, out List<NewsArticle> cachedNews))
        {
            return cachedNews;
        }

        var url = $"{_baseUrl}/news/top?api_token={_apiKey}&categories=tech&limit=5";
        var response = await httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var newsResponse = JsonSerializer.Deserialize<NewsApiResponse>(content, _jsonSerializerOptions);
            var news = newsResponse?.Data ?? [];

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(_cacheDuration);

            memoryCache.Set(CacheKey, news, cacheEntryOptions);
            return news;
        }
        return [];
    }
}