const _apiUrl = "http://localhost:5056/api/auth";

// Login method
export const login = async (email, password) => {
  try {
    console.log("Attempting to log in with:", { email, password });

    const response = await fetch(_apiUrl + "/login", {
      method: "POST",
      credentials: "include",  // Important for cookies/session
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ email, password }),
    });

    console.log("Login response status:", response.status);

    if (!response.ok) {
      const errorText = await response.text();
      console.error("Login failed with status:", response.status, "Message:", errorText);
      return null;
    }

    const data = await response.json();
    console.log("Login successful, response data:", data);

    // If GitHub authentication is required, redirect the user
    if (data.githubAuthUrl) {
      window.location.href = data.githubAuthUrl;
    }

    return data;
  } catch (error) {
    console.error("Error during login:", error.message);
    return null;
  }
};


// Logout method
export const logout = async () => {
  try {
    const response = await fetch(_apiUrl + "/logout", { method: "POST" });

    if (!response.ok) {
      throw new Error(`Logout failed: ${response.statusText}`);
    }

    return "Logout successful.";
  } catch (error) {
    console.error("Error during logout:", error);
    throw error;
  }
};

// Register method
export const register = async (userProfile) => {
  try {
    const response = await fetch(_apiUrl + "/register", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(userProfile),
    });

    if (!response.ok) {
      throw new Error(`Registration failed: ${response.statusText}`);
    }

    return await response.json(); // Response from the backend (e.g., success message)
  } catch (error) {
    console.error("Error during registration:", error);
    throw error;
  }
};

// Get User by ID method
export const getUserById = async (id) => {
  try {
    const response = await fetch(`${_apiUrl}/${id}`, {
      method: "GET",
    });

    if (!response.ok) {
      throw new Error(`Failed to fetch user: ${response.statusText}`);
    }

    return await response.json(); // User data
  } catch (error) {
    console.error("Error fetching user by ID:", error);
    throw error;
  }
};
