using FSPBook.Portal.Services.NewsService;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FSPBook.Portal.Pages;
public class IndexModel(ILogger<IndexModel> logger, Context dbContext, NewsService newsService) : PageModel
{
    private readonly ILogger<IndexModel> _logger = logger;
    public List<NewsArticle> NewsArticles { get; set; }


    public List<Post> Posts { get; set; }
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalPosts { get; set; }

    public async Task OnGetAsync(int? pageNumber)
    {
        CurrentPage = pageNumber ?? 1;

        TotalPosts = await dbContext.Posts.CountAsync();

        Posts = await dbContext.Posts
            .Include(p => p.Author)
            .OrderByDescending(p => p.DateTimePosted)
            .Skip((CurrentPage - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();

        // Get tech news
        NewsArticles = await newsService.GetTechNewsAsync();
    }
}