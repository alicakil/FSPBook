using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FSPBook.Portal.Pages;
public class ProfileDetailModel(Context context) : PageModel
{
    public Profile UserProfile { get; set; }
    public List<Post> Posts { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        UserProfile = await context.Profiles.FindAsync(id);

        if (UserProfile == null)
        {
            return NotFound();
        }

        Posts = await context.Posts
            .Where(p => p.AuthorId == id)
            .OrderByDescending(p => p.DateTimePosted)
            .ToListAsync();

        return Page();
    }
}