using Microsoft.EntityFrameworkCore;
using PostMVPProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Text.Json; 
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;


namespace PostMVPFinalProject.Context

{
    public class GitHubPostMVPDbContext : IdentityDbContext<User>
    {
        public GitHubPostMVPDbContext(DbContextOptions<GitHubPostMVPDbContext> options) : base(options)
        {
            // Add this line to suppress the warning
        Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }

        // Define your DbSets here
       // Define DbSets
        public DbSet<Repository> Repositories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRepository> UserRepositories { get; set; }
        public DbSet<Annotation> Annotations { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Issue> Issues { get; set; }

               protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

                var passwordHasher = new PasswordHasher<User>();


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

        modelBuilder.Entity<Repository>()
    .HasOne(r => r.Category)
    .WithMany(c => c.Repositories)
    .HasForeignKey(r => r.CategoryId)
    .IsRequired(false);

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

      modelBuilder.Entity<Repository>()
        .Property(r => r.Topics)
        .HasColumnType("jsonb")
        .HasConversion(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
            v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null))
        .Metadata.SetValueComparer(new ValueComparer<List<string>>(
            (c1, c2) => c1.SequenceEqual(c2),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList()));

    modelBuilder.Entity<Issue>()
        .Property(i => i.Labels)
        .HasColumnType("jsonb")
        .HasConversion(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
            v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null))
        .Metadata.SetValueComparer(new ValueComparer<List<string>>(
            (c1, c2) => c1.SequenceEqual(c2),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList()));

         modelBuilder.Entity<Category>().HasData(
        new Category { CategoryId = 1, Description = "Backend" },
        new Category { CategoryId = 2, Description = "Frontend" },
        new Category { CategoryId = 3, Description = "Artificial Intelligence" },
        new Category { CategoryId = 4, Description = "Full Stack" },
        new Category { CategoryId = 5, Description = "Data Science" },
        new Category { CategoryId = 6, Description = "Security" },
        new Category { CategoryId = 7, Description = "Testing" },
        new Category { CategoryId = 8, Description = "Machine Learning" },
        new Category { CategoryId = 9, Description = "Other" }
    );

modelBuilder.Entity<User>().HasData(
    new User
    {
        Id = "1",
        UserName = "john_doe",
        NormalizedUserName = "JOHN_DOE",
        Email = "john.doe@example.com",
        NormalizedEmail = "JOHN.DOE@EXAMPLE.COM",
        FirstName = "John",
        LastName = "Doe",
        EmailConfirmed = true,
        PasswordHash = passwordHasher.HashPassword(null, "Password123!")
    },
    new User
    {
        Id = "2",
        UserName = "jane_smith",
        NormalizedUserName = "JANE_SMITH",
        Email = "jane.smith@example.com",
        NormalizedEmail = "JANE.SMITH@EXAMPLE.COM",
        FirstName = "Jane",
        LastName = "Smith",
        EmailConfirmed = true,
        PasswordHash = passwordHasher.HashPassword(null, "Password123!")
    },
    new User
    {
        Id = "3",
        UserName = "alice_brown",
        NormalizedUserName = "ALICE_BROWN",
        Email = "alice.brown@example.com",
        NormalizedEmail = "ALICE.BROWN@EXAMPLE.COM",
        FirstName = "Alice",
        LastName = "Brown",
        EmailConfirmed = true,
        PasswordHash = passwordHasher.HashPassword(null, "Password123!")
    },
    new User
    {
        Id = "4",
        UserName = "bob_jones",
        NormalizedUserName = "BOB_JONES",
        Email = "bob.jones@example.com",
        NormalizedEmail = "BOB.JONES@EXAMPLE.COM",
        FirstName = "Bob",
        LastName = "Jones",
        EmailConfirmed = true,
        PasswordHash = passwordHasher.HashPassword(null, "Password123!")
    },
    new User
    {
        Id = "5",
        UserName = "carla_white",
        NormalizedUserName = "CARLA_WHITE",
        Email = "carla.white@example.com",
        NormalizedEmail = "CARLA.WHITE@EXAMPLE.COM",
        FirstName = "Carla",
        LastName = "White",
        EmailConfirmed = true,
        PasswordHash = passwordHasher.HashPassword(null, "Password123!")
    },
    new User
    {
        Id = "6",
        UserName = "michael_green",
        NormalizedUserName = "MICHAEL_GREEN",
        Email = "michael.green@example.com",
        NormalizedEmail = "MICHAEL.GREEN@EXAMPLE.COM",
        FirstName = "Michael",
        LastName = "Green",
        EmailConfirmed = true,
        PasswordHash = passwordHasher.HashPassword(null, "Password123!")
    },
    new User
    {
        Id = "7",
        UserName = "linda_lee",
        NormalizedUserName = "LINDA_LEE",
        Email = "linda.lee@example.com",
        NormalizedEmail = "LINDA.LEE@EXAMPLE.COM",
        FirstName = "Linda",
        LastName = "Lee",
        EmailConfirmed = true,
        PasswordHash = passwordHasher.HashPassword(null, "Password123!")
    },
    new User
    {
        Id = "8",
        UserName = "james_brown",
        NormalizedUserName = "JAMES_BROWN",
        Email = "james.brown@example.com",
        NormalizedEmail = "JAMES.BROWN@EXAMPLE.COM",
        FirstName = "James",
        LastName = "Brown",
        EmailConfirmed = true,
        PasswordHash = passwordHasher.HashPassword(null, "Password123!")
    },
    new User
    {
        Id = "9",
        UserName = "emma_clark",
        NormalizedUserName = "EMMA_CLARK",
        Email = "emma.clark@example.com",
        NormalizedEmail = "EMMA.CLARK@EXAMPLE.COM",
        FirstName = "Emma",
        LastName = "Clark",
        EmailConfirmed = true,
        PasswordHash = passwordHasher.HashPassword(null, "Password123!")
    },
    new User
    {
        Id = "10",
        UserName = "daniel_evans",
        NormalizedUserName = "DANIEL_EVANS",
        Email = "daniel.evans@example.com",
        NormalizedEmail = "DANIEL.EVANS@EXAMPLE.COM",
        FirstName = "Daniel",
        LastName = "Evans",
        EmailConfirmed = true,
        PasswordHash = passwordHasher.HashPassword(null, "Password123!")
    }
);


    modelBuilder.Entity<Annotation>().HasData(
         new Annotation
    {
        AnnotationId = 1,
        UserId = "1",
        RepositoryId = 1,
        Type = "Note",
        Content = "Excellent IDE for web development",
        CreatedAt = DateTime.Parse("2024-01-21").ToUniversalTime()
    },
    new Annotation
    {
        AnnotationId = 2,
        UserId = "2",
        RepositoryId = 2,
        Type = "Tag",
        Content = "Mobile Development",
        CreatedAt = DateTime.Parse("2024-01-21").ToUniversalTime()
    },
    new Annotation
    {
        AnnotationId = 3,
        UserId = "3",
        RepositoryId = 3,
        Type = "Note",
        Content = "Systems programming language with memory safety",
        CreatedAt = DateTime.Parse("2024-01-21").ToUniversalTime()
    },
    new Annotation
    {
        AnnotationId = 4,
        UserId = "4",
        RepositoryId = 4,
        Type = "Tag",
        Content = "Deep Learning",
        CreatedAt = DateTime.Parse("2024-01-21").ToUniversalTime()
    },
    new Annotation
    {
        AnnotationId = 5,
        UserId = "5",
        RepositoryId = 5,
        Type = "Note",
        Content = "JavaScript runtime environment",
        CreatedAt = DateTime.Parse("2024-01-21").ToUniversalTime()
    },
    new Annotation
    {
        AnnotationId = 6,
        UserId = "7",
        RepositoryId = 9,
        Type = "Tag",
        Content = "Automated Testing",
        CreatedAt = DateTime.Parse("2024-01-21").ToUniversalTime()
    },
    new Annotation
    {
        AnnotationId = 7,
        UserId = "8",
        RepositoryId = 10,
        Type = "Note",
        Content = "Great sandbox for testing ML algorithms.",
        CreatedAt = DateTime.Parse("2024-01-21").ToUniversalTime()
    },
    new Annotation
    {
        AnnotationId = 8,
        UserId = "9",
        RepositoryId = 8,
        Type = "Tag",
        Content = "Full Stack",
        CreatedAt = DateTime.Parse("2024-01-21").ToUniversalTime()
    },
    new Annotation
    {
        AnnotationId = 9,
        UserId = "10",
        RepositoryId = 9,
        Type = "Note",
        Content = "Awesome data visualization library for presentations.",
        CreatedAt = DateTime.Parse("2024-01-21").ToUniversalTime()
    },
    new Annotation
    {
        AnnotationId = 10,
        UserId = "1",
        RepositoryId = 9,
        Type = "Note",
        Content = "Could use this for testing in our current project.",
        CreatedAt = DateTime.Parse("2024-01-21").ToUniversalTime()
    },
    new Annotation
    {
        AnnotationId = 11,
        UserId = "2",
        RepositoryId = 10,
        Type = "Note",
        Content = "Sandbox looks promising for prototyping new ML models.",
        CreatedAt = DateTime.Parse("2024-01-21").ToUniversalTime()
    },
    new Annotation
    {
        AnnotationId = 12,
        UserId = "3",
        RepositoryId = 9,
        Type = "Tag",
        Content = "Starter Kit",
        CreatedAt = DateTime.Parse("2024-01-21").ToUniversalTime()
    }
);

modelBuilder.Entity<Repository>().HasData(
    new Repository 
    { 
        Id = 1,
        Name = "vscode",
        FullName = "microsoft/vscode",
        HtmlUrl = "https://github.com/microsoft/vscode",
        Description = "Visual Studio Code",
        Language = "TypeScript",
        Stars = 154000,
        Forks = 28500,
        OpenIssuesCount = 250,
        HasIssues = true,
        HasProjects = true,
        Topics = new List<string> { "typescript", "editor", "ide" },
        Visibility = "public",
        CreatedAt = DateTime.Parse("2015-09-03").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2024-01-21").ToUniversalTime(),
        OwnerLogin = "microsoft",
        OwnerHtmlUrl = "https://github.com/microsoft"
    },
    new Repository 
    { 
        Id = 2,
        Name = "flutter",
        FullName = "flutter/flutter",
        HtmlUrl = "https://github.com/flutter/flutter",
        Description = "Flutter makes it easy and fast to build beautiful apps for mobile and beyond",
        Language = "Dart",
        Stars = 159000,
        Forks = 25800,
        OpenIssuesCount = 320,
        HasIssues = true,
        HasProjects = true,
        Topics = new List<string> { "dart", "mobile", "cross-platform" },
        Visibility = "public",
        CreatedAt = DateTime.Parse("2015-03-06").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2024-01-21").ToUniversalTime(),
        OwnerLogin = "flutter",
        OwnerHtmlUrl = "https://github.com/flutter"
    },
    new Repository 
    { 
        Id = 3,
        Name = "rust",
        FullName = "rust-lang/rust",
        HtmlUrl = "https://github.com/rust-lang/rust",
        Description = "Empowering everyone to build reliable and efficient software",
        Language = "Rust",
        Stars = 88000,
        Forks = 11200,
        OpenIssuesCount = 420,
        HasIssues = true,
        HasProjects = true,
        Topics = new List<string> { "rust", "systems-programming", "compiler" },
        Visibility = "public",
        CreatedAt = DateTime.Parse("2010-06-16").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2024-01-21").ToUniversalTime(),
        OwnerLogin = "rust-lang",
        OwnerHtmlUrl = "https://github.com/rust-lang"
    },
    new Repository 
    { 
        Id = 4,
        Name = "pytorch",
        FullName = "pytorch/pytorch",
        HtmlUrl = "https://github.com/pytorch/pytorch",
        Description = "Tensors and Dynamic neural networks in Python",
        Language = "C++",
        Stars = 73000,
        Forks = 19800,
        OpenIssuesCount = 280,
        HasIssues = true,
        HasProjects = true,
        Topics = new List<string> { "python", "machine-learning", "deep-learning" },
        Visibility = "public",
        CreatedAt = DateTime.Parse("2016-08-13").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2024-01-21").ToUniversalTime(),
        OwnerLogin = "pytorch",
        OwnerHtmlUrl = "https://github.com/pytorch"
    },
    new Repository 
    { 
        Id = 5,
        Name = "node",
        FullName = "nodejs/node",
        HtmlUrl = "https://github.com/nodejs/node",
        Description = "Node.js JavaScript runtime",
        Language = "JavaScript",
        Stars = 98000,
        Forks = 26500,
        OpenIssuesCount = 180,
        HasIssues = true,
        HasProjects = true,
        Topics = new List<string> { "javascript", "runtime", "server" },
        Visibility = "public",
        CreatedAt = DateTime.Parse("2014-11-26").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2024-01-21").ToUniversalTime(),
        OwnerLogin = "nodejs",
        OwnerHtmlUrl = "https://github.com/nodejs"
    },

    new Repository 
    { 
        Id = 6,
        Name = "react",
        FullName = "facebook/react",
        HtmlUrl = "https://github.com/facebook/react",
        Description = "A declarative, efficient, and flexible JavaScript library for building user interfaces.",
        Language = "JavaScript",
        Stars = 210000,
        Forks = 44000,
        OpenIssuesCount = 1200,
        HasIssues = true,
        HasProjects = true,
        Topics = new List<string> { "javascript", "frontend", "react" },
        Visibility = "public",
        CreatedAt = DateTime.Parse("2013-05-24").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2024-01-21").ToUniversalTime(),
        OwnerLogin = "facebook",
        OwnerHtmlUrl = "https://github.com/facebook"
    },
    new Repository 
    { 
        Id = 7,
        Name = "tensorflow",
        FullName = "tensorflow/tensorflow",
        HtmlUrl = "https://github.com/tensorflow/tensorflow",
        Description = "An Open Source Machine Learning Framework for Everyone",
        Language = "C++",
        Stars = 180000,
        Forks = 87000,
        OpenIssuesCount = 1900,
        HasIssues = true,
        HasProjects = true,
        Topics = new List<string> { "machine-learning", "ai", "deep-learning" },
        Visibility = "public",
        CreatedAt = DateTime.Parse("2015-11-09").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2024-01-21").ToUniversalTime(),
        OwnerLogin = "tensorflow",
        OwnerHtmlUrl = "https://github.com/tensorflow"
    },
    new Repository 
    { 
        Id = 8,
        Name = "kubernetes",
        FullName = "kubernetes/kubernetes",
        HtmlUrl = "https://github.com/kubernetes/kubernetes",
        Description = "Production-Grade Container Scheduling and Management",
        Language = "Go",
        Stars = 97000,
        Forks = 36000,
        OpenIssuesCount = 1800,
        HasIssues = true,
        HasProjects = true,
        Topics = new List<string> { "container", "orchestration", "kubernetes" },
        Visibility = "public",
        CreatedAt = DateTime.Parse("2014-06-06").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2024-01-21").ToUniversalTime(),
        OwnerLogin = "kubernetes",
        OwnerHtmlUrl = "https://github.com/kubernetes"
    },
    new Repository 
    { 
        Id = 9,
        Name = "linux",
        FullName = "torvalds/linux",
        HtmlUrl = "https://github.com/torvalds/linux",
        Description = "Linux kernel source tree",
        Language = "C",
        Stars = 150000,
        Forks = 75000,
        OpenIssuesCount = 500,
        HasIssues = false,
        HasProjects = true,
        Topics = new List<string> { "operating-system", "kernel", "linux" },
        Visibility = "public",
        CreatedAt = DateTime.Parse("2001-04-16").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2024-01-21").ToUniversalTime(),
        OwnerLogin = "torvalds",
        OwnerHtmlUrl = "https://github.com/torvalds"
    },
    new Repository 
    { 
        Id = 10,
        Name = "django",
        FullName = "django/django",
        HtmlUrl = "https://github.com/django/django",
        Description = "The Web framework for perfectionists with deadlines",
        Language = "Python",
        Stars = 74000,
        Forks = 31000,
        OpenIssuesCount = 1200,
        HasIssues = true,
        HasProjects = true,
        Topics = new List<string> { "python", "web-framework", "django" },
        Visibility = "public",
        CreatedAt = DateTime.Parse("2005-07-21").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2024-01-21").ToUniversalTime(),
        OwnerLogin = "django",
        OwnerHtmlUrl = "https://github.com/django"
    },
    new Repository 
{ 
    Id = 11,
    Name = "express",
    FullName = "expressjs/express",
    HtmlUrl = "https://github.com/expressjs/express",
    Description = "Fast, unopinionated, minimalist web framework for Node.js",
    Language = "JavaScript",
    Stars = 60000,
    Forks = 10000,
    OpenIssuesCount = 150,
    HasIssues = true,
    HasProjects = true,
    Topics = new List<string> { "nodejs", "web-framework", "express" },
    Visibility = "public",
    CreatedAt = DateTime.Parse("2010-06-26").ToUniversalTime(),
    UpdatedAt = DateTime.Parse("2024-01-21").ToUniversalTime(),
    OwnerLogin = "expressjs",
    OwnerHtmlUrl = "https://github.com/expressjs"
}

);

modelBuilder.Entity<Issue>().HasData(
    new Issue
    {
        Id = 1,
        Title = "Update React documentation for new hooks",
        Body = "We need to update the documentation to reflect the latest changes in React hooks API",
        State = "open",
        Comments = 5,
        HtmlUrl = "https://github.com/facebook/react/issues/1",
        CreatedAt = DateTime.Parse("2024-01-15").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2024-01-21").ToUniversalTime(),
        RepositoryUrl = "https://github.com/facebook/react",
        Labels = new List<string> { "documentation", "good first issue" },
        RepositoryId = 6
    },
    new Issue
    {
        Id = 2,
        Title = "Add example for custom hooks",
        Body = "Need to add more examples showing how to create and use custom hooks effectively",
        State = "open",
        Comments = 3,
        HtmlUrl = "https://github.com/facebook/react/issues/2",
        CreatedAt = DateTime.Parse("2024-01-16").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2024-01-21").ToUniversalTime(),
        RepositoryUrl = "https://github.com/facebook/react",
        Labels = new List<string> { "documentation", "help wanted" },
        RepositoryId = 6
    },
    new Issue
    {
        Id = 3,
        Title = "Fix memory leak in VSCode",
        Body = "Memory leaks are observed when opening large JSON files",
        State = "open",
        Comments = 12,
        HtmlUrl = "https://github.com/microsoft/vscode/issues/3",
        CreatedAt = DateTime.Parse("2024-01-10").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2024-01-19").ToUniversalTime(),
        RepositoryUrl = "https://github.com/microsoft/vscode",
        Labels = new List<string> { "bug", "performance" },
        RepositoryId = 1
    },
    new Issue
    {
        Id = 4,
        Title = "Improve Flutter startup time",
        Body = "Investigate and optimize the startup time of Flutter apps",
        State = "open",
        Comments = 7,
        HtmlUrl = "https://github.com/flutter/flutter/issues/4",
        CreatedAt = DateTime.Parse("2024-01-14").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2024-01-20").ToUniversalTime(),
        RepositoryUrl = "https://github.com/flutter/flutter",
        Labels = new List<string> { "performance", "enhancement" },
        RepositoryId = 2
    },
    new Issue
    {
        Id = 5,
        Title = "Add Rust async programming guide",
        Body = "A comprehensive guide for writing async code in Rust should be created",
        State = "open",
        Comments = 8,
        HtmlUrl = "https://github.com/rust-lang/rust/issues/5",
        CreatedAt = DateTime.Parse("2024-01-17").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2024-01-21").ToUniversalTime(),
        RepositoryUrl = "https://github.com/rust-lang/rust",
        Labels = new List<string> { "documentation", "async" },
        RepositoryId = 3
    },
    new Issue
    {
        Id = 6,
        Title = "TensorFlow GPU memory issue",
        Body = "Running models on large datasets leads to memory overflow errors",
        State = "open",
        Comments = 15,
        HtmlUrl = "https://github.com/tensorflow/tensorflow/issues/6",
        CreatedAt = DateTime.Parse("2024-01-12").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2024-01-20").ToUniversalTime(),
        RepositoryUrl = "https://github.com/tensorflow/tensorflow",
        Labels = new List<string> { "bug", "gpu" },
        RepositoryId = 7
    },
    new Issue
    {
        Id = 7,
        Title = "Kubernetes networking problem",
        Body = "Pods are unable to communicate in custom networking configurations",
        State = "open",
        Comments = 10,
        HtmlUrl = "https://github.com/kubernetes/kubernetes/issues/7",
        CreatedAt = DateTime.Parse("2024-01-08").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2024-01-19").ToUniversalTime(),
        RepositoryUrl = "https://github.com/kubernetes/kubernetes",
        Labels = new List<string> { "networking", "help wanted" },
        RepositoryId = 8
    },
    new Issue
    {
        Id = 8,
        Title = "Fix Linux kernel panic on boot",
        Body = "Certain hardware configurations are causing kernel panic",
        State = "open",
        Comments = 22,
        HtmlUrl = "https://github.com/torvalds/linux/issues/8",
        CreatedAt = DateTime.Parse("2024-01-09").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2024-01-18").ToUniversalTime(),
        RepositoryUrl = "https://github.com/torvalds/linux",
        Labels = new List<string> { "kernel", "bug" },
        RepositoryId = 9
    },
    new Issue
    {
        Id = 9,
        Title = "Django ORM performance issues",
        Body = "Performance degradation observed when running complex queries",
        State = "open",
        Comments = 14,
        HtmlUrl = "https://github.com/django/django/issues/9",
        CreatedAt = DateTime.Parse("2024-01-11").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2024-01-19").ToUniversalTime(),
        RepositoryUrl = "https://github.com/django/django",
        Labels = new List<string> { "performance", "query" },
        RepositoryId = 10
    },
    new Issue
    {
        Id = 10,
        Title = "Improve Node.js documentation on async/await",
        Body = "Add examples and explanations for better understanding of async/await usage in Node.js",
        State = "open",
        Comments = 6,
        HtmlUrl = "https://github.com/nodejs/node/issues/10",
        CreatedAt = DateTime.Parse("2024-01-13").ToUniversalTime(),
        UpdatedAt = DateTime.Parse("2024-01-20").ToUniversalTime(),
        RepositoryUrl = "https://github.com/nodejs/node",
        Labels = new List<string> { "documentation", "async" },
        RepositoryId = 5
    },
    new Issue
{
    Id = 11,
    Title = "Optimize Express middleware performance",
    Body = "Improve the performance of built-in middleware functions to reduce response time.",
    State = "open",
    Comments = 9,
    HtmlUrl = "https://github.com/expressjs/express/issues/11",
    CreatedAt = DateTime.Parse("2024-01-17").ToUniversalTime(),
    UpdatedAt = DateTime.Parse("2024-01-21").ToUniversalTime(),
    RepositoryUrl = "https://github.com/expressjs/express",
    Labels = new List<string> { "performance", "enhancement" },
    RepositoryId = 11
}

);


modelBuilder.Entity<UserRepository>().HasData(
    new UserRepository { UserId = "1", RepositoryId = 1, SavedAt = DateTime.Parse("2024-01-21").ToUniversalTime() }, // vscode
    new UserRepository { UserId = "1", RepositoryId = 11, SavedAt = DateTime.Parse("2024-01-21").ToUniversalTime() },
    new UserRepository { UserId = "2", RepositoryId = 3, SavedAt = DateTime.Parse("2024-01-21").ToUniversalTime()}, // rust
    new UserRepository { UserId = "2", RepositoryId = 11, SavedAt = DateTime.Parse("2024-01-21").ToUniversalTime()} ,
    new UserRepository { UserId = "3", RepositoryId = 5, SavedAt = DateTime.Parse("2024-01-21").ToUniversalTime() }, // node
    new UserRepository { UserId = "4", RepositoryId = 6, SavedAt = DateTime.Parse("2024-01-21").ToUniversalTime() }, // react
    new UserRepository { UserId = "5", RepositoryId = 7, SavedAt = DateTime.Parse("2024-01-21").ToUniversalTime() }, // tensorflow
    new UserRepository { UserId = "6", RepositoryId = 8, SavedAt = DateTime.Parse("2024-01-21").ToUniversalTime() }, // kubernetes
    new UserRepository { UserId = "7", RepositoryId = 9, SavedAt = DateTime.Parse("2024-01-21").ToUniversalTime() }, // linux
    new UserRepository { UserId = "8", RepositoryId = 10, SavedAt = DateTime.Parse("2024-01-21").ToUniversalTime() } // django
);


modelBuilder.Entity<UserIssue>().HasData(
    new UserIssue { UserId = "1", IssueId = 3, AssignedAt = DateTime.Parse("2024-01-21").ToUniversalTime(), SavedAt = DateTime.Parse("2024-01-21").ToUniversalTime() },  // Fix memory leak in VSCode
    new UserIssue { UserId = "1", IssueId = 11, AssignedAt = DateTime.Parse("2024-01-21").ToUniversalTime(), SavedAt = DateTime.Parse("2024-01-21").ToUniversalTime() },
    new UserIssue { UserId = "2", IssueId = 5, AssignedAt = DateTime.Parse("2024-01-21").ToUniversalTime(), SavedAt = DateTime.Parse("2024-01-21").ToUniversalTime()},  // Rust async programming guide
      new UserIssue { UserId = "2", IssueId = 11, AssignedAt = DateTime.Parse("2024-01-21").ToUniversalTime(), SavedAt = DateTime.Parse("2024-01-21").ToUniversalTime()},   // Express middleware performance
    new UserIssue { UserId = "3", IssueId = 10, AssignedAt = DateTime.Parse("2024-01-21").ToUniversalTime(), SavedAt = DateTime.Parse("2024-01-21").ToUniversalTime() }, // Node.js async/await docs
    new UserIssue { UserId = "4", IssueId = 1, AssignedAt = DateTime.Parse("2024-01-21").ToUniversalTime(), SavedAt = DateTime.Parse("2024-01-21").ToUniversalTime() },  // React hooks update
    new UserIssue { UserId = "5", IssueId = 6, AssignedAt = DateTime.Parse("2024-01-21").ToUniversalTime(), SavedAt = DateTime.Parse("2024-01-21").ToUniversalTime() },  // TensorFlow GPU issue
    new UserIssue { UserId = "6", IssueId = 7, AssignedAt = DateTime.Parse("2024-01-21").ToUniversalTime(), SavedAt = DateTime.Parse("2024-01-21").ToUniversalTime() },  // Kubernetes networking
    new UserIssue { UserId = "7", IssueId = 8, AssignedAt = DateTime.Parse("2024-01-21").ToUniversalTime(), SavedAt = DateTime.Parse("2024-01-21").ToUniversalTime() },  // Linux kernel panic
    new UserIssue { UserId = "8", IssueId = 9, AssignedAt = DateTime.Parse("2024-01-21").ToUniversalTime(), SavedAt = DateTime.Parse("2024-01-21").ToUniversalTime() }   // Django ORM performance
);







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