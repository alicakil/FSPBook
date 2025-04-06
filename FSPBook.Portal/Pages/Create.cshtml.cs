using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FSPBook.Portal.Pages;
public class CreateModel(Context context) : PageModel
{
    public Context DbContext { get; set; } = context;

    public List<Profile> Profiles { get; set; }

    public bool Success { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Choose a person to post on behalf of")]
    [Range(1, 10000, ErrorMessage = "Choose a person to post on behalf of")]
    public int ProfileId { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Write a post")]
    [MinLength(1, ErrorMessage = "Post needs some content")]
    public string ContentInput { get; set; }

    public async Task OnGetAsync()
    {
        await LoadProfiles();
    }

    public async Task OnPostAsync()
    {
        if (ProfileId != -1)
        {
            DbContext.Posts.Add(new Post { AuthorId = ProfileId, Content = ContentInput, DateTimePosted = DateTimeOffset.Now });
            await DbContext.SaveChangesAsync();
            Success = true;
        }

        await LoadProfiles();
    }

    private async Task LoadProfiles()
    {
        Profiles = await DbContext.Profiles.ToListAsync();
    }
}
