import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import api from "../services/api";

const Login = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
    setError("");
    setLoading(true);

    try {
      const response = await api.post("/Auth/Login", {
        email,
        password,
      });

      localStorage.setItem("token", response.data.token);
      navigate("/dashboard");
    } catch (error) {
      setError(
        typeof error.response?.data === "string"
          ? error.response.data
          : error.response?.data?.message || "Invalid email or password.",
      );
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="login-container">
      <form onSubmit={handleLogin}>
        <h1>Welcome Back</h1>
        <p>Login to manage your expenses.</p>

        <input
          type="email"
          name="email"
          placeholder="Enter Your Email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
        />

        <input
          type="password"
          name="password"
          placeholder="Enter Your Password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
        />

        {error && <div className="auth-error">{error}</div>}

        <button type="submit" disabled={loading}>
          {loading ? "Logging in..." : "Login"}
        </button>

        <div className="auth-footer">
          Don't have an account? <Link to="/">Sign Up</Link>
        </div>
      </form>
    </div>
  );
};

export default Login;
