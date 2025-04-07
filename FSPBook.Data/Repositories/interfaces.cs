using FSPBook.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FSPBook.Data.Repositories;
public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task<T> AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task SaveChangesAsync();
}

public interface IProfileRepository : IRepository<Profile>
{
    Task<Profile> GetProfileWithPostsAsync(int id);
}

public interface IPostRepository : IRepository<Post>
{
    Task<IEnumerable<Post>> GetPostsByAuthorIdAsync(int authorId);
    Task<IEnumerable<Post>> GetLatestPostsAsync(int count, int skip = 0);
    Task<int> GetTotalPostsCountAsync();
}