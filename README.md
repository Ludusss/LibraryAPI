# Library API - .NET Developer Assignment

A comprehensive Library Management API built with **Domain-Driven Design (DDD)** using .NET 8, Entity Framework Core, and SQL Server.

## 🏗️ Architecture Overview

This project demonstrates a clean implementation of:
- **Domain-Driven Design (DDD)** with clear domain boundaries
- **Clean Architecture** principles with proper separation of concerns
- **CQRS** patterns with MediatR
- **Entity Framework Core** with SQL Server
- **Comprehensive testing** with xUnit and NSubstitute

## 📁 Project Structure

```
Library/
├── API/              # API Layer (Controllers, Configuration)
├── Application/      # Application Layer (Services, DTOs, Interfaces)
├── Domain/          # Domain Layer (Entities, Value Objects, Events)
├── Infrastructure/   # Infrastructure Layer (Data Access, Repositories)
├── Tests.Unit/      # Unit Tests
├── Tests.SystemTests/      # System Tests
└── Tests.Integration/ # Integration Tests

```

## 🚀 Features

### Core Library System ✅
- **Book Management** - CRUD operations with advanced search capabilities
- **Borrower Management** - User profiles with borrowing limits and history
- **Borrowing System** - Complete lending workflow with due dates and extensions
- **Analytics & Reporting** - Most borrowed books, top readers, reading rates

## 🛠️ Technology Stack

- **.NET 8** - Latest .NET framework
- **Entity Framework Core 8** - ORM for database operations
- **SQL Server** - Database for data persistence
- **AutoMapper** - Object-to-object mapping
- **MediatR** - Mediator pattern implementation
- **Serilog** - Structured logging
- **Swagger/OpenAPI** - API documentation
- **xUnit** - Unit testing framework
- **NSubstitute** - Mocking framework
- **FluentValidation** - Input validation

## 🔧 Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB or full instance)
- Visual Studio 2022 or VS Code

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd LibraryAPI
   ```

2. **Navigate to the Library directory**
   ```bash
   cd Library
   ```

3. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

4. **Update connection string**
   
   Edit `Library.API/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=LibraryDB;Trusted_Connection=true;MultipleActiveResultSets=true"
     }
   }
   ```

5. **Run the application**
   ```bash
   dotnet run --project Library.API
   ```

6. **Access the API**
   - Swagger UI: `https://localhost:5001` (or `http://localhost:5000`)
   - API Base URL: `https://localhost:5001/api`

## 🧪 Testing

### Run Unit Tests
```bash
dotnet test Library.Tests.Unit
```

### Run Integration Tests
```bash
dotnet test Library.Tests.Integration
```

### Run All Tests
```bash
dotnet test
```

## 📊 Sample Data

The application includes pre-seeded sample data:

### Books
- The Great Gatsby (F. Scott Fitzgerald)
- To Kill a Mockingbird (Harper Lee)
- 1984 (George Orwell)
- Pride and Prejudice (Jane Austen)
- The Catcher in the Rye (J.D. Salinger)

### Borrowers
- John Doe (john.doe@example.com)
- Jane Smith (jane.smith@example.com)
- Bob Johnson (bob.johnson@example.com)
- Alice Brown (alice.brown@example.com)
- Charlie Davis (charlie.davis@example.com)

## 🔍 Key Business Requirements Addressed

### ✅ Most Borrowed Books
- Endpoint: `GET /api/books/most-borrowed`
- Returns books ranked by borrowing frequency

### ✅ Book Availability Status
- Endpoint: `GET /api/books/{id}/availability`
- Shows available vs. borrowed copies

### ✅ Top Borrowers Analysis
- Endpoint: `GET /api/borrowers/top-borrowers`
- Identifies users who borrowed most books within timeframe

### ✅ User Borrowing History
- Endpoint: `GET /api/borrowers/{id}/history`
- Shows what books a user borrowed during specified period

### ✅ Reading Rate Calculation
- Endpoint: `GET /api/books/{bookId}/reading-rate`
- Estimates pages/day based on borrow/return times

## 🏛️ Domain-Driven Design Implementation

### Domain Layer
- **Entities**: Book, Borrower, Borrowing
- **Value Objects**: ISBN, Email
- **Domain Services**: WarmupTasks

### Application Layer
- **CQRS**: Command Query Responsibility segregation
- **DTOs**: Comprehensive data transfer objects

### Infrastructure Layer
- **Repositories**: BookRepository, BorrowerRepository, BorrowingRepository with EF Core
- **DbContext**: LibraryDbContext with proper configurations
- **Migrations**: Database schema management

## 🔒 Data Validation & Business Rules

- **ISBN Validation**: Supports both ISBN-10 and ISBN-13
- **Email Validation**: Proper email format validation
- **Borrowing Limits**: Configurable per-user borrowing limits
- **Due Date Management**: Automatic due date calculation
- **Overdue Tracking**: Late fee calculation and tracking

## 📝 Logging & Monitoring

- **Structured Logging**: Serilog with multiple sinks
- **Request Logging**: Comprehensive API request tracking
- **Error Handling**: Global exception handling with proper error responses

## 🎯 Performance Considerations

- **Entity Framework Optimizations**: Proper indexing and query optimization
- **Lazy Loading**: Controlled loading of related entities
- **Connection Pooling**: Efficient database connection management
- **Memory Management**: Proper disposal of resources

## 📚 Additional Resources

- [Domain-Driven Design by Eric Evans](https://domainlanguage.com/ddd/)
- [Clean Architecture by Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [.NET 8 Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)

## 🤝 Contributing

This project demonstrates senior-level .NET development practices including:
- Clean code principles
- SOLID design principles
- Comprehensive testing strategies
- Proper error handling and logging
- API documentation best practices

## 📄 License

This project is created for educational and demonstration purposes as part of a senior .NET developer assignment.
