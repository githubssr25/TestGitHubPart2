using Microsoft.EntityFrameworkCore;

namespace TestDemo
{
    public class GitHubPostMVPDbContext : DbContext
    {
        public GitHubPostMVPDbContext(DbContextOptions<GitHubPostMVPDbContext> options) : base(options)
        {
        }

        // Define your DbSets here
        public DbSet<Repository> Repositories { get; set; }
        public DbSet<GitHubIssue> Issues { get; set; }
    }
}
