import { useEffect, useState } from "react";
import api from "../services/api";
import Sidebar from "../components/Sidebar";
import Navbar from "../components/Navbar";

const frequencyValues = {
  Daily: 0,
  Weekly: 1,
  Monthly: 2,
  Yearly: 3,
};

const frequencyNames = ["Daily", "Weekly", "Monthly", "Yearly"];

const emptyForm = {
  title: "",
  amount: "",
  description: "",
  category: "",
  frequency: "Monthly",
  startDate: "",
  endDate: "",
};

function RecurringExpenses() {
  const [expenses, setExpenses] = useState([]);
  const [history, setHistory] = useState([]);
  const [activeTab, setActiveTab] = useState("active");

  const [form, setForm] = useState(emptyForm);
  const [editingId, setEditingId] = useState(null);
  const [showForm, setShowForm] = useState(false);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    fetchExpenses();
    fetchHistory();
  }, []);

  const fetchExpenses = async () => {
    try {
      const response = await api.get("/RecurringExpense");
      setExpenses(response.data);
    } catch (error) {
      console.log(error.response?.data || error.message);
    }
  };

  const fetchHistory = async () => {
    try {
      const response = await api.get("/RecurringExpense/history");
      setHistory(response.data);
    } catch (error) {
      console.log(error.response?.data || error.message);
    }
  };

  const handleChange = (e) => {
    setForm({
      ...form,
      [e.target.name]: e.target.value,
    });
  };

  const resetForm = () => {
    setForm(emptyForm);
    setEditingId(null);
    setShowForm(false);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const payload = {
      ...form,
      amount: Number(form.amount),
      frequency: frequencyValues[form.frequency],
      endDate: form.endDate || null,
    };

    setLoading(true);

    try {
      if (editingId) {
        await api.put(`/RecurringExpense/${editingId}`, payload);
        alert("Recurring expense updated successfully");
      } else {
        await api.post("/RecurringExpense", payload);
        alert("Recurring expense added successfully");
      }

      resetForm();
      await fetchExpenses();
      await fetchHistory();
    } catch (error) {
      console.log("API Error:", error.response?.data || error.message);

      const errorData = error.response?.data;

      if (typeof errorData === "string") {
        alert(errorData);
      } else {
        alert(JSON.stringify(errorData?.errors || errorData || error.message));
      }
    } finally {
      setLoading(false);
    }
  };

  const handleEdit = (expense) => {
    setForm({
      title: expense.title,
      amount: expense.amount,
      description: expense.description || "",
      category: expense.category,
      frequency:
        typeof expense.frequency === "number"
          ? frequencyNames[expense.frequency]
          : expense.frequency,
      startDate: expense.startDate?.split("T")[0] || "",
      endDate: expense.endDate?.split("T")[0] || "",
    });

    setEditingId(expense.id);
    setShowForm(true);
    setActiveTab("active");
  };

  const handleDeactivate = async (id) => {
    if (
      !window.confirm(
        "Are you sure you want to deactivate this recurring expense?",
      )
    ) {
      return;
    }

    try {
      await api.delete(`/RecurringExpense/${id}`);
      await fetchExpenses();
      await fetchHistory();
    } catch (error) {
      alert(error.response?.data || error.message);
    }
  };

  const handleRestore = async (id) => {
    try {
      await api.put(`/RecurringExpense/restore/${id}`);
      await fetchExpenses();
      await fetchHistory();
      alert("Recurring expense restored successfully");
    } catch (error) {
      alert(error.response?.data || error.message);
    }
  };

  const handlePermanentDelete = async (id) => {
    const confirmDelete = window.confirm(
      "Permanently delete this recurring rule? Previously generated expenses will be preserved.",
    );

    if (!confirmDelete) return;

    try {
      await api.delete(`/RecurringExpense/permanent/${id}`);
      await fetchHistory();
      alert("Recurring expense permanently deleted");
    } catch (error) {
      alert(error.response?.data || error.message);
    }
  };

  const formatDate = (date) => {
    if (!date) return "-";
    return new Date(date).toLocaleDateString("en-IN");
  };

  const displayedExpenses = activeTab === "active" ? expenses : history;

  return (
    <div className="dashboard-container">
      <Sidebar />

      <div className="dashboard-main">
        <Navbar />

        <div className="recurring-page">
          <div className="recurring-header">
            <div>
              <h1>Recurring Expenses</h1>
              <p>Manage your regular and scheduled expenses.</p>
            </div>

            {activeTab === "active" && (
              <button
                className="recurring-add-btn"
                onClick={() => {
                  resetForm();
                  setShowForm(true);
                }}
              >
                + Add Recurring Expense
              </button>
            )}
          </div>

          {showForm && activeTab === "active" && (
            <div className="recurring-form-card">
              <h2>
                {editingId ? "Edit Recurring Expense" : "Add Recurring Expense"}
              </h2>

              <form onSubmit={handleSubmit}>
                <div className="recurring-form-grid">
                  <div className="recurring-field">
                    <label>Title</label>
                    <input
                      name="title"
                      value={form.title}
                      onChange={handleChange}
                      placeholder="e.g. Monthly Rent"
                      required
                    />
                  </div>

                  <div className="recurring-field">
                    <label>Amount</label>
                    <input
                      type="number"
                      name="amount"
                      value={form.amount}
                      onChange={handleChange}
                      placeholder="Enter amount"
                      min="0.01"
                      step="0.01"
                      required
                    />
                  </div>

                  <div className="recurring-field">
                    <label>Category</label>
                    <input
                      name="category"
                      value={form.category}
                      onChange={handleChange}
                      placeholder="e.g. Rent, Bills, Subscription"
                      required
                    />
                  </div>

                  <div className="recurring-field">
                    <label>Frequency</label>
                    <select
                      name="frequency"
                      value={form.frequency}
                      onChange={handleChange}
                      required
                    >
                      <option value="Daily">Daily</option>
                      <option value="Weekly">Weekly</option>
                      <option value="Monthly">Monthly</option>
                      <option value="Yearly">Yearly</option>
                    </select>
                  </div>

                  <div className="recurring-field">
                    <label>Start Date</label>
                    <input
                      type="date"
                      name="startDate"
                      value={form.startDate}
                      onChange={handleChange}
                      required
                    />
                  </div>

                  <div className="recurring-field">
                    <label>End Date (Optional)</label>
                    <input
                      type="date"
                      name="endDate"
                      value={form.endDate}
                      onChange={handleChange}
                    />
                  </div>

                  <div className="recurring-field recurring-description">
                    <label>Description</label>
                    <textarea
                      name="description"
                      value={form.description}
                      onChange={handleChange}
                      placeholder="Optional description"
                    />
                  </div>
                </div>

                <div className="recurring-form-actions">
                  <button type="submit" disabled={loading}>
                    {loading
                      ? "Saving..."
                      : editingId
                        ? "Update Expense"
                        : "Add Expense"}
                  </button>

                  <button
                    type="button"
                    className="cancel-btn"
                    onClick={resetForm}
                  >
                    Cancel
                  </button>
                </div>
              </form>
            </div>
          )}

          <div className="recurring-tabs">
            <button
              className={activeTab === "active" ? "active-tab" : ""}
              onClick={() => {
                setActiveTab("active");
                resetForm();
              }}
            >
              Active ({expenses.length})
            </button>

            <button
              className={activeTab === "history" ? "active-tab" : ""}
              onClick={() => {
                setActiveTab("history");
                resetForm();
              }}
            >
              History ({history.length})
            </button>
          </div>

          <div className="recurring-table-card">
            <div className="recurring-table-heading">
              <h2>
                {activeTab === "active"
                  ? "Active Recurring Expenses"
                  : "Inactive History"}
              </h2>
            </div>

            <div className="recurring-table-wrapper">
              <table className="recurring-table">
                <thead>
                  <tr>
                    <th>Title</th>
                    <th>Amount</th>
                    <th>Category</th>
                    <th>Frequency</th>
                    <th>Start Date</th>
                    <th>End Date</th>
                    <th>Actions</th>
                  </tr>
                </thead>

                <tbody>
                  {displayedExpenses.length === 0 ? (
                    <tr>
                      <td colSpan="7" className="recurring-empty">
                        {activeTab === "active"
                          ? "No active recurring expenses found."
                          : "No history found."}
                      </td>
                    </tr>
                  ) : (
                    displayedExpenses.map((expense) => (
                      <tr key={expense.id}>
                        <td>{expense.title}</td>

                        <td>
                          ₹{Number(expense.amount).toLocaleString("en-IN")}
                        </td>

                        <td>{expense.category}</td>

                        <td>
                          {typeof expense.frequency === "number"
                            ? frequencyNames[expense.frequency]
                            : expense.frequency}
                        </td>

                        <td>{formatDate(expense.startDate)}</td>

                        <td>{formatDate(expense.endDate)}</td>

                        <td>
                          <div className="recurring-actions">
                            {activeTab === "active" ? (
                              <>
                                <button
                                  className="edit-action"
                                  onClick={() => handleEdit(expense)}
                                >
                                  Edit
                                </button>

                                <button
                                  className="deactivate-action"
                                  onClick={() => handleDeactivate(expense.id)}
                                >
                                  Deactivate
                                </button>
                              </>
                            ) : (
                              <>
                                <button
                                  className="restore-action"
                                  onClick={() => handleRestore(expense.id)}
                                >
                                  Restore
                                </button>

                                <button
                                  className="permanent-delete-action"
                                  onClick={() =>
                                    handlePermanentDelete(expense.id)
                                  }
                                >
                                  Delete Permanently
                                </button>
                              </>
                            )}
                          </div>
                        </td>
                      </tr>
                    ))
                  )}
                </tbody>
              </table>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default RecurringExpenses;
