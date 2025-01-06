import { Navigate } from "react-router-dom";
import PropTypes from "prop-types";

const AuthorizedRoute = ({ children, loggedInUser, roles, all }) => {
  let authed = false;

  if (loggedInUser) {
    if (roles && roles.length) {
      authed = all
        ? roles.every((r) => loggedInUser.roles.includes(r))
        : roles.some((r) => loggedInUser.roles.includes(r));
    } else {
      authed = true;
    }
  }

  return authed ? children : <Navigate to="/login" replace />;
};

AuthorizedRoute.propTypes = {
  loggedInUser: PropTypes.object, // Adjust based on your user object.
  children: PropTypes.element.isRequired,
  roles: PropTypes.arrayOf(PropTypes.string), // Optional roles for authorization.
  all: PropTypes.bool, // Whether all roles must match (default: false).
};

AuthorizedRoute.defaultProps = {
  roles: [],
  all: false,
};

export default AuthorizedRoute;



//import { Navigate } from "react-router-dom";
// const AuthorizedRoute = ({ loggedInUser, children }) => {
//   if (!loggedInUser) {
//     return <Navigate to="/login" replace />;
//   }
//   return children;
// };

// export default AuthorizedRoute;
