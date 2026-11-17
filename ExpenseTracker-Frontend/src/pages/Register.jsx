import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import api from "../services/api";

const Register = () => {
  const navigate = useNavigate();

  const [fullName, setFullName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const handleRegister = async (e) => {
    e.preventDefault();
    setError("");

    if (password !== confirmPassword) {
      setError("Passwords do not match.");
      return;
    }

    setLoading(true);

    try {
      await api.post("/Auth/Register", {
        fullName,
        email,
        password,
      });

      navigate("/login");
    } catch (err) {
      setError(
        typeof err.response?.data === "string"
          ? err.response.data
          : err.response?.data?.message || "Registration failed.",
      );
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="login-container">
      <form onSubmit={handleRegister}>
        <h1>Create Account</h1>
        <p>Start managing your expenses today.</p>

        <input
          type="text"
          placeholder="Full Name"
          value={fullName}
          onChange={(e) => setFullName(e.target.value)}
          required
        />

        <input
          type="email"
          placeholder="Email Address"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
        />

        <input
          type="password"
          placeholder="Password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
        />

        <input
          type="password"
          placeholder="Confirm Password"
          value={confirmPassword}
          onChange={(e) => setConfirmPassword(e.target.value)}
          required
        />

        {error && <div className="auth-error">{error}</div>}

        <button type="submit" disabled={loading}>
          {loading ? "Creating Account..." : "Sign Up"}
        </button>

        <div className="auth-footer">
          Already have an account? <Link to="/login">Login</Link>
        </div>
      </form>
    </div>
  );
};

export default Register;
