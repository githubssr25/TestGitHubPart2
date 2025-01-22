

// SearchIssues.jsx
import { useState } from "react";
import { searchIssues } from "./manager/issueManager";

export const SearchIssues = () => {
  const [query, setQuery] = useState("");
  const [goodFirstIssue, setGoodFirstIssue] = useState(false);
  const [helpWanted, setHelpWanted] = useState(false);
  const [createdAfter, setCreatedAfter] = useState("");
  const [updatedAfter, setUpdatedAfter] = useState("");
  const [minOpenIssues, setMinOpenIssues] = useState("");
  const [searchResults, setSearchResults] = useState([]);
  const [error, setError] = useState("");

  const handleSearchIssues = async () => {
    if (!query.trim()) {
      setError("Query parameter is required.");
      return;
    }

    try {
      const data = await searchIssues({
        query,
        goodFirstIssue,
        helpWanted,
        createdAfter,
        updatedAfter,
        minOpenIssues: minOpenIssues ? parseInt(minOpenIssues, 10) : null
      });
      setSearchResults(data.items);
      setError("");
    } catch (err) {
      console.error(err);
      setError("Failed to fetch issues.");
    }
  };

  return (
    <div className="issues-container">
      <h1>GitHub Issues Search</h1>

      <form onSubmit={(e) => e.preventDefault()} className="search-form">
        <label>
          <span>Query (required):</span>
          <input
            type="text"
            value={query}
            onChange={(e) => setQuery(e.target.value)}
            placeholder="e.g., bug, feature"
            required
          />
        </label>

        <label>
          <span>Good First Issue:</span>
          <input
            type="checkbox"
            checked={goodFirstIssue}
            onChange={(e) => setGoodFirstIssue(e.target.checked)}
          />
        </label>

        <label>
          <span>Help Wanted:</span>
          <input
            type="checkbox"
            checked={helpWanted}
            onChange={(e) => setHelpWanted(e.target.checked)}
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
          <span>Minimum Open Issues:</span>
          <input
            type="number"
            value={minOpenIssues}
            onChange={(e) => setMinOpenIssues(e.target.value)}
            placeholder="e.g., 5"
          />
        </label>

        <button type="button" onClick={handleSearchIssues}>
          Search
        </button>
      </form>

      {error && <p className="error-message">{error}</p>}

      <div className="search-results">
        <h2>Search Results</h2>
        <ul>
          {searchResults.map((issue) => (
            <li key={issue.id} className="issue-item">
              <div className="issue-header">
                <h3>
                  <a href={issue.htmlUrl} target="_blank" rel="noopener noreferrer">
                    {issue.title}
                  </a>
                </h3>
              </div>

              <div className="issue-details">
                <p>State: {issue.state}</p>
                <p>Comments: {issue.comments}</p>
                {issue.body && (
                  <p className="issue-body">{issue.body.substring(0, 200)}...</p>
                )}
              </div>

              <div className="issue-dates">
                <p>Created: {issue.createdAt ? new Date(issue.createdAt).toLocaleDateString() : "N/A"}</p>
                <p>Updated: {issue.updatedAt ? new Date(issue.updatedAt).toLocaleDateString() : "N/A"}</p>
              </div>

              {issue.labels && issue.labels.length > 0 && (
                <div className="issue-labels">
                  <p>Labels:</p>
                  <div className="labels-list">
                    {issue.labels.map((label, index) => (
                      <span key={index} className="label">
                        {label}
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
};