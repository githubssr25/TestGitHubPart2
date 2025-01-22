import { Route, Routes, Outlet } from "react-router-dom";
import AuthorizedRoute from "./AuthorizedRoute";
import Login from "./components/Login"; // Correct path
import Register from "./components/Register"; // Correct path
import { Home } from "./components/Home"; // Update based on the `manager` folder
import {SearchRepositories} from "./components/SearchRepositories"; // Correct path
import { NavBar } from "./components/Navbar"; // Correct path
import {SearchIssues} from "./components/SearchIssues";


/* eslint-disable react/prop-types */
export const ApplicationViews = ({ loggedInUser, setLoggedInUser }) => {
  return (
    <Routes>
      {/* Non-Authorized Routes */}
      <Route path="login" element={<Login setLoggedInUser={setLoggedInUser} />} />
      <Route path="register" element={<Register setLoggedInUser={setLoggedInUser} />} />

      {/* Authorized Routes */}
      <Route
        path="/"
        element={
          <AuthorizedRoute loggedInUser={loggedInUser}>
            <>
              <NavBar loggedInUser={loggedInUser} setLoggedInUser={setLoggedInUser} />
              <Outlet />
            </>
          </AuthorizedRoute>
        }
      >
        <Route index element={<Home />} />
        <Route path="search" element={<SearchRepositories />} />
        <Route path="search/issues" element={<SearchIssues />} />
      </Route>
    </Routes>
  );
};
