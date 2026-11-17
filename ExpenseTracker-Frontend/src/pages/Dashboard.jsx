import { Wallet, TrendingUp, Receipt, Tag } from "lucide-react";
import Sidebar from "../components/Sidebar";
import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
  PieChart,
  Pie,
  Cell,
} from "recharts";
import api from "../services/api";
import { useState, useEffect } from "react";
import Navbar from "../components/Navbar";
import SummaryCard from "../components/SummaryCard";

const Dashboard = () => {
  const [summary, setSummary] = useState({});
  const [monthlyData, setMonthlyData] = useState([]);
  const [categoryData, setCategoryData] = useState([]);
  const COLORS = ["#6366f1", "#22c55e", "#f59e0b", "#ec4899", "#06b6d4"];

  useEffect(() => {
    const getdata = async () => {
      try {
        const response = await api.get("/Dashboard");
        const responseMonthly = await api.get("/Dashboard/monthly-breakdown");
        const responseCategory = await api.get("/Dashboard/category-breakdown");

        setSummary(response.data);
        setMonthlyData(responseMonthly.data);
        setCategoryData(responseCategory.data);
      } catch (error) {
        console.log(error.response?.data || error.message);
      }
    };

    getdata();
  }, []);
  return (
    <div className="dashboard-container">
      <Sidebar />
      <div className="dashboard-main">
        <Navbar />
        <h1>Dashboard</h1>
        <p>Welcome back! Here's your financial overview.</p>

        <div className="summary-container">
          <SummaryCard
            title="Total Expenses"
            value={`₹${summary.totalExpense ?? 0}`}
            icon={<Wallet />}
          />

          <SummaryCard
            title="Monthly Expenses"
            value={`₹${summary.monthlyExpense ?? 0}`}
            icon={<TrendingUp />}
          />

          <SummaryCard
            title="Total Transactions"
            value={summary.totalTransactions ?? 0}
            icon={<Receipt />}
          />

          <SummaryCard
            title="Top Category"
            value={summary.topCategory || "N/A"}
            icon={<Tag />}
          />
        </div>

        <div className="charts-container">
          <div className="chart-card">
            <h3>Monthly Expenses</h3>

            <ResponsiveContainer width="100%" height={300}>
              <LineChart data={monthlyData}>
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="month" />
                <YAxis />
                <Tooltip />
                <Line
                  type="monotone"
                  dataKey="totalExpense"
                  stroke="#6366f1"
                  strokeWidth={3}
                />
              </LineChart>
            </ResponsiveContainer>
          </div>

          <div className="chart-card">
            <h3>Category Breakdown</h3>

            <ResponsiveContainer width="100%" height={300}>
              <PieChart>
                <Pie
                  data={categoryData}
                  dataKey="totalExpense"
                  nameKey="category"
                  outerRadius={100}
                  label
                >
                  {categoryData.map((entry, index) => (
                    <Cell
                      key={entry.category}
                      fill={COLORS[index % COLORS.length]}
                    />
                  ))}
                </Pie>
                <Tooltip />
              </PieChart>
            </ResponsiveContainer>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Dashboard;
