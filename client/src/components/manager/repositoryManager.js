const BASE_URL = "http://localhost:5201/api/repository";

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





















