# Automated Expense Tracker & Smart Wealth Analytics System

A full-stack expense management application built using **ASP.NET Core Web API and React**. It helps users manage daily expenses, track budgets, monitor recurring expenses, import expenses through CSV files, and analyze spending patterns through an interactive dashboard.

## Features

- **User Authentication:** Secure registration and login using JWT authentication.
- **Expense Management:** Create, view, update, and delete expenses.
- **Expense Filtering:** Filter expenses by category and date.
- **Pagination & Sorting:** Organize expense records efficiently.
- **Dashboard Analytics:** View total expenses, monthly spending, and category-wise breakdowns.
- **Budget Management:** Create and manage monthly budgets.
- **Budget Tracking:** Monitor actual spending, remaining balance, and exceeded budgets.
- **Recurring Expenses:** Manage recurring expenses and their history.
- **Restore Functionality:** Restore previously deleted recurring expenses.
- **CSV Import:** Import multiple expenses using CSV files.
- **Background Service:** Automatically process recurring expenses.

## Technology Stack

### Backend

- C#
- ASP.NET Core Web API (.NET 10)
- Entity Framework Core
- SQL Server
- LINQ
- JWT Authentication
- Repository Pattern
- Dependency Injection
- CsvHelper

### Frontend

- React
- JavaScript
- Vite
- React Router
- Axios
- Recharts
- Lucide React

### Development Tools

- Visual Studio 2026
- SQL Server Management Studio (SSMS)
- Swagger / OpenAPI
- Postman
- Git & GitHub

## Project Structure

```text
ExpenseTracker/
├── ExpenseTracker/
│   ├── Controllers/
│   ├── DTOs/
│   ├── Data/
│   ├── Model/
│   ├── Repositories/
│   ├── Services/
│   └── Migrations/
│
├── ExpenseTracker-Frontend/
│   ├── src/
│   ├── public/
│   └── package.json
│
└── ExpenseTracker.slnx
```

## Getting Started

### Prerequisites

- .NET 10 SDK
- SQL Server
- SQL Server Management Studio (SSMS)
- Node.js and npm
- Visual Studio 2026

### 1. Clone the Repository

```bash
git clone https://github.com/semdantani/ExpenseTracker.git
cd ExpenseTracker
```

### 2. Backend Setup

Open the solution in Visual Studio.

Configure the SQL Server connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ExpenseTrackerDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

**Note:** This connection string uses Windows Authentication. Update the server name according to your local SQL Server configuration.

Apply Entity Framework Core migrations:

```bash
dotnet ef database update
```

Run the ASP.NET Core Web API project from Visual Studio.

Swagger UI can be used to explore and test the API endpoints.

### 3. Frontend Setup

Open a terminal in the frontend directory:

```bash
cd ExpenseTracker-Frontend
```

Install dependencies:

```bash
npm install
```

Start the React development server:

```bash
npm run dev
```

Open the local URL displayed in the terminal to access the application.

## Database

The application uses Microsoft SQL Server as its primary database. Entity Framework Core handles database operations, migrations, and relationships.

## Authentication & Security

- JWT-based authentication.
- Protected API endpoints using authorization.
- User-specific expense and budget data.
- Password hashing.
- Secure API communication through authenticated requests.

## Author

**Sampat Dantani**

GitHub: https://github.com/semdantani

LinkedIn: https://linkedin.com/in/sampat-dantani/
