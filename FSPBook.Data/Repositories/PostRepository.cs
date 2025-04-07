using FSPBook.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FSPBook.Data.Repositories;

public class PostRepository(Context context) : Repository<Post>(context), IPostRepository
{
    public async Task<IEnumerable<Post>> GetPostsByAuthorIdAsync(int authorId)
    {
        return await _context.Posts
            .Where(p => p.AuthorId == authorId)
            .OrderByDescending(p => p.DateTimePosted)
            .ToListAsync();
    }

    public async Task<IEnumerable<Post>> GetLatestPostsAsync(int count, int skip = 0)
    {
        return await _context.Posts
            .Include(p => p.Author)
            .OrderByDescending(p => p.DateTimePosted)
            .Skip(skip)
            .Take(count)
            .ToListAsync();
    }

    public async Task<int> GetTotalPostsCountAsync()
    {
        return await _context.Posts.CountAsync();
    }
}
