using System.Text.Json.Serialization;

namespace TestDemo;

public class GitHubIssue
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("html_url")]
    public string HtmlUrl { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }

    [JsonPropertyName("labels")]
    public List<GitHubLabel> Labels { get; set; }
}
