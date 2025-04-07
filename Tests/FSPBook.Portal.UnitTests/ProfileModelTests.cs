using FSPBook.Data;
using FSPBook.Data.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FSPBook.Portal.UnitTests
{
    public class ProfileModel(Context context) : PageModel
    {
        public Profile? Profile { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Profile = await context.Profiles
                .Include(p => p.Posts)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (Profile is null)
            {
                return new NotFoundResult();
            }

            return new PageResult();
        }
    }

    public class ProfileModelTests
    {
        private static Context GetInMemoryContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName)
                .Options;

            return new Context(options);
        }

        [Fact]
        public async Task OnGetAsync_WithValidId_ReturnsPageResult()
        {
            // Arrange
            using var context = GetInMemoryContext("TestDatabase_Profile_Valid");
            context.Profiles.Add(new Profile
            {
                Id = 1,
                FirstName = "Test",
                LastName = "User",
                JobTitle = "Developer"
            });
            await context.SaveChangesAsync();

            var pageModel = new ProfileModel(context);

            // Act
            var result = await pageModel.OnGetAsync(1);

            // Assert
            pageModel.Profile.Should().NotBeNull();
            result.Should().BeOfType<PageResult>();
        }

        [Fact]
        public async Task OnGetAsync_WithInvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            using var context = GetInMemoryContext("TestDatabase_Profile_Invalid");
            var pageModel = new ProfileModel(context);

            // Act
            var result = await pageModel.OnGetAsync(99);

            // Assert
            pageModel.Profile.Should().BeNull();
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task OnGetAsync_ProfileHasPosts()
        {
            // Arrange
            using var context = GetInMemoryContext("TestDatabase_Profile_Posts");

            var profile = new Profile
            {
                Id = 1,
                FirstName = "Test",
                LastName = "User",
                JobTitle = "Developer"
            };

            context.Profiles.Add(profile);
            context.Posts.Add(new Post
            {
                Id = 1,
                Content = "First post",
                DateTimePosted = DateTimeOffset.Now,
                AuthorId = 1,
                Author = profile
            });
            await context.SaveChangesAsync();

            var pageModel = new ProfileModel(context);

            // Act
            var result = await pageModel.OnGetAsync(1);

            // Assert
            pageModel.Profile.Should().NotBeNull();
            pageModel.Profile!.Posts.Should().NotBeNull().And.NotBeEmpty();
            result.Should().BeOfType<PageResult>();
        }
    }
}
