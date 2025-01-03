using System.Text.Json.Serialization;

namespace TestDemo;

public class GitHubLabel
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}
