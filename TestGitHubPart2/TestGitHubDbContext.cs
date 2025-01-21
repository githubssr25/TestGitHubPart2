using Microsoft.EntityFrameworkCore;
using PostMVPProject.Models;


namespace PostMVPFinalProject.Context

{
    public class GitHubPostMVPDbContext : DbContext
    {
        public GitHubPostMVPDbContext(DbContextOptions<GitHubPostMVPDbContext> options) : base(options)
        {
        }

        // Define your DbSets here
       // Define DbSets
        public DbSet<Repository> Repositories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRepository> UserRepositories { get; set; }
        public DbSet<Annotation> Annotations { get; set; }
        public DbSet<Category> Categories { get; set; }

               protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // User-Repository (Many-to-Many)
    modelBuilder.Entity<UserRepository>()
        .HasKey(ur => new { ur.UserId, ur.RepositoryId });

    modelBuilder.Entity<UserRepository>()
        .HasOne(ur => ur.User)
        .WithMany(u => u.UserRepositories)
        .HasForeignKey(ur => ur.UserId);

    modelBuilder.Entity<UserRepository>()
        .HasOne(ur => ur.Repository)
        .WithMany(r => r.UserRepositories)
        .HasForeignKey(ur => ur.RepositoryId);

    // User-Issue (Many-to-Many)
    modelBuilder.Entity<UserIssue>()
        .HasKey(ui => new { ui.UserId, ui.IssueId });

    modelBuilder.Entity<UserIssue>()
        .HasOne(ui => ui.User)
        .WithMany(u => u.UserIssues)
        .HasForeignKey(ui => ui.UserId);

    modelBuilder.Entity<UserIssue>()
        .HasOne(ui => ui.Issue)
        .WithMany(i => i.UserIssues)
        .HasForeignKey(ui => ui.IssueId);

    // Repository-Issue (One-to-Many)
    modelBuilder.Entity<Issue>()
        .HasOne(i => i.Repository)
        .WithMany(r => r.Issues)
        .HasForeignKey(i => i.RepositoryId)
        .OnDelete(DeleteBehavior.Cascade);

    // Repository-Annotation (One-to-Many)
    modelBuilder.Entity<Annotation>()
        .HasOne(a => a.Repository)
        .WithMany(r => r.Annotations)
        .HasForeignKey(a => a.RepositoryId)
        .OnDelete(DeleteBehavior.Cascade);








        //     // Composite Key for UserRepository
        //     modelBuilder.Entity<UserRepository>()
        //         .HasKey(ur => new { ur.UserId, ur.RepositoryId });

        //     // UserRepository Relationships
        //     modelBuilder.Entity<UserRepository>()
        //         .HasOne(ur => ur.User)
        //         .WithMany(u => u.UserRepositories)
        //         .HasForeignKey(ur => ur.UserId);

        //     modelBuilder.Entity<UserRepository>()
        //         .HasOne(ur => ur.Repository)
        //         .WithMany(r => r.UserRepository)
        //         .HasForeignKey(ur => ur.RepositoryId);

        //     // Annotation Relationships
        //     modelBuilder.Entity<Annotation>()
        //         .HasOne(a => a.User)
        //         .WithMany(u => u.Annotations)
        //         .HasForeignKey(a => a.UserId)
        //         .OnDelete(DeleteBehavior.Cascade);

        //     modelBuilder.Entity<Annotation>()
        //         .HasOne(a => a.Repository)
        //         .WithMany(r => r.Annotations)
        //         .HasForeignKey(a => a.RepositoryId)
        //         .OnDelete(DeleteBehavior.Cascade);

        //     // Category Relationships
        //     modelBuilder.Entity<Category>()
        //         .HasMany(c => c.Repositories)
        //         .WithOne(r => r.Category);

        //     // Repository Configuration
        //     modelBuilder.Entity<Repository>()
        //         .HasMany(r => r.Annotations)
        //         .WithOne(a => a.Repository)
        //         .HasForeignKey(a => a.RepositoryId);

        //     // User Configuration
        //     modelBuilder.Entity<User>()
        //         .HasMany(u => u.UserRepositories)
        //         .WithOne(ur => ur.User)
        //         .HasForeignKey(ur => ur.UserId);

        //     modelBuilder.Entity<User>()
        //         .HasMany(u => u.Annotations)
        //         .WithOne(a => a.User)
        //         .HasForeignKey(a => a.UserId);
        // }
        

    }
}

}