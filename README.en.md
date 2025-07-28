# projekt – Warehouse Application

This project is a web application for managing and tracking materials in the projekt company. It allows technicians to withdraw materials, administrators to manage stock, and superadmins to oversee multiple departments (e.g. Plzeň, Prague, Beroun).

## Features

- User authentication and role management (Technician, Admin, SuperAdmin)
- Dashboard with clear statistics (withdrawals, top technicians, average usage)
- CRUD operations for:
  - Materials
  - Material types
  - Users
  - Withdrawn materials
- Critical stock monitoring
- Monthly withdrawal limits enforced
- Responsive UI using Bootstrap (Bootswatch Slate)
- Time-period filtering (week, month, quarter, year)

## Technologies Used

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core (SQL Server)
- Identity for user management
- AutoMapper
- Chart.js
- Bootstrap (Bootswatch)

## User Roles

- **Technician** – can withdraw material and view only their stock
- **Admin** – manages users, types, and materials for their department
- **SuperAdmin** – full access across all departments

## Project Structure

- `/Controllers` – handles requests
- `/Models` – EF Core entities
- `/ViewModels` – view data
- `/Services` – business logic
- `/Repositories` – database access
- `/Views` – Razor pages
- `/wwwroot/js` – JavaScript logic for graphs (Chart.js)

## Test Credentials

| Role         | Email              | Password  |
|--------------|--------------------|-----------|
| SuperAdmin   | admin@admin.cz     | 12345678  |
| Technician   | technik@projekt.cz | 12345678  |

## How to Run

1. Open in **Visual Studio 2022+**
2. Make sure **SQL Server LocalDB** is installed
3. Run migrations or use provided init script
4. Start the application (`F5`)

## Dashboard Highlights

- Top 5 technicians by material withdrawals
- Top 5 materials
- Average consumption by selected period
- Visualized with Chart.js

## Author

Created as part of ASP.NET Core studies – Šimon Durák (2025)
