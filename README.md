# Book Api Project

This project is a **RESTful Web API** built with **ASP.NET Core** for managing books, authors, and user authentication.\
It includes **JWT authentication**, **role-based authorization**, **input validation**, **exception handling**, and **Swagger API documentation**.

## 🚀 Features

- **User Authentication**

  - JWT-based login
  - Registration
  - Delete account by username or email
  - Role-based access (`Admin`, `User`)

- **Book Management**

  - Create, read, update, delete books
  - Link books to authors

- **Author Management**

  - Create, read, update, delete authors
  - Only admins can delete authors

- **Validation**

  - Input validation with **FluentValidation**
  - Age restrictions for authors
  - Correct email & password validation

- **Error Handling**

  - Custom exception classes for different error cases
  - Exception filters for controllers
  - Global error handling middleware

- **Documentation**

  - Integrated **Swagger UI**
  - XML comments for API endpoints

## 🛠 Tech Stack

- **ASP.NET Core 8.0**
- **JWT Authentication** (`Microsoft.AspNetCore.Authentication.JwtBearer`)
- **FluentValidation**
- **AutoMapper**
- **Swagger**
- **BCrypt.Net** (password hashing)
- **C# 12**

## 📦 Installation & Run

1. **Clone the repository**

   ```bash
   git clone https://github.com/NataliiaSolodarenko/BookApiProject
   cd BookApiProject
   ```

2. **Install dependencies**

   ```bash
   dotnet restore
   ```

3. **Run the project**

   ```bash
   dotnet run
   ```

4. **Open Swagger API documentation**\
   Navigate to:

   ```
   https://localhost:5062/swagger
   ```


## 🔑 Default Admin Account

When the API starts, it has one default admin user:

| Field    | Value             |
| -------- | ----------------- |
| Username | `admin`           |
| Email    | `admin@gmail.com` |
| Password | `admin123`        |
| Role     | `Admin`           |

## 📂 Project Structure

```
📦 BookApi
 ┣ 📂 Controllers                              # API Controllers
 ┣ 📂 DTOs                                     # Data Transfer Objects
 ┣ 📂 Exceptions                               # Custom exception classes
 ┣ 📂 Filters                                  # Exception and Validation filters
 ┣ 📂 Models                                   # Project Models
 ┣ 📂 Services                                 # Business logic
 ┣ 📂 Storage                                  # Temporary database replacement
 ┣ 📂 Validators                               # FluentValidation rules
 ┣ 📜 ExceptionHandlingMiddleware.cs           # Global exception handler
 ┣ 📜 JwtSettings.cs                           # Configuration settings for JWT auth
 ┣ 📜 MappingProfile.cs                        # AutoMapper profile
 ┣ 📜 Program.cs                               # App configuration
```

## 📖 API Documentation

Once the project is running, visit **Swagger UI** at:

```
https://localhost:5062/swagger
```

You will find:

- **Auth endpoints** (login, register, delete account)
- **Book endpoints** (CRUD operations)
- **Author endpoints** (CRUD operations)

## 📝 Author

Created by [Nataliia Solodarenko](https://www.linkedin.com/in/nataliia-solodarenko-5272b0305/). Contributions and suggestions are welcome!

