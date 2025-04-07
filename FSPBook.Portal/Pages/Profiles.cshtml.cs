using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FSPBook.Portal.Pages;
public class ProfileModel(Context context) : PageModel
{
    public List<Profile> Profiles { get; set; }

    public async Task OnGetAsync()
    {
        Profiles = await context.Profiles
            .Include(p => p.Posts)
            .ToListAsync();
    }
}