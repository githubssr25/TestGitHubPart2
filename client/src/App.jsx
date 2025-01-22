import { useEffect, useState } from "react";
import "./App.css";
import "bootstrap/dist/css/bootstrap.min.css";
import { getUserById } from "./components/manager/userManager";
import { Spinner } from "reactstrap";
// import { NavBar } from "./components/NavBar";
import { ApplicationViews } from "./ApplicationViews";

function App() {
  const [loggedInUser, setLoggedInUser] = useState();

  useEffect(() => {
    const urlParams = new URLSearchParams(window.location.search);
    const userId = urlParams.get("userId");

    if (userId) {
      console.log("Received userId from query params:", userId);
      localStorage.setItem("loggedInUserId", userId);

      getUserById(userId).then((user) => {
        if (user) {
          localStorage.setItem("loggedInUser", JSON.stringify(user));
          setLoggedInUser(user);
          window.location.href = "/";  // Redirect to homepage after login
        }
      });

      // Clean up the URL to remove the query parameter
      window.history.replaceState({}, document.title, "/");
    } else {
      // If no userId in the URL, check local storage
      const storedUser = localStorage.getItem("loggedInUser");
      if (storedUser) {
        setLoggedInUser(JSON.parse(storedUser));
      } else {
        setLoggedInUser(null);
      }
    }
  }, []);

  if (loggedInUser === undefined) {
    return <Spinner />;
  }

  return (
    <>
      <ApplicationViews
        loggedInUser={loggedInUser}
        setLoggedInUser={setLoggedInUser}
      />
    </>
  );
}

export default App;
