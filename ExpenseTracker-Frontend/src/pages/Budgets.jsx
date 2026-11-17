import { useState, useEffect } from "react";
import {
  Plus,
  Pencil,
  Trash2,
  X,
  Wallet,
  TrendingDown,
  PiggyBank,
} from "lucide-react";

import Sidebar from "../components/Sidebar";
import Navbar from "../components/Navbar";
import api from "../services/api";

const Budgets = () => {
  const [budgets, setBudgets] = useState([]);
  const [budgetStatus, setBudgetStatus] = useState([]);
  const [showForm, setShowForm] = useState(false);
  const [editId, setEditId] = useState(null);

  const [newBudget, setNewBudget] = useState({
    category: "",
    amount: "",
    month: new Date().getMonth() + 1,
    year: new Date().getFullYear(),
  });

  const fetchBudgets = async () => {
    try {
      const month = new Date().getMonth() + 1;
      const year = new Date().getFullYear();

      const [budgetRes, statusRes] = await Promise.all([
        api.get("/Budget"),
        api.get(`/Budget/status?month=${month}&year=${year}`),
      ]);

      setBudgets(budgetRes.data);
      setBudgetStatus(statusRes.data);
    } catch (error) {
      console.log(error.response?.data || error.message);
    }
  };

  useEffect(() => {
    fetchBudgets();
  }, []);

  const totalBudget = budgetStatus.reduce(
    (sum, budget) => sum + budget.budgetAmount,
    0,
  );

  const totalSpent = budgetStatus.reduce(
    (sum, budget) => sum + budget.actualExpense,
    0,
  );

  const remainingBudget = budgetStatus.reduce(
    (sum, budget) => sum + budget.remainingAmount,
    0,
  );

  const handleChange = (e) => {
    setNewBudget({
      ...newBudget,
      [e.target.name]: e.target.value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const payload = {
      ...newBudget,
      amount: Number(newBudget.amount),
      month: Number(newBudget.month),
      year: Number(newBudget.year),
    };

    try {
      if (editId === null) {
        await api.post("/Budget", payload);
      } else {
        await api.put(`/Budget/${editId}`, payload);
      }

      setNewBudget({
        category: "",
        amount: "",
        month: new Date().getMonth() + 1,
        year: new Date().getFullYear(),
      });

      setEditId(null);
      setShowForm(false);
      await fetchBudgets();
    } catch (error) {
      alert(error.response?.data || error.message);
    }
  };

  const editBudget = (budget) => {
    setNewBudget({
      category: budget.category,
      amount: budget.amount,
      month: budget.month,
      year: budget.year,
    });

    setEditId(budget.id);
    setShowForm(true);
  };

  const deleteBudget = async (id) => {
    if (!window.confirm("Are you sure you want to delete this budget?")) {
      return;
    }

    try {
      await api.delete(`/Budget/${id}`);
      await fetchBudgets();
    } catch (error) {
      alert(error.response?.data || error.message);
    }
  };

  const closeForm = () => {
    setShowForm(false);
    setEditId(null);
    setNewBudget({
      category: "",
      amount: "",
      month: new Date().getMonth() + 1,
      year: new Date().getFullYear(),
    });
  };

  return (
    <div className="dashboard-container">
      <Sidebar />

      <div className="dashboard-main">
        <Navbar />

        <div className="page-header">
          <div>
            <h1>Budget Management</h1>
            <p>Plan your monthly spending and track your limits.</p>
          </div>

          <button className="primary-btn" onClick={() => setShowForm(true)}>
            <Plus size={18} /> Add Budget
          </button>
        </div>

        {/* Budget Overview */}
        <div className="summary-grid">
          <div className="summary-card">
            <div className="summary-icon">
              <Wallet size={22} />
            </div>
            <p>Total Budget</p>
            <h2>₹{totalBudget.toLocaleString("en-IN")}</h2>
          </div>

          <div className="summary-card">
            <div className="summary-icon">
              <TrendingDown size={22} />
            </div>
            <p>Total Spent</p>
            <h2>₹{totalSpent.toLocaleString("en-IN")}</h2>
          </div>

          <div className="summary-card">
            <div className="summary-icon">
              <PiggyBank size={22} />
            </div>
            <p>Remaining Budget</p>
            <h2>₹{remainingBudget.toLocaleString("en-IN")}</h2>
          </div>
        </div>

        {/* Budget Form */}
        {showForm && (
          <div className="form-card">
            <div className="form-header">
              <h2>{editId === null ? "Add Budget" : "Edit Budget"}</h2>
              <button onClick={closeForm} className="icon-btn">
                <X size={20} />
              </button>
            </div>

            <form onSubmit={handleSubmit}>
              <div className="form-group">
                <label>Category</label>
                <select
                  name="category"
                  value={newBudget.category}
                  onChange={handleChange}
                  required
                >
                  <option value="">Select Category</option>
                  <option value="Food">Food</option>
                  <option value="Travel">Travel</option>
                  <option value="Shopping">Shopping</option>
                  <option value="Bills">Bills</option>
                  <option value="Entertainment">Entertainment</option>
                  <option value="Health">Health</option>
                  <option value="Education">Education</option>
                  <option value="Other">Other</option>
                </select>
              </div>

              <div className="form-group">
                <label>Budget Amount</label>
                <input
                  type="number"
                  name="amount"
                  value={newBudget.amount}
                  onChange={handleChange}
                  min="0.01"
                  step="0.01"
                  placeholder="Enter amount"
                  required
                />
              </div>

              <div className="form-group">
                <label>Month</label>
                <select
                  name="month"
                  value={newBudget.month}
                  onChange={handleChange}
                  required
                >
                  {Array.from({ length: 12 }, (_, i) => (
                    <option key={i + 1} value={i + 1}>
                      {new Date(2000, i).toLocaleString("en-IN", {
                        month: "long",
                      })}
                    </option>
                  ))}
                </select>
              </div>

              <div className="form-group">
                <label>Year</label>
                <input
                  type="number"
                  name="year"
                  value={newBudget.year}
                  onChange={handleChange}
                  min="2000"
                  max="2100"
                  required
                />
              </div>

              <button type="submit" className="primary-btn">
                {editId === null ? "Save Budget" : "Update Budget"}
              </button>
            </form>
          </div>
        )}

        {/* Budget Status */}
        <div className="table-card">
          <h2>Current Month Budget Status</h2>

          <div className="table-responsive">
            <table>
              <thead>
                <tr>
                  <th>Category</th>
                  <th>Budget</th>
                  <th>Spent</th>
                  <th>Remaining</th>
                  <th>Status</th>
                </tr>
              </thead>

              <tbody>
                {budgetStatus.length > 0 ? (
                  budgetStatus.map((item, index) => (
                    <tr key={index}>
                      <td>{item.category}</td>
                      <td>₹{item.budgetAmount.toLocaleString("en-IN")}</td>
                      <td>₹{item.actualExpense.toLocaleString("en-IN")}</td>
                      <td>₹{item.remainingAmount.toLocaleString("en-IN")}</td>
                      <td>
                        <span
                          className={
                            item.isExceeded ? "status-danger" : "status-success"
                          }
                        >
                          {item.isExceeded ? "Exceeded" : "Within Budget"}
                        </span>
                      </td>
                    </tr>
                  ))
                ) : (
                  <tr>
                    <td colSpan="5">No budget found for this month.</td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
        </div>

        {/* All Budgets */}
        <div className="table-card">
          <h2>All Budgets</h2>

          <div className="table-responsive">
            <table>
              <thead>
                <tr>
                  <th>Category</th>
                  <th>Amount</th>
                  <th>Month</th>
                  <th>Year</th>
                  <th>Actions</th>
                </tr>
              </thead>

              <tbody>
                {budgets.length > 0 ? (
                  budgets.map((budget) => (
                    <tr key={budget.id}>
                      <td>{budget.category}</td>
                      <td>₹{budget.amount.toLocaleString("en-IN")}</td>
                      <td>
                        {new Date(2000, budget.month - 1).toLocaleString(
                          "en-IN",
                          { month: "long" },
                        )}
                      </td>
                      <td>{budget.year}</td>
                      <td>
                        <button
                          className="icon-btn"
                          onClick={() => editBudget(budget)}
                        >
                          <Pencil size={17} />
                        </button>

                        <button
                          className="icon-btn"
                          onClick={() => deleteBudget(budget.id)}
                        >
                          <Trash2 size={17} />
                        </button>
                      </td>
                    </tr>
                  ))
                ) : (
                  <tr>
                    <td colSpan="5">No budgets added yet.</td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Budgets;
