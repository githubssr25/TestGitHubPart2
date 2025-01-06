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
    const storedUser = localStorage.getItem("loggedInUser");
    if (storedUser) {
      setLoggedInUser(JSON.parse(storedUser));
    } else {
      const storedUserId = localStorage.getItem("loggedInUserId");
      if (storedUserId) {
        getUserById(storedUserId).then((user) => {
          if (user) {
            localStorage.setItem("loggedInUser", JSON.stringify(user));
            setLoggedInUser(user);
          }
        });
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
      {/* <NavBar loggedInUser={loggedInUser} setLoggedInUser={setLoggedInUser} /> */}
      <ApplicationViews
        loggedInUser={loggedInUser}
        setLoggedInUser={setLoggedInUser}
      />
    </>
  );
}

export default App;
