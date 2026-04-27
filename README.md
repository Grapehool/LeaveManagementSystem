# Leave Management System

## 📌 Overview

This is a Leave Management System built using ASP.NET Core with Clean Architecture.
It includes:

* Web API for backend operations
* Console UI client consuming the API via HTTP
* EF Core with SQL Server database

---

## 🏗️ Architecture

* **Core** → Domain + Application logic
* **Infrastructure** → EF Core + Database implementation
* **Presentation**

  * Web API (backend service)
  * Console UI (client application)

---

## ⚙️ Technologies Used

* .NET (ASP.NET Core Web API)
* Entity Framework Core
* Microsoft SQL Server
* Clean Architecture
* HTTP Client (for Console UI)

---

## 🗄️ Database Setup

### Option 1: Restore Backup

1. Open SQL Server Management Studio (SSMS)
2. Right-click on **Databases → Restore Database**
3. Select the file:

   ```
   /database/LeaveManagement.bak
   ```
4. Restore and confirm

---

### Option 2: Using EF Core Migrations

Run the following command:

```
dotnet ef database update
```

---

## 🚀 How to Run the Project

### 1. Clone the repository

```
git clone https://github.com/Grapehool/LeaveManagementSystem.git
```

### 2. Set up database

Restore the `.bak` file or run migrations.

---

### 3. Configure connection string

Update `appsettings.json` in API project:

```json
"ConnectionStrings": {
  "Default": "YOUR_CONNECTION_STRING_HERE"
}
```

---

### 4. Run Web API

```
dotnet run --project src/Presentation/Api
```

---

### 5. Run Console UI

```
dotnet run --project src/Presentation/ConsoleUI
```

---

## 📡 Communication Flow

Console UI communicates with Web API using HTTP requests.

---
