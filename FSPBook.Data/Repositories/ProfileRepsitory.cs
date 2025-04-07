using FSPBook.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FSPBook.Data.Repositories;

public class ProfileRepository(Context context) : Repository<Profile>(context), IProfileRepository
{
    public async Task<Profile> GetProfileWithPostsAsync(int id)
    {
        return await _context.Profiles
            .Include(p => p.Posts)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}