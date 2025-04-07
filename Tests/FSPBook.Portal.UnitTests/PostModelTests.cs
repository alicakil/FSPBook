using FSPBook.Data;
using FSPBook.Data.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FSPBook.Portal.UnitTests;

// Minimal implementation of a PostModel PageModel for testing purposes.
public class PostModel(Context context) : PageModel
{
    public Post? Post { get; private set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Post = await context.Posts
                    .Include(p => p.Author)
                    .FirstOrDefaultAsync(p => p.Id == id);

        if (Post is null)
        {
            return new NotFoundResult();
        }

        return new PageResult();
    }
}

public class PostModelTests
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
        using var context = GetInMemoryContext("TestDatabase_Post_Valid");
        var author = new Profile
        {
            Id = 1,
            FirstName = "Author",
            LastName = "One",
            JobTitle = "Writer"
        };

        context.Profiles.Add(author);
        context.Posts.Add(new Post
        {
            Id = 1,
            Content = "Post content",
            DateTimePosted = DateTimeOffset.Now,
            AuthorId = 1,
            Author = author
        });
        await context.SaveChangesAsync();

        var pageModel = new PostModel(context);

        // Act
        var result = await pageModel.OnGetAsync(1);

        // Assert using FluentAssertions
        pageModel.Post.Should().NotBeNull();
        pageModel.Post!.Author.Should().NotBeNull();
        result.Should().BeOfType<PageResult>();
    }

    [Fact]
    public async Task OnGetAsync_WithInvalidId_ReturnsNotFoundResult()
    {
        // Arrange
        using var context = GetInMemoryContext("TestDatabase_Post_Invalid");
        var pageModel = new PostModel(context);

        // Act
        var result = await pageModel.OnGetAsync(99);

        // Assert using FluentAssertions
        pageModel.Post.Should().BeNull();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task OnGetAsync_PostIncludesAuthorDetails()
    {
        // Arrange
        using var context = GetInMemoryContext("TestDatabase_Post_Author");
        var author = new Profile
        {
            Id = 1,
            FirstName = "Jane",
            LastName = "Doe",
            JobTitle = "Editor"
        };

        context.Profiles.Add(author);
        context.Posts.Add(new Post
        {
            Id = 2,
            Content = "Detailed post content",
            DateTimePosted = DateTimeOffset.Now,
            AuthorId = 1,
            Author = author
        });
        await context.SaveChangesAsync();

        var pageModel = new PostModel(context);

        // Act
        var result = await pageModel.OnGetAsync(2);

        // Assert using FluentAssertions
        pageModel.Post.Should().NotBeNull();
        pageModel.Post!.Content.Should().Be("Detailed post content");
        pageModel.Post!.Author.Should().NotBeNull();
        pageModel.Post!.Author.FirstName.Should().Be("Jane");
        pageModel.Post!.Author.LastName.Should().Be("Doe");
        result.Should().BeOfType<PageResult>();
    }
}
