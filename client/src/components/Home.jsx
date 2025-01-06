
export const Home = () => {
    const githubSearchUrl =
    "https://github.com/search?q=language:javascript+stars:>100+pushed:>2023-01-01";
    const searchUrl = "https://docs.github.com/en/search-github/github-code-search/understanding-github-code-search-syntax";
  
    return (
      <div className="home-container">
        <h1 className="home-title">Welcome to NSS Final Project</h1>
  
        <p className="home-paragraph">For Some Info On Search Tips</p>
        <a
          href={searchUrl}
          target="_blank"
          className="btn btn-primary home-link"
          rel="noopener noreferrer"
        >
          Click Here For Search Tips
        </a>
  
        <p className="home-paragraph">Search for open source projects on GitHub to contribute to.</p>
      <a
        href={githubSearchUrl}
        target="_blank"
        rel="noopener noreferrer"
        className="btn btn-primary my-3"
      >
        Click this Link to Access GitHub and Begin Your Search
      </a>
      <div className="card mt-3">
        <div className="card-body">
          <h5 className="card-title">Common Features to Search By:</h5>
          <ul>
            <li>Language</li>
            <li>Stars</li>
            <li>Labels</li>
            <li>Last Updated</li>
            <li>Topics</li>
            <li>Issue/PR Filters</li>
          </ul>
        </div>
      </div>
    </div>
  );
  };