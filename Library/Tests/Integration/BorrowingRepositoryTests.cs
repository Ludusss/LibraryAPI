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
        var book = await CreateTestBook();
        var borrower = await CreateTestBorrower();

        // Act
        var borrowing = new Borrowing
        {
            BookId = book.Id,
            BorrowerId = borrower.Id,
            BorrowDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(14),
            IsReturned = false,
        };

        await _repository.CreateBorrowingAsync(borrowing);
        var result = await _repository.GetBorrowingByIdAsync(borrowing.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(book.Id, result.BookId);
        Assert.Equal(borrower.Id, result.BorrowerId);
        Assert.False(result.IsReturned);
    }

    [Fact]
    public async Task GetActiveBorrowings_ShouldReturnOnlyActive()
    {
        // Arrange
        var book = await CreateTestBook();
        var borrower = await CreateTestBorrower();

        var activeBorrowing = new Borrowing
        {
            BookId = book.Id,
            BorrowerId = borrower.Id,
            BorrowDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(14),
            IsReturned = false,
        };

        var returnedBorrowing = new Borrowing
        {
            BookId = book.Id,
            BorrowerId = borrower.Id,
            BorrowDate = DateTime.UtcNow.AddDays(-14),
            DueDate = DateTime.UtcNow.AddDays(-7),
            ReturnDate = DateTime.UtcNow,
            IsReturned = true,
        };

        await _repository.CreateBorrowingAsync(activeBorrowing);
        await _repository.CreateBorrowingAsync(returnedBorrowing);

        // Act
        var activeBorrowings = await _repository.GetActiveBorrowingsAsync();

        // Assert
        Assert.Single(activeBorrowings);
        Assert.Equal(activeBorrowing.Id, activeBorrowings[0].Id);
    }

    [Fact]
    public async Task GetOverdueBorrowings_ShouldReturnOnlyOverdue()
    {
        // Arrange
        var book = await CreateTestBook();
        var borrower = await CreateTestBorrower();

        var overdueBorrowing = new Borrowing
        {
            BookId = book.Id,
            BorrowerId = borrower.Id,
            BorrowDate = DateTime.UtcNow.AddDays(-14),
            DueDate = DateTime.UtcNow.AddDays(-1),
            IsReturned = false,
        };

        var activeBorrowing = new Borrowing
        {
            BookId = book.Id,
            BorrowerId = borrower.Id,
            BorrowDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(14),
            IsReturned = false,
        };

        await _repository.CreateBorrowingAsync(overdueBorrowing);
        await _repository.CreateBorrowingAsync(activeBorrowing);

        // Act
        var overdueBorrowings = await _repository.GetOverdueBorrowingsAsync();

        // Assert
        Assert.Single(overdueBorrowings);
        Assert.Equal(overdueBorrowing.Id, overdueBorrowings[0].Id);
    }

    private async Task<Book> CreateTestBook()
    {
        var book = new Book
        {
            Title = "Test Book",
            Author = "Test Author",
            Isbn = Isbn.Create("978-0-306-40615-7"),
            PageCount = 200,
            PublicationDate = DateTime.Now,
            TotalCopies = 1,
            AvailableCopies = 1,
            Genre = "Test Genre",
            Description = "Test Description",
        };

        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }

    private async Task<Borrower> CreateTestBorrower()
    {
        var borrower = new Borrower
        {
            FirstName = "Test",
            LastName = "Borrower",
            Email = Email.Create("test@example.com"),
            PhoneNumber = "1234567890",
            MembershipDate = DateTime.Now,
            IsActive = true,
            MaxBorrowLimit = 5,
        };

        _context.Borrowers.Add(borrower);
        await _context.SaveChangesAsync();
        return borrower;
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
