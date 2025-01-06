import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { login } from "./manager/authManager";
import { Button, FormFeedback, FormGroup, Input, Label } from "reactstrap";
// import "./Login.css";
/* eslint-disable react/prop-types */
export const Login = ({ setLoggedInUser }) => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [failedLogin, setFailedLogin] = useState(false);
  const navigate = useNavigate();

  const handleSubmit = (e) => {
    e.preventDefault();
    login(email, password).then((user) => {
      if (!user) {
        setFailedLogin(true);
      } else {
        localStorage.setItem("loggedInUser", JSON.stringify(user));
        setLoggedInUser(user);
        navigate("/");
      }
    });
  };

  return (
    <div className="container">
      <h3>Login</h3>
      <FormGroup>
        <Label>Email</Label>
        <Input
          invalid={failedLogin}
          type="text"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
        />
      </FormGroup>
      <FormGroup>
        <Label>Password</Label>
        <Input
          invalid={failedLogin}
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />
        <FormFeedback>Login failed.</FormFeedback>
      </FormGroup>
      <Button onClick={handleSubmit}>Login</Button>
      {/* <p>
        Don't have an account? <Link to="/register">Register</Link>
      </p> */}
    </div>
  );
};

export default Login;
