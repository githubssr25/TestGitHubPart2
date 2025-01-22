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
            <select value={visibility} onChange={(e) => setVisibility(e.target.value)}>
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
            {searchResults.map((repo) => (
              <li key={repo.id} className="repository-item">
                <div className="repo-header">
                  <h3>
                    <a href={repo.htmlUrl} target="_blank" rel="noopener noreferrer">
                      {repo.fullName}
                    </a>
                  </h3>
                  <p className="repo-description">
                    {repo.description || "No description provided."}
                  </p>
                </div>
    
                <div className="repo-stats">
                  <span className="stat-item">Stars: {repo.stars}</span>
                  <span className="stat-item">Forks: {repo.forks}</span>
                  <span className="stat-item">Watchers: {repo.watchersCount}</span>
                  {repo.openIssuesCount && (
                    <span className="stat-item">Open Issues: {repo.openIssuesCount}</span>
                  )}
                </div>
    
                <div className="repo-details">
                  <div className="details-section">
                    <p>Language: {repo.language || "Not specified"}</p>
                    <p>Visibility: {repo.visibility}</p>
                    {repo.license && <p>License: {repo.license}</p>}
                  </div>
    
                  <div className="repo-features">
                    <p>Repository Features:</p>
                    <p>Issues: {repo.hasIssues ? "Enabled" : "Disabled"}</p>
                    <p>Projects: {repo.hasProjects ? "Enabled" : "Disabled"}</p>
                  </div>
                </div>
    
                <div className="repo-dates">
                  <h4>Timeline</h4>
                  <div className="dates-grid">
                    <p>Created: {repo.createdAt ? new Date(repo.createdAt).toLocaleDateString() : "N/A"}</p>
                    <p>Updated: {repo.updatedAt ? new Date(repo.updatedAt).toLocaleDateString() : "N/A"}</p>
                    <p>Pushed: {repo.pushedAt ? new Date(repo.pushedAt).toLocaleDateString() : "N/A"}</p>
                  </div>
                </div>
    
                <div className="repo-links">
                  {repo.ownerLogin && (
                    <p>
                      Owner:{" "}
                      <a href={repo.ownerHtmlUrl} target="_blank" rel="noopener noreferrer">
                        {repo.ownerLogin}
                      </a>
                    </p>
                  )}
                  <div className="links-grid">
                    {repo.cloneUrl && (
                      <p>
                        Clone URL: <a href={repo.cloneUrl}>View</a>
                      </p>
                    )}
                    {repo.issuesUrl && (
                      <p>
                        Issues: <a href={repo.issuesUrl}>View</a>
                      </p>
                    )}
                    {repo.pullsUrl && (
                      <p>
                        Pull Requests: <a href={repo.pullsUrl}>View</a>
                      </p>
                    )}
                    {repo.releasesUrl && (
                      <p>
                        Releases: <a href={repo.releasesUrl}>View</a>
                      </p>
                    )}
                  </div>
                </div>
    
                {repo.topics && repo.topics.length > 0 && (
                  <div className="topics-section">
                    <h4>Topics</h4>
                    <div className="topics-list">
                      {repo.topics.map((topic, index) => (
                        <span key={index} className="topic">
                          {topic}
                        </span>
                      ))}
                    </div>
                  </div>
                )}
              </li>
            ))}
          </ul>
        </div>
      </div>
    );
}