// issueManager.js
export const searchIssues = async ({
    query,
    goodFirstIssue = false,
    helpWanted = false,
    createdAfter = null,
    updatedAfter = null,
    minOpenIssues = null
  }) => {
    try {
      const BASE_URL = "http://localhost:5056";
      const url = new URL("/api/issues/search-issues", BASE_URL);
  
      // Append query parameters
      if (query) url.searchParams.append("query", query);
      if (goodFirstIssue) url.searchParams.append("goodFirstIssue", goodFirstIssue.toString());
      if (helpWanted) url.searchParams.append("helpWanted", helpWanted.toString());
      if (createdAfter) url.searchParams.append("createdAfter", createdAfter);
      if (updatedAfter) url.searchParams.append("updatedAfter", updatedAfter);
      if (minOpenIssues) url.searchParams.append("minOpenIssues", minOpenIssues.toString());
  
      const response = await fetch(url.toString(), {
        method: "GET",
        credentials: "include", // Add this for cookies
        headers: {
          "Accept": "application/json",
          "Content-Type": "application/json"
        }
      });
  
      if (!response.ok) {
        throw new Error(`Error fetching issues: ${response.statusText}`);
      }
  
      return await response.json();
    } catch (error) {
      console.error("Error searching issues:", error);
      throw error;
    }
  };


  export const createIssue = async (issueData) => {
    try {
      // First verify repository exists and user has access
    const verifyRepoUrl = `https://api.github.com/repos/${issueData.repositoryOwner}/${issueData.repositoryName}`;
    const verifyResponse = await fetch(verifyRepoUrl);
    if (!verifyResponse.ok) {
      throw new Error("Repository not found or no access");
    }
      const response = await fetch("http://localhost:5056/api/issues/create", {
        method: "POST",
        credentials: "include",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          title: issueData.title,
          body: issueData.body,
          repositoryOwner: issueData.repositoryOwner,
          repositoryName: issueData.repositoryName,
          labels: issueData.labels || []
        })
      });
  
      if (!response.ok) {
        throw new Error(`Error creating issue: ${response.statusText}`);
      }
  
      const createdIssue = await response.json();
    
      // Verify issue was created by fetching it
      const issueUrl = createdIssue.html_url;
      window.open(issueUrl, '_blank'); // Show user the created issue
      
      return createdIssue;
    } catch (error) {
      console.error("Error creating issue:", error);
      throw error;
    }
  };

  export const saveLocalIssue = async (issue) => {
    const response = await fetch("http://localhost:5056/api/issues", {
      method: "POST",
      credentials: "include",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(issue)
    });
  
    if (!response.ok) {
      throw new Error("Failed to save issue locally");
    }
  
    return await response.json();
  };