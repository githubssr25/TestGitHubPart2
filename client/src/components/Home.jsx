import { useState } from "react";
import { searchTestFilterSearch } from "./manager/repositoryManager";

export const Home = () => {
  const [query, setQuery] = useState("");
  const [type, setType] = useState("repositories");
  const [language, setLanguage] = useState("");
  const [minStars, setMinStars] = useState("");
  const [maxStars, setMaxStars] = useState("");
  const [createdAfter, setCreatedAfter] = useState("");
  const [updatedAfter, setUpdatedAfter] = useState("");
  const [pushedBefore, setPushedBefore] = useState("");
  const [hasOpenIssues, setHasOpenIssues] = useState(false);
  const [topics, setTopics] = useState("");
  const [visibility, setVisibility] = useState("");
  const [readmeKeyword, setReadmeKeyword] = useState("");
  const [searchResults, setSearchResults] = useState([]);
  const [error, setError] = useState("");
  //1-9 SO WE GET BACK DATA BUT ITS NOT DISPLAYING PROPERLY I CONSOLE LOG DATA FROM HERE WE CAN RESTRUCTURE THIS PART THAT RECEIVS RESPOSNE
  // BETTER IN WAY SO IT ACTUALLY CORRESPODNS TO DATA STRUCTURE THAT IS NEXT TASK

  const handleSearchRepositories = async () => {
    if (!query.trim()) {
      setError("Query is required to perform a search.");
      return;
    }

    try {
      const data = await searchTestFilterSearch({
        query,
        type,
        language,
        minStars: minStars ? parseInt(minStars, 10) : null,
        maxStars: maxStars ? parseInt(maxStars, 10) : null,
        createdAfter,
        updatedAfter,
        pushedBefore,
        hasOpenIssues,
        topics,
        visibility,
        readmeKeyword,
      });
      console.log("what is data ie hte response", data);
      setSearchResults(data);
      setError("");
    } catch (err) {
      console.error(err);
      setError("Failed to fetch repositories.");
    }
  };

  return (
    <div className="home-container">
      <h1>GitHub Repository Search</h1>

      <form onSubmit={(e) => e.preventDefault()} className="search-form">
        <label>
          <span>Query (required):</span>
          <input
            type="text"
            value={query}
            onChange={(e) => setQuery(e.target.value)}
            placeholder="e.g., React, JavaScript"
            required
          />
        </label>

        <label>
          <span>Type:</span>
          <select value={type} onChange={(e) => setType(e.target.value)}>
            <option value="repositories">Repositories</option>
            <option value="issues">Issues</option>
          </select>
        </label>

        <label>
          <span>Language:</span>
          <input
            type="text"
            value={language}
            onChange={(e) => setLanguage(e.target.value)}
            placeholder="e.g., JavaScript"
          />
        </label>

        <label>
          <span>Min Stars:</span>
          <input
            type="number"
            value={minStars}
            onChange={(e) => setMinStars(e.target.value)}
            placeholder="e.g., 100"
          />
        </label>

        <label>
          <span>Max Stars:</span>
          <input
            type="number"
            value={maxStars}
            onChange={(e) => setMaxStars(e.target.value)}
            placeholder="e.g., 500"
          />
        </label>

        <label>
          <span>Created After:</span>
          <input
            type="date"
            value={createdAfter}
            onChange={(e) => setCreatedAfter(e.target.value)}
          />
        </label>

        <label>
          <span>Updated After:</span>
          <input
            type="date"
            value={updatedAfter}
            onChange={(e) => setUpdatedAfter(e.target.value)}
          />
        </label>

        <label>
          <span>Pushed Before:</span>
          <input
            type="date"
            value={pushedBefore}
            onChange={(e) => setPushedBefore(e.target.value)}
          />
        </label>

        <label>
          <span>Has Open Issues:</span>
          <input
            type="checkbox"
            checked={hasOpenIssues}
            onChange={(e) => setHasOpenIssues(e.target.checked)}
          />
        </label>

        <label>
          <span>Topics:</span>
          <input
            type="text"
            value={topics}
            onChange={(e) => setTopics(e.target.value)}
            placeholder="e.g., front-end, back-end"
          />
        </label>

        <label>
          <span>Visibility:</span>
          <select
            value={visibility}
            onChange={(e) => setVisibility(e.target.value)}
          >
            <option value="">Any</option>
            <option value="public">Public</option>
            <option value="private">Private</option>
          </select>
        </label>

        <label>
          <span>Readme Keywords:</span>
          <input
            type="text"
            value={readmeKeyword}
            onChange={(e) => setReadmeKeyword(e.target.value)}
            placeholder="e.g., beginner"
          />
        </label>

        <button type="button" onClick={handleSearchRepositories}>
          Search
        </button>
      </form>

      {error && <p className="error-message">{error}</p>}

      <div className="search-results">
        <h2>Search Results</h2>
        <ul>
          {searchResults.map((result, index) => (
            <li key={index}>
              <a
                href={result.htmlUrl}
                target="_blank"
                rel="noopener noreferrer"
              >
                {result.fullName}
              </a>
              <p>{result.description || "No description provided."}</p>
              <p>
                Stars: {result.stars} | Forks: {result.forks}
              </p>
              <p>
                Created At: {new Date(result.createdAt).toLocaleDateString()}
              </p>
              <p>
                Last Pushed: {new Date(result.pushedAt).toLocaleDateString()}
              </p>
              <p>
                Last Updated: {new Date(result.updatedAt).toLocaleDateString()}
              </p>
              <p>Language: {result.language || "Not specified"}</p>
              <p>
                Topics:{" "}
                {result.topics.length > 0 ? result.topics.join(", ") : "None"}
              </p>
              <p>
                Owner:{" "}
                <a
                  href={result.owner.htmlUrl}
                  target="_blank"
                  rel="noopener noreferrer"
                >
                  {result.owner.name}
                </a>
              </p>
              <p>Open Issues: {result.openIssues}</p>
              <p>
                Projects: {result.hasProjects ? "Yes" : "No"} | Issues Enabled:{" "}
                {result.hasIssues ? "Yes" : "No"}
              </p>
            </li>
          ))}
        </ul>
      </div>
    </div>
  );
};
