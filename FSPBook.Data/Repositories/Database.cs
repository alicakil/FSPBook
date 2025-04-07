using Microsoft.EntityFrameworkCore;
using System;

namespace FSPBook.Data.Repositories
{
    public class Database(DbContextOptions<Context> options) : Context(options)
    {
        private readonly Lazy<PostRepository> _postRepository = new(() => new PostRepository());
        private readonly Lazy<ProfileRepository> _profileRepository = new(() => new ProfileRepository());

        public PostRepository Posts => _postRepository.Value;
        public ProfileRepository Profiles => _profileRepository.Value;
    }
}
