

**This is a first idea, that will update with the project's update**


# GroceryApp

A simple Grocery App: Design Guide and UI Documentation.


## Overview
GroceryApp is a Blazor-based web application designed to manage grocery items and users. The app provides basic authentication features, product listing, and database-backed data persistence using SQLite.



## Technology Stack

Framework: .NET 10.0 (Blazor / ASP.NET Core)

Language: C#

Database: SQLite

ORM: Entity Framework Core

Frontend: Razor Components + Bootstrap

Platform: Windows (local development)



## Project Structure

The project follows a clean, component-based structure:

Components/ – Razor components, layouts, and pages

Components/Pages/Auth/ – Authentication pages (Login, Profile)

Models/ – Domain models (User, GroceryItem, ShoppingCart, CartItem)

Data/ – EF Core DbContext and database initialization logic

Services/ – Business logic and helpers (e.g., GroceryService, PasswordHelper)

Migrations/ – Entity Framework Core migrations



## Database

Uses SQLite with Entity Framework Core

Database file: grocery.db

Automatically created and seeded on application startup

Initial data includes sample users and grocery products



## Build and Run Instructions

Make sure the .NET SDK is installed.
dotnet restore
dotnet build
dotnet run


## After running, the application starts locally at:
http://localhost:5215



## Packages Used

Microsoft.EntityFrameworkCore

Microsoft.EntityFrameworkCore.Design

Microsoft.EntityFrameworkCore.Sqlite



## Application Behavior

The application builds successfully with no errors

On startup, the database is verified and initialized

Existing users and products are detected.



## User Guide
1. **Sign Up / Login:** Create an account or login to access the app.  
2. **Browse Products:** Navigate the shop to view available groceries.  
3. **Add to Cart:** Click "Add" on items you want to purchase.  
4. **View Cart:** Review items in your cart and update quantities if needed.  
5. **Checkout:** Enter shipping information and complete payment.  
6. **Profile:** View past orders and account details.

## Admin Guide
1. **Login as Admin:** Use admin credentials to access the admin panel.  
2. **Manage Products:** Add, update, or delete grocery items.  
3. **View Orders:** See all user orders and their statuses.  
4. **Manage Users:** Optionally view and manage user accounts.  
5. **Dashboard:** Monitor app activity and inventory levels.


## Conclusion


