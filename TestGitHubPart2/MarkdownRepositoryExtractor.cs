using  PostMVPProject.Models;

namespace PostMVPProject.Extractor;

public class MarkdownRepositoryExtractor{
  //This class takes the raw markdown text and turns it into a list of repositories.
    public static List<Repository> ExtractRepositoriesFromMarkdown(string markdown)
    {
        var repos = new List<Repository>();
        var lines = markdown.Split("\n");
        //         The markdown.Split("\n") breaks the whole file into individual lines of text. Each line contains:
        // A section like ## JavaScript
        // A link to a repo like - [freeCodeCamp](https://github.com/freeCodeCamp/freeCodeCamp) - A coding curriculum.

        string? currentLanguage = null;

        foreach (var line in lines)
        {
            // Detect when a new language section starts
            if (line.StartsWith("## "))
            //Whenever it finds a line that starts with ## , it knows it's starting a new language section like this: ie ## JavaScript
            // This will extract "JavaScript" and store it in the currentLanguage variable. This way, every repo that appears after this belongs to the "JavaScript" category.
            {
                currentLanguage = line.Replace("## ", "").Trim();
            }

            // Detect repository links (these typically look like - [repo name](url) - description)
            var match = System.Text.RegularExpressions.Regex.Match(line, @"\[\s*(.+?)\s*\]\s*\((https?://[^\)]+)\)(?:\s*-\s*(.*))?");
            //This part finds the following pattern from the markdown: ie [Repository Name](https://github.com/username/repo-name) - Description
            //             Using Regular Expressions, it breaks down the important pieces into 3 groups:
            // Name — Inside the [ ] (like freeCodeCamp)
            // URL — Inside the ( ) (like https://github.com/freeCodeCamp/freeCodeCamp)
            // Description — Everything after - (like "A coding curriculum")
            // If it finds this pattern, it creates a new Repository object like this:
            if (match.Success)
            {
                repos.Add(new Repository
                {
                    Name = match.Groups[1].Value,          // freeCodeCamp
                    HtmlUrl = match.Groups[2].Value,           // https://github.com/freeCodeCamp/freeCodeCamp
                    Description = match.Groups[3].Value,  // A coding curriculum
                    Language = currentLanguage,            // JavaScript (from earlier) // Default values for new properties that require API calls is what all above are 

                     // Setting default values for other properties to prevent null reference issues
                    Stars = 0,
                    Forks = 0,
                    OpenIssuesCount = 0,
                    HasIssues = false,
                    HasProjects = false,
                    Topics = new List<string>(),
                    Visibility = "public",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }
        }

        return repos;
    }
}
//want to parse the markdown text into useful repository objects 

// The raw markdown file looks like plain text, but it has a structure (links, language headings, descriptions, etc.). We need a way to extract the important parts like:
// Repository Name
// URL (where users can go to view the repo)
// Description (brief description of the repo)
// Language (which section it was under, like "JavaScript", "Python", etc.)

// Split the markdown into individual lines (because each repo is on its own line)
// Look for language headers (like ## JavaScript or ## Python).
// Look for links in the format:
// - [Repository Name](https://github.com/username/repo-name) - Description
// Extract the repository name (what’s in [ ]),
// Extract the URL (what’s in ( )),
// Extract the description (the text after the -).
// Store it as a Repository object and add it to the list.

// so overall
//  Route /awesome-beginners is called.
// 2️ The raw markdown file is fetched from GitHub.
// 3️The MarkdownRepositoryExtractor parses the raw markdown into Repository objects.
// 4️ You now have a list of repository objects that look like this:
// [
//   {
//     "name": "freeCodeCamp",
//     "url": "https://github.com/freeCodeCamp/freeCodeCamp",
//     "description": "A coding curriculum",
//     "language": "JavaScript"
//   },
//   {
//     "name": "30-seconds-of-code",
//     "url": "https://github.com/30-seconds/30-seconds-of-code",
//     "description": "Short, reusable JavaScript snippets",
//     "language": "JavaScript"
//   }
// ]
//  This list is filtered using the query parameters you provided, such as query, language, tag, etc. using System.Text.Json.Serialization;
