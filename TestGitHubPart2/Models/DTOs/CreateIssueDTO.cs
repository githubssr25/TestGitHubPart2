
namespace PostMVPProject.Models.DTOs;

public class CreateIssueDTO
        {
            public string RepositoryOwner { get; set; }
            public string RepositoryName { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }
            public List<string>? Labels { get; set; }
        }