using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace FSPBook.Portal.Pages;
public class EditProfileModel(Context context) : PageModel
{
    private readonly Context _context = context;

    [BindProperty]
    public Profile ProfileData { get; set; }

    public bool Success { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        ProfileData = await _context.Profiles.FindAsync(id);

        if (ProfileData == null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var profileToUpdate = await _context.Profiles.FindAsync(ProfileData.Id);

        if (profileToUpdate == null)
        {
            return NotFound();
        }

        profileToUpdate.FirstName = ProfileData.FirstName;
        profileToUpdate.LastName = ProfileData.LastName;
        profileToUpdate.JobTitle = ProfileData.JobTitle;

        await _context.SaveChangesAsync();
        Success = true;

        return Page();
    }
}