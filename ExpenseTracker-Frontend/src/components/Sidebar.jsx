import {
  LayoutDashboard,
  Wallet,
  PieChart,
  Repeat,
  Upload,
  Settings,
} from "lucide-react";
import { NavLink } from "react-router-dom";

const Sidebar = () => {
  return (
    <div className="sidebar">
      <h2>ExpenseTracker</h2>
      <nav>
        <NavLink to="/dashboard">
          <LayoutDashboard /> Dashboard
        </NavLink>
        <NavLink to="/expenses">
          <Wallet /> Expenses
        </NavLink>
        <NavLink to="/budgets">
          <PieChart /> Budgets
        </NavLink>
        <NavLink to="/recurring">
          <Repeat /> Recurring Expenses
        </NavLink>
        <NavLink to="/import">
          <Upload /> Import CSV
        </NavLink>
      </nav>
    </div>
  );
};

export default Sidebar;
