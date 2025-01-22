using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TestGitHubPart2.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    GitHubToken = table.Column<string>(type: "text", nullable: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Repositories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    HtmlUrl = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Language = table.Column<string>(type: "text", nullable: true),
                    Stars = table.Column<int>(type: "integer", nullable: false),
                    Forks = table.Column<int>(type: "integer", nullable: false),
                    OpenIssuesCount = table.Column<int>(type: "integer", nullable: true),
                    HasIssues = table.Column<bool>(type: "boolean", nullable: false),
                    HasProjects = table.Column<bool>(type: "boolean", nullable: false),
                    Topics = table.Column<string>(type: "jsonb", nullable: true),
                    Visibility = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PushedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CloneUrl = table.Column<string>(type: "text", nullable: true),
                    WatchersCount = table.Column<int>(type: "integer", nullable: true),
                    License = table.Column<string>(type: "text", nullable: true),
                    ContributorsUrl = table.Column<string>(type: "text", nullable: true),
                    SubscribersUrl = table.Column<string>(type: "text", nullable: true),
                    CommitsUrl = table.Column<string>(type: "text", nullable: true),
                    GitCommitsUrl = table.Column<string>(type: "text", nullable: true),
                    IssuesUrl = table.Column<string>(type: "text", nullable: true),
                    PullsUrl = table.Column<string>(type: "text", nullable: true),
                    ReleasesUrl = table.Column<string>(type: "text", nullable: true),
                    TagsUrl = table.Column<string>(type: "text", nullable: true),
                    OwnerLogin = table.Column<string>(type: "text", nullable: true),
                    OwnerHtmlUrl = table.Column<string>(type: "text", nullable: true),
                    CategoryId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repositories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Repositories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId");
                });

            migrationBuilder.CreateTable(
                name: "Annotations",
                columns: table => new
                {
                    AnnotationId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RepositoryId = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Annotations", x => x.AnnotationId);
                    table.ForeignKey(
                        name: "FK_Annotations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Annotations_Repositories_RepositoryId",
                        column: x => x.RepositoryId,
                        principalTable: "Repositories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Issues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Body = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<string>(type: "text", nullable: false),
                    Comments = table.Column<int>(type: "integer", nullable: false),
                    HtmlUrl = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RepositoryUrl = table.Column<string>(type: "text", nullable: true),
                    Labels = table.Column<string>(type: "jsonb", nullable: true),
                    RepositoryId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Issues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Issues_Repositories_RepositoryId",
                        column: x => x.RepositoryId,
                        principalTable: "Repositories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRepositories",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RepositoryId = table.Column<int>(type: "integer", nullable: false),
                    SavedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRepositories", x => new { x.UserId, x.RepositoryId });
                    table.ForeignKey(
                        name: "FK_UserRepositories_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRepositories_Repositories_RepositoryId",
                        column: x => x.RepositoryId,
                        principalTable: "Repositories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserIssue",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    IssueId = table.Column<int>(type: "integer", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SavedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserIssue", x => new { x.UserId, x.IssueId });
                    table.ForeignKey(
                        name: "FK_UserIssue_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserIssue_Issues_IssueId",
                        column: x => x.IssueId,
                        principalTable: "Issues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "GitHubToken", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "1", 0, "d0a44604-4cb1-4ad7-8818-e188a32b28e0", "john.doe@example.com", true, "John", null, "Doe", false, null, "JOHN.DOE@EXAMPLE.COM", "JOHN_DOE", "AQAAAAIAAYagAAAAECDUX5xUcPe4qvir3Shm2L7orJecXxIb/Isv6b/Zwhqx9xgpqDuU9FrBVztjkfg4OQ==", null, false, "6d3edab2-0c30-4168-9cc9-a178bbe4a345", false, "john_doe" },
                    { "10", 0, "a1df6465-e1bc-4123-9bbd-9740fb8100b4", "daniel.evans@example.com", true, "Daniel", null, "Evans", false, null, "DANIEL.EVANS@EXAMPLE.COM", "DANIEL_EVANS", "AQAAAAIAAYagAAAAEMC92Q6TM3dI3PtM1Qu282s2KPl3r7hjwR53BB0RsXtwxyPttpgkGKWqAHaqQRrLvA==", null, false, "34451cac-a8c1-4f21-b5ee-242486a7c040", false, "daniel_evans" },
                    { "2", 0, "92aa6b78-b1d9-4f44-b40d-50944ad17128", "jane.smith@example.com", true, "Jane", null, "Smith", false, null, "JANE.SMITH@EXAMPLE.COM", "JANE_SMITH", "AQAAAAIAAYagAAAAEPT9EDefveOQt/XPGIuq0KhF8NX4iZibNSJoys8VqtHSdRPGIovY5binhwyfyHEr5w==", null, false, "d46bdce0-e6c7-44a9-8ba1-6ce3a028dbf7", false, "jane_smith" },
                    { "3", 0, "608d7a6b-a914-4298-828b-38fcfa1a83fb", "alice.brown@example.com", true, "Alice", null, "Brown", false, null, "ALICE.BROWN@EXAMPLE.COM", "ALICE_BROWN", "AQAAAAIAAYagAAAAEPIh2oH6ZX/RpNbA2+YIZwvR9uFTUWsZrei3dIsMiAsBptT/9b+XK3vFxFnW7VqbuQ==", null, false, "10221039-2c00-458c-a887-cb05e5ca3117", false, "alice_brown" },
                    { "4", 0, "e85a5523-80a4-4f15-9bd8-58a4670a1fb4", "bob.jones@example.com", true, "Bob", null, "Jones", false, null, "BOB.JONES@EXAMPLE.COM", "BOB_JONES", "AQAAAAIAAYagAAAAEDnDz90ayDFylGe5Hc3FFvhp6uCFeT8YH1QKCRdTK+ScBhYFcintpSenZcnuEyg3pQ==", null, false, "60ceeb24-7884-4c72-8fd0-cef3c4a97fe2", false, "bob_jones" },
                    { "5", 0, "a220dec4-50fb-4c2d-97bf-465b162be606", "carla.white@example.com", true, "Carla", null, "White", false, null, "CARLA.WHITE@EXAMPLE.COM", "CARLA_WHITE", "AQAAAAIAAYagAAAAEEbysmL7xFZuu1X3aiwiPYJAITeYY0LpHdQEjX2GwIjrpYAFxnl0Z1g8rmhgF/Wrag==", null, false, "260b308b-63a6-4c09-ada4-e8544653723b", false, "carla_white" },
                    { "6", 0, "f43136cc-a199-4990-96ea-8f60bd9a2847", "michael.green@example.com", true, "Michael", null, "Green", false, null, "MICHAEL.GREEN@EXAMPLE.COM", "MICHAEL_GREEN", "AQAAAAIAAYagAAAAED0VFpSZIMRyoPl8wVYqf/CmXWOJxU09L5wbyaENI1F5TvJC7vJnKXQCT+tO76qwmA==", null, false, "00161403-659a-415b-b3d4-d18625e7a41d", false, "michael_green" },
                    { "7", 0, "5f78bb30-d51a-48fd-8b4e-663d382485f3", "linda.lee@example.com", true, "Linda", null, "Lee", false, null, "LINDA.LEE@EXAMPLE.COM", "LINDA_LEE", "AQAAAAIAAYagAAAAEDWHP/ZW+ELpRQp7TdsBc2wIzj4NGU4W/0HNzMFcjxSuhquu4yUhTARN/Ne5ByiFAQ==", null, false, "bbea2324-b89d-4694-a244-868c1e1596df", false, "linda_lee" },
                    { "8", 0, "03ed317c-65e1-47ee-b722-0861a65ba1fb", "james.brown@example.com", true, "James", null, "Brown", false, null, "JAMES.BROWN@EXAMPLE.COM", "JAMES_BROWN", "AQAAAAIAAYagAAAAEOES0PeC5E3YWhXCPsVQH6Ys+tmDer9/aker1Zwa6VtLW6QkLQF+Z85tBkx8iL7LXg==", null, false, "8e49e38d-2231-4d78-b4db-5a6e4875cbb5", false, "james_brown" },
                    { "9", 0, "09a4aa64-a929-4a4c-ad34-e2fa626a87e4", "emma.clark@example.com", true, "Emma", null, "Clark", false, null, "EMMA.CLARK@EXAMPLE.COM", "EMMA_CLARK", "AQAAAAIAAYagAAAAEC79N0TsOjnL4k50U//RxDWZzi2b63oxMhrfP1n0YakD8klNv7JZHRgNmc7lPmoozQ==", null, false, "57e8cc19-3467-4e50-988e-a569f0e0d833", false, "emma_clark" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Description" },
                values: new object[,]
                {
                    { 1, "Backend" },
                    { 2, "Frontend" },
                    { 3, "Artificial Intelligence" },
                    { 4, "Full Stack" },
                    { 5, "Data Science" },
                    { 6, "Security" },
                    { 7, "Testing" },
                    { 8, "Machine Learning" },
                    { 9, "Other" }
                });

            migrationBuilder.InsertData(
                table: "Repositories",
                columns: new[] { "Id", "CategoryId", "CloneUrl", "CommitsUrl", "ContributorsUrl", "CreatedAt", "Description", "Forks", "FullName", "GitCommitsUrl", "HasIssues", "HasProjects", "HtmlUrl", "IssuesUrl", "Language", "License", "Name", "OpenIssuesCount", "OwnerHtmlUrl", "OwnerLogin", "PullsUrl", "PushedAt", "ReleasesUrl", "Stars", "SubscribersUrl", "TagsUrl", "Topics", "UpdatedAt", "Visibility", "WatchersCount" },
                values: new object[,]
                {
                    { 1, null, null, null, null, new DateTime(2015, 9, 3, 5, 0, 0, 0, DateTimeKind.Utc), "Visual Studio Code", 28500, "microsoft/vscode", null, true, true, "https://github.com/microsoft/vscode", null, "TypeScript", null, "vscode", 250, "https://github.com/microsoft", "microsoft", null, null, null, 154000, null, null, "[\"typescript\",\"editor\",\"ide\"]", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc), "public", null },
                    { 2, null, null, null, null, new DateTime(2015, 3, 6, 6, 0, 0, 0, DateTimeKind.Utc), "Flutter makes it easy and fast to build beautiful apps for mobile and beyond", 25800, "flutter/flutter", null, true, true, "https://github.com/flutter/flutter", null, "Dart", null, "flutter", 320, "https://github.com/flutter", "flutter", null, null, null, 159000, null, null, "[\"dart\",\"mobile\",\"cross-platform\"]", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc), "public", null },
                    { 3, null, null, null, null, new DateTime(2010, 6, 16, 5, 0, 0, 0, DateTimeKind.Utc), "Empowering everyone to build reliable and efficient software", 11200, "rust-lang/rust", null, true, true, "https://github.com/rust-lang/rust", null, "Rust", null, "rust", 420, "https://github.com/rust-lang", "rust-lang", null, null, null, 88000, null, null, "[\"rust\",\"systems-programming\",\"compiler\"]", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc), "public", null },
                    { 4, null, null, null, null, new DateTime(2016, 8, 13, 5, 0, 0, 0, DateTimeKind.Utc), "Tensors and Dynamic neural networks in Python", 19800, "pytorch/pytorch", null, true, true, "https://github.com/pytorch/pytorch", null, "C++", null, "pytorch", 280, "https://github.com/pytorch", "pytorch", null, null, null, 73000, null, null, "[\"python\",\"machine-learning\",\"deep-learning\"]", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc), "public", null },
                    { 5, null, null, null, null, new DateTime(2014, 11, 26, 6, 0, 0, 0, DateTimeKind.Utc), "Node.js JavaScript runtime", 26500, "nodejs/node", null, true, true, "https://github.com/nodejs/node", null, "JavaScript", null, "node", 180, "https://github.com/nodejs", "nodejs", null, null, null, 98000, null, null, "[\"javascript\",\"runtime\",\"server\"]", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc), "public", null },
                    { 6, null, null, null, null, new DateTime(2013, 5, 24, 5, 0, 0, 0, DateTimeKind.Utc), "A declarative, efficient, and flexible JavaScript library for building user interfaces.", 44000, "facebook/react", null, true, true, "https://github.com/facebook/react", null, "JavaScript", null, "react", 1200, "https://github.com/facebook", "facebook", null, null, null, 210000, null, null, "[\"javascript\",\"frontend\",\"react\"]", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc), "public", null },
                    { 7, null, null, null, null, new DateTime(2015, 11, 9, 6, 0, 0, 0, DateTimeKind.Utc), "An Open Source Machine Learning Framework for Everyone", 87000, "tensorflow/tensorflow", null, true, true, "https://github.com/tensorflow/tensorflow", null, "C++", null, "tensorflow", 1900, "https://github.com/tensorflow", "tensorflow", null, null, null, 180000, null, null, "[\"machine-learning\",\"ai\",\"deep-learning\"]", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc), "public", null },
                    { 8, null, null, null, null, new DateTime(2014, 6, 6, 5, 0, 0, 0, DateTimeKind.Utc), "Production-Grade Container Scheduling and Management", 36000, "kubernetes/kubernetes", null, true, true, "https://github.com/kubernetes/kubernetes", null, "Go", null, "kubernetes", 1800, "https://github.com/kubernetes", "kubernetes", null, null, null, 97000, null, null, "[\"container\",\"orchestration\",\"kubernetes\"]", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc), "public", null },
                    { 9, null, null, null, null, new DateTime(2001, 4, 16, 5, 0, 0, 0, DateTimeKind.Utc), "Linux kernel source tree", 75000, "torvalds/linux", null, false, true, "https://github.com/torvalds/linux", null, "C", null, "linux", 500, "https://github.com/torvalds", "torvalds", null, null, null, 150000, null, null, "[\"operating-system\",\"kernel\",\"linux\"]", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc), "public", null },
                    { 10, null, null, null, null, new DateTime(2005, 7, 21, 5, 0, 0, 0, DateTimeKind.Utc), "The Web framework for perfectionists with deadlines", 31000, "django/django", null, true, true, "https://github.com/django/django", null, "Python", null, "django", 1200, "https://github.com/django", "django", null, null, null, 74000, null, null, "[\"python\",\"web-framework\",\"django\"]", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc), "public", null },
                    { 11, null, null, null, null, new DateTime(2010, 6, 26, 5, 0, 0, 0, DateTimeKind.Utc), "Fast, unopinionated, minimalist web framework for Node.js", 10000, "expressjs/express", null, true, true, "https://github.com/expressjs/express", null, "JavaScript", null, "express", 150, "https://github.com/expressjs", "expressjs", null, null, null, 60000, null, null, "[\"nodejs\",\"web-framework\",\"express\"]", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc), "public", null }
                });

            migrationBuilder.InsertData(
                table: "Annotations",
                columns: new[] { "AnnotationId", "Content", "CreatedAt", "RepositoryId", "Type", "UserId" },
                values: new object[,]
                {
                    { 1, "Excellent IDE for web development", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc), 1, "Note", "1" },
                    { 2, "Mobile Development", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc), 2, "Tag", "2" },
                    { 3, "Systems programming language with memory safety", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc), 3, "Note", "3" },
                    { 4, "Deep Learning", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc), 4, "Tag", "4" },
                    { 5, "JavaScript runtime environment", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc), 5, "Note", "5" },
                    { 6, "Automated Testing", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc), 9, "Tag", "7" },
                    { 7, "Great sandbox for testing ML algorithms.", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc), 10, "Note", "8" },
                    { 8, "Full Stack", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc), 8, "Tag", "9" },
                    { 9, "Awesome data visualization library for presentations.", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc), 9, "Note", "10" },
                    { 10, "Could use this for testing in our current project.", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc), 9, "Note", "1" },
                    { 11, "Sandbox looks promising for prototyping new ML models.", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc), 10, "Note", "2" },
                    { 12, "Starter Kit", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc), 9, "Tag", "3" }
                });

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "Id", "Body", "Comments", "CreatedAt", "HtmlUrl", "Labels", "RepositoryId", "RepositoryUrl", "State", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "We need to update the documentation to reflect the latest changes in React hooks API", 5, new DateTime(2024, 1, 15, 6, 0, 0, 0, DateTimeKind.Utc), "https://github.com/facebook/react/issues/1", "[\"documentation\",\"good first issue\"]", 6, "https://github.com/facebook/react", "open", "Update React documentation for new hooks", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, "Need to add more examples showing how to create and use custom hooks effectively", 3, new DateTime(2024, 1, 16, 6, 0, 0, 0, DateTimeKind.Utc), "https://github.com/facebook/react/issues/2", "[\"documentation\",\"help wanted\"]", 6, "https://github.com/facebook/react", "open", "Add example for custom hooks", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, "Memory leaks are observed when opening large JSON files", 12, new DateTime(2024, 1, 10, 6, 0, 0, 0, DateTimeKind.Utc), "https://github.com/microsoft/vscode/issues/3", "[\"bug\",\"performance\"]", 1, "https://github.com/microsoft/vscode", "open", "Fix memory leak in VSCode", new DateTime(2024, 1, 19, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, "Investigate and optimize the startup time of Flutter apps", 7, new DateTime(2024, 1, 14, 6, 0, 0, 0, DateTimeKind.Utc), "https://github.com/flutter/flutter/issues/4", "[\"performance\",\"enhancement\"]", 2, "https://github.com/flutter/flutter", "open", "Improve Flutter startup time", new DateTime(2024, 1, 20, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, "A comprehensive guide for writing async code in Rust should be created", 8, new DateTime(2024, 1, 17, 6, 0, 0, 0, DateTimeKind.Utc), "https://github.com/rust-lang/rust/issues/5", "[\"documentation\",\"async\"]", 3, "https://github.com/rust-lang/rust", "open", "Add Rust async programming guide", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { 6, "Running models on large datasets leads to memory overflow errors", 15, new DateTime(2024, 1, 12, 6, 0, 0, 0, DateTimeKind.Utc), "https://github.com/tensorflow/tensorflow/issues/6", "[\"bug\",\"gpu\"]", 7, "https://github.com/tensorflow/tensorflow", "open", "TensorFlow GPU memory issue", new DateTime(2024, 1, 20, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, "Pods are unable to communicate in custom networking configurations", 10, new DateTime(2024, 1, 8, 6, 0, 0, 0, DateTimeKind.Utc), "https://github.com/kubernetes/kubernetes/issues/7", "[\"networking\",\"help wanted\"]", 8, "https://github.com/kubernetes/kubernetes", "open", "Kubernetes networking problem", new DateTime(2024, 1, 19, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { 8, "Certain hardware configurations are causing kernel panic", 22, new DateTime(2024, 1, 9, 6, 0, 0, 0, DateTimeKind.Utc), "https://github.com/torvalds/linux/issues/8", "[\"kernel\",\"bug\"]", 9, "https://github.com/torvalds/linux", "open", "Fix Linux kernel panic on boot", new DateTime(2024, 1, 18, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, "Performance degradation observed when running complex queries", 14, new DateTime(2024, 1, 11, 6, 0, 0, 0, DateTimeKind.Utc), "https://github.com/django/django/issues/9", "[\"performance\",\"query\"]", 10, "https://github.com/django/django", "open", "Django ORM performance issues", new DateTime(2024, 1, 19, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { 10, "Add examples and explanations for better understanding of async/await usage in Node.js", 6, new DateTime(2024, 1, 13, 6, 0, 0, 0, DateTimeKind.Utc), "https://github.com/nodejs/node/issues/10", "[\"documentation\",\"async\"]", 5, "https://github.com/nodejs/node", "open", "Improve Node.js documentation on async/await", new DateTime(2024, 1, 20, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { 11, "Improve the performance of built-in middleware functions to reduce response time.", 9, new DateTime(2024, 1, 17, 6, 0, 0, 0, DateTimeKind.Utc), "https://github.com/expressjs/express/issues/11", "[\"performance\",\"enhancement\"]", 11, "https://github.com/expressjs/express", "open", "Optimize Express middleware performance", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "UserRepositories",
                columns: new[] { "RepositoryId", "UserId", "SavedAt" },
                values: new object[,]
                {
                    { 1, "1", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { 11, "1", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, "2", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { 11, "2", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, "3", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { 6, "4", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, "5", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { 8, "6", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, "7", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { 10, "8", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "UserIssue",
                columns: new[] { "IssueId", "UserId", "AssignedAt", "SavedAt" },
                values: new object[,]
                {
                    { 3, "1", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { 11, "1", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, "2", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { 11, "2", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { 10, "3", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { 1, "4", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { 6, "5", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, "6", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { 8, "7", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, "8", new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 21, 6, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Annotations_RepositoryId",
                table: "Annotations",
                column: "RepositoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Annotations_UserId",
                table: "Annotations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Issues_RepositoryId",
                table: "Issues",
                column: "RepositoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Repositories_CategoryId",
                table: "Repositories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserIssue_IssueId",
                table: "UserIssue",
                column: "IssueId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRepositories_RepositoryId",
                table: "UserRepositories",
                column: "RepositoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Annotations");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "UserIssue");

            migrationBuilder.DropTable(
                name: "UserRepositories");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Issues");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Repositories");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
