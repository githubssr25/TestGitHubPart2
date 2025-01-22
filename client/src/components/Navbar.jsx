import { Link } from "react-router-dom";
// import "./NavBar.css"; // Import the custom CSS file

/* eslint-disable react/prop-types */
export const NavBar = ({ loggedInUser, setLoggedInUser }) => {
  const handleLogout = () => {
    localStorage.removeItem("loggedInUser");
    setLoggedInUser(null);
  };

  const userId = loggedInUser.id;

  return (
    <nav className="navbar">
      <ul className="navbar-links">
        <li>
          <Link to="/">Home</Link>
        </li>
        <li>
          <Link to="/users">Users</Link>
        </li>
        <li>
          <Link to="/repositories">View All Saved Repositories</Link>
        </li>
        <li>
          <Link to="/repositories/create">Create A New Repository</Link>
        </li>
        <li>
          <Link to="/annotations">View All Your Annotations</Link>
        </li>
        <li>
          <Link to="/annotations/add">Create A New Annotation</Link>
        </li>
        <li>
          <Link to="/search/issues"> Search Issues </Link>
        </li>
        <li>
          <Link to={`/repositories/user/${userId}`}>Get Your Saved Repositories</Link>
        </li>
        <li>
          <button className="logout-button" onClick={handleLogout}>
            Logout
          </button>
        </li>
      </ul>
    </nav>
  );
};