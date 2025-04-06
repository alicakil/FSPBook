using FSPBook.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FSPBook.Data;
public class Context(DbContextOptions<Context> options) : DbContext(options)
{
    public virtual DbSet<Profile> Profiles { get; set; }
    public virtual DbSet<Post> Posts { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Profile>()
            .HasMany(p => p.Posts)
            .WithOne(p => p.Author)
            .HasForeignKey(p => p.AuthorId);
    }
}