using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Database;
using Infrastructure.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.Integration;

public class BorrowingRepositoryTests : IDisposable
{
    private readonly LibraryDbContext _context;
    private readonly BorrowingRepository _repository;

    public BorrowingRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(databaseName: "TestLibraryDb_" + Guid.NewGuid().ToString())
            .Options;

        _context = new LibraryDbContext(options);
        _repository = new BorrowingRepository(_context);
    }

    [Fact]
    public async Task CreateAndGetBorrowing_ShouldWork()
    {
        // Arrange
        var borrower = new Borrower("John", "Doe", Email.Create("john@example.com"), "1234567890");
        var book = new Book(
            "Test Book",
            "Test Author",
            Isbn.Create("978-0-306-40615-7"),
            200,
            DateTime.Now,
            1,
            "Test Genre",
            "Test Description"
        );

        _context.Borrowers.Add(borrower);
        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        var borrowing = new Borrowing(borrower.Id, book.Id, DateTime.UtcNow);

        // Act
        var createdBorrowing = await _repository.CreateBorrowingAsync(borrowing);
        var retrievedBorrowing = await _repository.GetBorrowingByIdAsync(createdBorrowing.Id);

        // Assert
        Assert.NotNull(retrievedBorrowing);
        Assert.Equal(borrowing.BorrowerId, retrievedBorrowing.BorrowerId);
        Assert.Equal(borrowing.BookId, retrievedBorrowing.BookId);
        Assert.Equal(borrowing.BorrowDate.Date, retrievedBorrowing.BorrowDate.Date);
    }

    [Fact]
    public async Task GetActiveBorrowings_ShouldReturnOnlyActiveBorrowings()
    {
        // Arrange
        var borrower = new Borrower(
            "Jane",
            "Smith",
            Email.Create("jane@example.com"),
            "1234567890"
        );
        var book1 = new Book(
            "Book 1",
            "Author 1",
            Isbn.Create("978-0-306-40615-7"),
            200,
            DateTime.Now,
            1,
            "Genre",
            "Description"
        );
        var book2 = new Book(
            "Book 2",
            "Author 2",
            Isbn.Create("978-0-306-40615-7"),
            300,
            DateTime.Now,
            1,
            "Genre",
            "Description"
        );

        _context.Borrowers.Add(borrower);
        _context.Books.AddRange(book1, book2);
        await _context.SaveChangesAsync();

        var activeBorrowing = new Borrowing(borrower.Id, book1.Id, DateTime.UtcNow);
        var returnedBorrowing = new Borrowing(borrower.Id, book2.Id, DateTime.UtcNow.AddDays(-10));
        returnedBorrowing.Return();

        await _repository.CreateBorrowingAsync(activeBorrowing);
        await _repository.CreateBorrowingAsync(returnedBorrowing);

        // Act
        var activeBorrowings = await _repository.GetActiveBorrowingsAsync();

        // Assert
        Assert.Single(activeBorrowings);
        Assert.Equal(book1.Id, activeBorrowings[0].BookId);
    }

    [Fact]
    public async Task GetOverdueBorrowings_ShouldReturnOnlyOverdueBorrowings()
    {
        // Arrange
        var borrower = new Borrower("Bob", "Wilson", Email.Create("bob@example.com"), "1234567890");
        var book1 = new Book(
            "Book 1",
            "Author 1",
            Isbn.Create("978-0-306-40615-7"),
            200,
            DateTime.Now,
            1,
            "Genre",
            "Description"
        );
        var book2 = new Book(
            "Book 2",
            "Author 2",
            Isbn.Create("978-0-306-40615-7"),
            300,
            DateTime.Now,
            1,
            "Genre",
            "Description"
        );

        _context.Borrowers.Add(borrower);
        _context.Books.AddRange(book1, book2);
        await _context.SaveChangesAsync();

        var overdueBorrowing = new Borrowing(borrower.Id, book1.Id, DateTime.UtcNow.AddDays(-20));
        var currentBorrowing = new Borrowing(borrower.Id, book2.Id, DateTime.UtcNow);

        await _repository.CreateBorrowingAsync(overdueBorrowing);
        await _repository.CreateBorrowingAsync(currentBorrowing);

        // Act
        var overdueBorrowings = await _repository.GetOverdueBorrowingsAsync();

        // Assert
        Assert.Single(overdueBorrowings);
        Assert.Equal(book1.Id, overdueBorrowings[0].BookId);
        Assert.True(overdueBorrowings[0].IsOverdue);
    }

    [Fact]
    public async Task GetBorrowingsByBorrower_ShouldReturnBorrowerBorrowings()
    {
        // Arrange
        var borrower1 = new Borrower(
            "Alice",
            "Johnson",
            Email.Create("alice@example.com"),
            "1234567890"
        );
        var borrower2 = new Borrower(
            "Charlie",
            "Brown",
            Email.Create("charlie@example.com"),
            "1234567890"
        );
        var book1 = new Book(
            "Book 1",
            "Author 1",
            Isbn.Create("978-0-306-40615-7"),
            200,
            DateTime.Now,
            1,
            "Genre",
            "Description"
        );
        var book2 = new Book(
            "Book 2",
            "Author 2",
            Isbn.Create("978-0-306-40615-7"),
            300,
            DateTime.Now,
            1,
            "Genre",
            "Description"
        );

        _context.Borrowers.AddRange(borrower1, borrower2);
        _context.Books.AddRange(book1, book2);
        await _context.SaveChangesAsync();

        var borrowing1 = new Borrowing(borrower1.Id, book1.Id, DateTime.UtcNow);
        var borrowing2 = new Borrowing(borrower1.Id, book2.Id, DateTime.UtcNow);
        var borrowing3 = new Borrowing(borrower2.Id, book1.Id, DateTime.UtcNow);

        await _repository.CreateBorrowingAsync(borrowing1);
        await _repository.CreateBorrowingAsync(borrowing2);
        await _repository.CreateBorrowingAsync(borrowing3);

        // Act
        var borrower1Borrowings = await _repository.GetBorrowingsByBorrowerAsync(borrower1.Id);

        // Assert
        Assert.Equal(2, borrower1Borrowings.Count);
        Assert.All(borrower1Borrowings, b => Assert.Equal(borrower1.Id, b.BorrowerId));
    }

    [Fact]
    public async Task UpdateBorrowing_ShouldUpdateBorrowing()
    {
        // Arrange
        var borrower = new Borrower(
            "David",
            "Miller",
            Email.Create("david@example.com"),
            "1234567890"
        );
        var book = new Book(
            "Test Book",
            "Test Author",
            Isbn.Create("978-0-306-40615-7"),
            200,
            DateTime.Now,
            1,
            "Test Genre",
            "Test Description"
        );

        _context.Borrowers.Add(borrower);
        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        var borrowing = new Borrowing(borrower.Id, book.Id, DateTime.UtcNow);
        var createdBorrowing = await _repository.CreateBorrowingAsync(borrowing);

        // Act
        createdBorrowing.Return();
        await _repository.UpdateBorrowingAsync(createdBorrowing);
        var updatedBorrowing = await _repository.GetBorrowingByIdAsync(createdBorrowing.Id);

        // Assert
        Assert.NotNull(updatedBorrowing);
        Assert.True(updatedBorrowing.IsReturned);
        Assert.NotNull(updatedBorrowing.ReturnDate);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
