using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;

namespace VirtoCommerce.GithubOAuth.Data.Repositories
{
    public class GithubOAuthDbContext : DbContextWithTriggers
    {
        public GithubOAuthDbContext(DbContextOptions<GithubOAuthDbContext> options)
            : base(options)
        {
        }

        protected GithubOAuthDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<GithubOAuthEntity>().ToTable("GithubOAuth").HasKey(x => x.Id);
            //modelBuilder.Entity<GithubOAuthEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();
        }
    }
}
