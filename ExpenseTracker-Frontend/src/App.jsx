import Dashboard from "./pages/Dashboard";
import "./App.css";
import Login from "./pages/Login";
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import Expenses from "./pages/Expenses";
import ProtectedRoute from "./components/ProtectedRoute";
import Budgets from "./pages/Budgets";
import RecurringExpenses from "./pages/RecurringExpenses";
import ImportCSV from "./pages/ImportCsv";
import Register from "./pages/Register";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Register />} />
        <Route path="/register" element={<Register />} />
        <Route path="/login" element={<Login />} />
        <Route
          path="/expenses"
          element={
            <ProtectedRoute>
              <Expenses />
            </ProtectedRoute>
          }
        />
        <Route
          path="/dashboard"
          element={
            <ProtectedRoute>
              <Dashboard />
            </ProtectedRoute>
          }
        />
        <Route
          path="/budgets"
          element={
            <ProtectedRoute>
              <Budgets />
            </ProtectedRoute>
          }
        />
        <Route
          path="/recurring"
          element={
            <ProtectedRoute>
              <RecurringExpenses />
            </ProtectedRoute>
          }
        />
        <Route path="/import" element={<ImportCSV />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
