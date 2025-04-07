using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace FSPBook.Portal.Services;

public class NewsArticle
{
    public string Title { get; set; }
    public string Url { get; set; }
    public string Source { get; set; }
    public string PublishedAt { get; set; }
}

public class NewsService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public NewsService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["NewsApi:ApiKey"];
    }

    public async Task<List<NewsArticle>> GetTechNewsAsync()
    {
        var url = $"https://api.thenewsapi.com/v1/news/top?api_token={_apiKey}&categories=tech&limit=5";
        var response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var newsResponse = JsonSerializer.Deserialize<NewsApiResponse>(content, options);
            return newsResponse?.Data ?? [];
        }

        return new List<NewsArticle>();
    }
    private class NewsApiResponse
    {
        public List<NewsArticle> Data { get; set; }
    }
}