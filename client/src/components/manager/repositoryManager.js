const BASE_URL = "http://localhost:5056/api/repository";

// Fetch all repositories
export const getAllRepositories = async () => {
  try {
    const response = await fetch(`${BASE_URL}`, {
      method: "GET",
    });

    if (!response.ok) {
      throw new Error(`Error fetching repositories: ${response.statusText}`);
    }

    return await response.json();
  } catch (error) {
    console.error("Error fetching repositories:", error);
    throw error;
  }
};

// Fetch repositories by userId
export const getRepositoriesByUserId = async (userId) => {
  try {
    const response = await fetch(`${BASE_URL}/user/${userId}`, {
      method: "GET",
    });

    if (!response.ok) {
      throw new Error(`Error fetching repositories for user: ${response.statusText}`);
    }

    return await response.json();
  } catch (error) {
    console.error(`Error fetching repositories for user with ID ${userId}:`, error);
    throw error;
  }
};

export const getAnnotationsByUser = async (userId) => {
  // const BASE_URL = "http://localhost:5201/api/annotation";

  try {
    const response = await fetch(`${BASE_URL}/user/${userId}`, {
      method: "GET",
    });

    if (!response.ok) {
      throw new Error(`Error fetching annotations: ${response.statusText}`);
    }

    return await response.json();
  } catch (error) {
    console.error("Error fetching annotations by user:", error);
    throw error;
  }
};



export const createNewRepository = async (createRepositoryDto) => {
try {
  // Ensure userId is converted to an integer
  const response = await fetch(`${BASE_URL}`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(createRepositoryDto),
  });

  return await response.json();
} catch (error) {
  console.error("Error creating/associating repository", error);
}
};

export const deleteRepository = async (repositoryId) => {
  try {
    const response = await fetch(`${BASE_URL}/${repositoryId}`, {
      method: "DELETE",
    });

    if (!response.ok) {
      throw new Error(`Error deleting repository: ${response.statusText}`);
    }

    return true; // Return success flag
  } catch (error) {
    console.error("Error deleting repository:", error);
    throw error;
  }
};

export const getRecentIssuesDistinct = async (days = 7) => {
  try {
    const response = await fetch(`/api/recent-issues-distinct?days=${days}`, {
      method: "GET",
    });

    if (!response.ok) {
      throw new Error(`Error fetching recent distinct issues: ${response.statusText}`);
    }

    return await response.json(); // Parse and return JSON response
  } catch (error) {
    console.error("Error fetching recent distinct issues:", error);
    throw error;
  }
};

export const searchRepositories = async (params = {}) => {
  try {
    const queryParams = new URLSearchParams(params).toString();
    const response = await fetch(`${BASE_URL}/search?${queryParams}`, {
      method: "GET",
    });

    if (!response.ok) {
      throw new Error(`Error fetching repositories: ${response.statusText}`);
    }

    return await response.json();
  } catch (error) {
    console.error("Error searching repositories:", error);
    throw error;
  }
};

export const searchTestFilterSearch = async ({
  query,
  type = "repositories",
  goodFirstIssue = false,
  helpWanted = false,
  minStars = null,
  maxStars = null,
  language = null,
  createdAfter = null,
  updatedAfter = null,
  pushedBefore = null,
  hasOpenIssues = false,
  topics = null,
  visibility = null,
  readmeKeyword = null,
}) => {
  try {
    const BASE_URL = "http://localhost:5056"; // Update with your backend URL
    const url = new URL("/testFilterSearch", BASE_URL);

    // Append query parameters
    if (query) url.searchParams.append("query", query);
    if (type) url.searchParams.append("type", type);
    if (goodFirstIssue) url.searchParams.append("goodFirstIssue", goodFirstIssue.toString());
    if (helpWanted) url.searchParams.append("helpWanted", helpWanted.toString());
    if (minStars) url.searchParams.append("minStars", minStars);
    if (maxStars) url.searchParams.append("maxStars", maxStars);
    if (language) url.searchParams.append("language", language);
    if (createdAfter) url.searchParams.append("createdAfter", createdAfter);
    if (updatedAfter) url.searchParams.append("updatedAfter", updatedAfter);
    if (pushedBefore) url.searchParams.append("pushedBefore", pushedBefore);
    if (hasOpenIssues) url.searchParams.append("hasOpenIssues", hasOpenIssues.toString());
    if (topics) url.searchParams.append("topics", topics);
    if (visibility) url.searchParams.append("visibility", visibility);
    if (readmeKeyword) url.searchParams.append("readmeKeyword", readmeKeyword);

    const response = await fetch(url.toString(), {
      method: "GET",
    });

    if (!response.ok) {
      throw new Error(`Error fetching repositories: ${response.statusText}`);
    }

    return await response.json();
  } catch (error) {
    console.error("Error searching repositories:", error);
    throw error;
  }
};


















