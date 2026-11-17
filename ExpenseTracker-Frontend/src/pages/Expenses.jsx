import Sidebar from "../components/Sidebar";
import Navbar from "../components/Navbar";
import { Plus, Pencil, Trash2 } from "lucide-react";
import { useState, useEffect } from "react";
import api from "../services/api";
import axios from "axios";

const Expenses = () => {
  const [showForm, setShowForm] = useState(false);
  const [searchTerm, setSearchTerm] = useState("");
  const [selectedCategory, setSelectedCategory] = useState("All");
  const [newExpense, setNewExpense] = useState({
    title: "",
    category: "",
    amount: "",
    date: "",
    description: "",
  });
  const [editId, setEditId] = useState(null);
  const [expenses, setExpenses] = useState([]);

  useEffect(() => {
    const fetchExpenses = async () => {
      try {
        const response = await api.get("/Expense");
        setExpenses(response.data.expenses);
      } catch (error) {
        console.log(error.response.data || error.message);
      }
    };
    fetchExpenses();
  }, []);

  const handleChange = (e) => {
    setNewExpense({
      ...newExpense,
      [e.target.name]: e.target.value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (editId === null) {
      try {
        const response = await api.post("/Expense", {
          ...newExpense,
          amount: Number(newExpense.amount),
        });
        setExpenses([...expenses, response.data]);
      } catch (error) {
        console.log(error.response?.data || error.message);
        return;
      }
    } else {
      try {
        await api.put(`/Expense/${editId}`, {
          ...newExpense,
          amount: Number(newExpense.amount),
        });
        setExpenses(
          expenses.map((e) =>
            e.id === editId
              ? { ...e, ...newExpense, amount: Number(newExpense.amount) }
              : e,
          ),
        );
      } catch (error) {
        console.log(error.response?.data || error.message);
        return;
      }
    }

    setNewExpense({
      title: "",
      category: "",
      amount: "",
      date: "",
      description: "",
    });
    setShowForm(false);
    setEditId(null);
  };

  const editExpense = (Id) => {
    var expense = expenses.find((e) => e.id === Id);
    if (!expense) return;
    setNewExpense({
      title: expense.title,
      category: expense.category,
      amount: expense.amount,
      date: expense.date,
      description: expense.description || "",
    });
    setEditId(Id);
    setShowForm(true);
  };

  const deleteExpense = async (Id) => {
    try {
      await api.delete(`/Expense/${Id}`);
      setExpenses(expenses.filter((e) => e.id !== Id));
    } catch (error) {
      console.log(error.response?.data || error.message);
    }
  };
  const filteredExpenses = expenses.filter((expense) => {
    return (
      expense.title.toLowerCase().includes(searchTerm.toLowerCase()) &&
      (selectedCategory === "All" || expense.category === selectedCategory)
    );
  });

  return (
    <div className="dashboard-container">
      <Sidebar />

      <div className="dashboard-main">
        <Navbar />

        <div className="page-header">
          <div>
            <h1>Expenses</h1>
            <p>Manage and track all your expenses.</p>
          </div>

          <button
            className="add-button"
            onClick={() => {
              setEditId(null);
              setNewExpense({ title: "", category: "", amount: "", date: "" });
              setShowForm(true);
            }}
          >
            <Plus size={18} />
            Add Expense
          </button>
        </div>

        <div className="expense-filters">
          <input
            className="search-input"
            onChange={(e) => setSearchTerm(e.target.value)}
            value={searchTerm}
            type="text"
            placeholder="Search expense..."
          />
          <select
            className="category-filter"
            onChange={(e) => setSelectedCategory(e.target.value)}
            value={selectedCategory}
            name=""
            id=""
          >
            <option value="All">All Categories</option>
            <option value="Food">Food</option>
            <option value="Travel">Travel</option>
            <option value="Shopping">Shopping</option>
            <option value="Bills">Bills</option>
            <option value="Other">Other</option>
          </select>
        </div>

        {showForm && (
          <form className="expense-form" onSubmit={handleSubmit}>
            <input
              value={newExpense.title}
              onChange={handleChange}
              type="text"
              name="title"
              placeholder="Enter Title"
            />
            <select
              value={newExpense.category}
              onChange={handleChange}
              name="category"
            >
              <option value="">Select Category</option>
              <option value="Food">Food</option>
              <option value="Travel">Travel</option>
              <option value="Shopping">Shopping</option>
              <option value="Bills">Bills</option>
              <option value="Other">Other</option>
            </select>
            <input
              value={newExpense.amount}
              onChange={handleChange}
              type="number"
              name="amount"
              placeholder="Enter Amount"
            />
            <input
              value={newExpense.date}
              onChange={handleChange}
              type="date"
              name="date"
              placeholder="Enter Date"
            />
            <input
              type="text"
              name="description"
              value={newExpense.description || ""}
              onChange={handleChange}
              placeholder="Enter Descriptions"
            />
            <button type="submit">
              {editId === null ? "Add Expense" : "Update Expense"}
            </button>
          </form>
        )}

        <div className="expense-table">
          <table>
            <thead>
              <tr>
                <th>Title</th>
                <th>Category</th>
                <th>Amount</th>
                <th>Date</th>
                <th>Actions</th>
              </tr>
            </thead>

            <tbody>
              {filteredExpenses.length > 0 ? (
                filteredExpenses.map((expense) => (
                  <tr key={expense.id}>
                    <td>{expense.title}</td>
                    <td>{expense.category}</td>
                    <td>₹{expense.amount}</td>
                    <td>
                      {new Date(expense.date).toLocaleString("en-IN", {
                        day: "2-digit",
                        month: "short",
                        year: "numeric",
                        hour: "2-digit",
                        minute: "2-digit",
                        hour12: true,
                      })}
                    </td>
                    <td>
                      <button
                        onClick={() => editExpense(expense.id)}
                        className="action-button"
                      >
                        <Pencil size={17} />
                      </button>
                      <button
                        onClick={() => deleteExpense(expense.id)}
                        className="action-button delete"
                      >
                        <Trash2 size={17} />
                      </button>
                    </td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan={5}>No Expenses Found</td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
};

export default Expenses;
