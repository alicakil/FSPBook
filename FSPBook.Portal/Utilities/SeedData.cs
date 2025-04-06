using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FSPBook.Portal.Utilities;
internal static class SeedData
{
    private static readonly string exampleText = @"Lorem Ipsum is simply dummy text of the printing and typesetting 
    industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an 
    unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived 
    not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.";

    public static void Seed(IServiceProvider serviceProvider)
    {
        using var context = new Context(serviceProvider.GetRequiredService<DbContextOptions<Context>>());

        if (!context.Profiles.Any())
        {
            var profiles = new List<Profile> {
                new() { Id = 1, FirstName = "Jane", LastName = "Doe", JobTitle = "Developer" },
                new() { Id = 2, FirstName = "John", LastName = "Smith", JobTitle = "Consultant" },
                new() { Id = 3, FirstName = "Maisie", LastName = "Jones", JobTitle = "Project Manager" },
                new() { Id = 4, FirstName = "Jack", LastName = "Simpson", JobTitle = "Engagement Officer" },
                new() { Id = 5, FirstName = "Sadie", LastName = "Williams", JobTitle = "Finance Director" },
                new() { Id = 6, FirstName = "Pete", LastName = "Jackson", JobTitle = "Developer" },
                new() { Id = 7, FirstName = "Sinead", LastName = "O'Leary", JobTitle = "Consultant" }
            };
            context.Profiles.AddRange(profiles);

            var posts = new List<Post> {
                new() { Id = 1, Content = exampleText, DateTimePosted = new DateTimeOffset(2020, 11, 1, 10, 0, 0, TimeSpan.Zero), AuthorId = 1 },
                new() { Id = 2, Content = exampleText, DateTimePosted = new DateTimeOffset(2020, 11, 1, 15, 0, 0, TimeSpan.Zero), AuthorId = 2 }
            };

            context.Posts.AddRange(posts);
            context.SaveChanges();
        }
    }
}