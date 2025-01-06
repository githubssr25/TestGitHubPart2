import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { register } from "./manager/authManager";
import { Button, FormGroup, Input, Label } from "reactstrap";
// import "./Register.css";

const Register = ({ setLoggedInUser }) => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const user = await register(email, password);
      localStorage.setItem("loggedInUser", JSON.stringify(user));
      setLoggedInUser(user);
      navigate("/");
    } catch (err) {
      setError("Registration failed.");
      console.error(err);
    }
  };

  return (
    <div className="container">
      <h3>Register</h3>
      <form onSubmit={handleSubmit}>
        {error && <p className="text-danger">{error}</p>}
        <FormGroup>
          <Label>Email</Label>
          <Input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />
        </FormGroup>
        <FormGroup>
          <Label>Password</Label>
          <Input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
        </FormGroup>
        <Button>Register</Button>
      </form>
      <p>
        Already have an account? <Link to="/login">Login</Link>
      </p>
    </div>
  );
};

export default Register;
