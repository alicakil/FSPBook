using System.Collections.Generic;

namespace FSPBook.Portal.Services.NewsService;

public class NewsArticle
{
    public string Title { get; set; }
    public string Url { get; set; }
    public string Source { get; set; }
    public string PublishedAt { get; set; }
}

public class NewsApiResponse
{
    public List<NewsArticle> Data { get; set; }
}
