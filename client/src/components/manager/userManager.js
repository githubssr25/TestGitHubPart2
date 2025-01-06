// UserManager.js
const BASE_URL = "http://localhost:5056/api/user";

// Fetch all users
export const getAllUsers = async () => {
  try {
    const response = await fetch(`${BASE_URL}`, {
      method: "GET",
    });

    if (!response.ok) {
      throw new Error(`Error fetching users: ${response.statusText}`);
    }

    return await response.json();
  } catch (error) {
    console.error("Error fetching all users:", error);
    throw error;
  }
};

// Fetch a user by ID
export const getUserById = async (userId) => {
  try {
    const response = await fetch(`${BASE_URL}/${userId}`, {
      method: "GET",
    });

    if (!response.ok) {
      throw new Error(`Error fetching user by ID: ${response.statusText}`);
    }

    return await response.json();
  } catch (error) {
    console.error(`Error fetching user with ID ${userId}:`, error);
    throw error;
  }
};
