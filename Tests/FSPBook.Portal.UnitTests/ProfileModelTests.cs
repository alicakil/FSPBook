using FSPBook.Data;
using FSPBook.Data.Entities;
using FSPBook.Portal.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FSPBook.Portal.UnitTests
{
    public class ProfileModelTests
    {
        [Fact]
        public async Task OnGetAsync_WithValidId_ReturnsPageResult()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Profile")
                .Options;

            // Create test data
            using (var context = new Context(options))
            {
                context.Profiles.Add(new Profile
                {
                    Id = 1,
                    FirstName = "Test",
                    LastName = "User",
                    JobTitle = "Developer"
                });
                context.Posts.Add(new Post
                {
                    Id = 1,
                    Content = "Test post content",
                    DateTimePosted = DateTimeOffset.Now,
                    AuthorId = 1
                });
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new Context(options))
            {
                var pageModel = new ProfileModel(context);
                var result = await pageModel.OnGetAsync(1);

                // Assert
                Assert.IsType<PageResult>(result);
                Assert.NotNull(pageModel.UserProfile);
                Assert.Equal("Test User", pageModel.UserProfile.FullName);
                Assert.Single(pageModel.Posts);
            }
        }

        [Fact]
        public async Task OnGetAsync_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_NotFound")
                .Options;

            // Act
            using var context = new Context(options);
            var pageModel = new ProfileModel(context);
            var result = await pageModel.OnGetAsync(999); // Non-existent ID

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}