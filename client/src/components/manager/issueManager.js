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