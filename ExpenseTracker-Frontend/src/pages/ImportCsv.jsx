import { useState } from "react";
import api from "../services/api";
import Sidebar from "../components/Sidebar";
import Navbar from "../components/Navbar";

function ImportCSV() {
  const [file, setFile] = useState(null);
  const [loading, setLoading] = useState(false);
  const [result, setResult] = useState(null);
  const [error, setError] = useState("");

  const handleFileChange = (e) => {
    const selectedFile = e.target.files[0];

    setResult(null);
    setError("");

    if (!selectedFile) {
      setFile(null);
      return;
    }

    if (!selectedFile.name.toLowerCase().endsWith(".csv")) {
      setError("Only CSV files are allowed.");
      setFile(null);
      e.target.value = "";
      return;
    }

    if (selectedFile.size > 5 * 1024 * 1024) {
      setError("File size cannot exceed 5 MB.");
      setFile(null);
      e.target.value = "";
      return;
    }

    setFile(selectedFile);
  };

  const handleImport = async () => {
    if (!file) {
      setError("Please select a CSV file first.");
      return;
    }

    const formData = new FormData();
    formData.append("file", file);

    setLoading(true);
    setError("");
    setResult(null);

    try {
      const response = await api.post("/ExpenseImport", formData);

      setResult(response.data);
      setFile(null);

      // Reset file input
      const input = document.getElementById("csv-file-input");
      if (input) input.value = "";
    } catch (err) {
      const data = err.response?.data;

      if (typeof data === "string") {
        setError(data);
      } else if (data?.message) {
        setError(data.message);
      } else {
        setError("CSV import failed. Please check your file and try again.");
      }

      console.log("CSV Import Error:", data || err.message);
      console.log("Found Headers:", data?.foundHeaders);
    } finally {
      setLoading(false);
    }
  };

  const downloadSample = () => {
    const csvContent =
      "Title,Amount,Description,Date,Category\n" +
      "House Rent,8000,Monthly house rent,2026-11-01,Housing\n" +
      "Internet Bill,700,Monthly internet bill,2026-11-05,Bills\n" +
      "Groceries,1500,Weekly groceries,2026-11-08,Food";

    const blob = new Blob([csvContent], {
      type: "text/csv;charset=utf-8;",
    });

    const url = URL.createObjectURL(blob);
    const link = document.createElement("a");

    link.href = url;
    link.download = "expense-sample.csv";
    link.click();

    URL.revokeObjectURL(url);
  };

  return (
    <div className="dashboard-container">
      <Sidebar />

      <div className="dashboard-main">
        <Navbar />

        <div className="csv-import-page">
          <div className="csv-import-header">
            <div>
              <h1>Import Expenses</h1>
              <p>Upload a CSV file to import multiple expenses at once.</p>
            </div>
          </div>

          <div className="csv-import-card">
            <div className="csv-import-icon">
              <span>↑</span>
            </div>

            <h2>Upload CSV File</h2>
            <p className="csv-import-description">
              Select a CSV file containing your expense records.
            </p>

            <label htmlFor="csv-file-input" className="csv-file-label">
              Choose CSV File
            </label>

            <input
              id="csv-file-input"
              type="file"
              accept=".csv,text/csv"
              onChange={handleFileChange}
              hidden
            />

            {file && (
              <div className="csv-selected-file">
                <span>{file.name}</span>
                <span>{(file.size / 1024).toFixed(1)} KB</span>
              </div>
            )}

            <button
              className="csv-import-btn"
              onClick={handleImport}
              disabled={!file || loading}
            >
              {loading ? "Importing..." : "Import Expenses"}
            </button>

            {error && <div className="csv-error">{error}</div>}
          </div>

          <div className="csv-instructions-card">
            <h2>CSV File Requirements</h2>

            <ul>
              <li>Only CSV files are accepted.</li>
              <li>Maximum file size: 5 MB.</li>
              <li>All required column headers must be present.</li>
              <li>Amount must be a valid positive number.</li>
              <li>Date must be a valid date.</li>
            </ul>

            <h3>Required CSV Columns</h3>

            <div className="csv-columns">
              <span>Title</span>
              <span>Amount</span>
              <span>Description</span>
              <span>Date</span>
              <span>Category</span>
            </div>

            <button className="csv-sample-btn" onClick={downloadSample}>
              Download Sample CSV
            </button>
          </div>

          {result && (
            <div className="csv-result-card">
              <h2>Import Results</h2>

              <div className="csv-result-stats">
                <div className="csv-stat success">
                  <span>Successfully Imported</span>
                  <strong>{result.importedCount}</strong>
                </div>

                <div className="csv-stat failed">
                  <span>Failed Records</span>
                  <strong>{result.failedCount}</strong>
                </div>
              </div>

              {result.errors?.length > 0 && (
                <div className="csv-errors-list">
                  <h3>Import Errors</h3>

                  {result.errors.map((item, index) => (
                    <p key={index}>{item}</p>
                  ))}
                </div>
              )}

              <p className="csv-result-note">
                Imported expenses are now saved in your account.
              </p>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}

export default ImportCSV;
