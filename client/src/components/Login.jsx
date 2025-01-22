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

  const handleSubmit = async (e) => {
    e.preventDefault();
  
    try {
      const user = await login(email, password);
  
      if (!user) {
        setFailedLogin(true);
        return;
      }
  
      if (user.githubAuthUrl) {
        console.log("Redirecting to GitHub authentication...");
        window.location.href = user.githubAuthUrl;
        return;
      }
  
      localStorage.setItem("loggedInUser", JSON.stringify(user));
      setLoggedInUser(user);
      navigate("/");
    } catch (error) {
      console.error("Login submission error:", error);
    }
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
