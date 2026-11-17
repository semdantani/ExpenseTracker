import { Bell, Search, UserCircle, LogOut } from "lucide-react";
import { useNavigate } from "react-router-dom";

const Navbar = () => {
  const navigate = useNavigate();

  const handleLogout = () => {
    localStorage.removeItem("token");
    navigate("/", { replace: true });
  };

  return (
    <div className="navbar">
      <div className="search-box">
        <Search />
        <input type="text" placeholder="Search expenses..." />
      </div>

      <div className="navbar-right">
        <Bell className="navbar-icon" />
        <UserCircle className="navbar-icon" />

        <span>Welcome, User</span>

        <button className="logout-btn" onClick={handleLogout}>
          <LogOut size={17} />
          <span>Logout</span>
        </button>
      </div>
    </div>
  );
};

export default Navbar;
