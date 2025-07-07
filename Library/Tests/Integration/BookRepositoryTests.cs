using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Database;
using Infrastructure.Database.Repositories;

namespace Tests.Integration;

public class BookRepositoryTests : IDisposable
{
    private readonly LibraryDbContext _context;
    private readonly BookRepository _repository;

    public BookRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(databaseName: "TestLibraryDb_" + Guid.NewGuid().ToString())
            .Options;

        _context = new LibraryDbContext(options);
        _repository = new BookRepository(_context);
    }

    [Fact]
    public async Task CreateAndGetBook_ShouldWork()
    {
        // Arrange
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

        // Act
        var createdBook = await _repository.CreateBookAsync(book);
        var retrievedBook = await _repository.GetBookByIdAsync(createdBook.Id);

        // Assert
        Assert.NotNull(retrievedBook);
        Assert.Equal(book.Title, retrievedBook.Title);
        Assert.Equal(book.Author, retrievedBook.Author);
        Assert.Equal(book.Isbn, retrievedBook.Isbn);
    }

    [Fact]
    public async Task GetBooksAvailable_ShouldReturnOnlyAvailableBooks()
    {
        // Arrange
        var availableBook = new Book(
            "Available Book",
            "Test Author",
            Isbn.Create("978-0-306-40615-7"),
            200,
            DateTime.Now,
            1,
            "Test Genre",
            "Test Description"
        );

        await _repository.CreateBookAsync(availableBook);

        // Act
        var availableBooks = await _repository.GetBooksAvailableAsync();

        // Assert
        Assert.Single(availableBooks);
        Assert.Equal("Available Book", availableBooks[0].Title);
    }

    [Fact]
    public async Task SearchBooks_ShouldFindByTitleAuthorAndDescription()
    {
        // Arrange
        var books = new[]
        {
            new Book(
                "The Great Adventure",
                "John Smith",
                Isbn.Create("978-0-306-40615-7"),
                200,
                DateTime.Now,
                1,
                "Adventure",
                "An exciting journey"
            ),
            new Book(
                "Programming Guide",
                "Jane Adventure",
                Isbn.Create("978-0-306-40615-7"),
                300,
                DateTime.Now,
                1,
                "Technology",
                "Learn programming"
            ),
            new Book(
                "Cooking Basics",
                "Chef Gordon",
                Isbn.Create("978-0-306-40615-7"),
                150,
                DateTime.Now,
                1,
                "Cooking",
                "Start your adventure in cooking"
            ),
        };

        foreach (var book in books)
        {
            await _repository.CreateBookAsync(book);
        }

        // Act & Assert
        var titleResults = await _repository.SearchBooksAsync("Great");
        Assert.Single(titleResults);
        Assert.Equal("The Great Adventure", titleResults[0].Title);

        var authorResults = await _repository.SearchBooksAsync("Adventure");
        Assert.Equal(2, authorResults.Count); // Matches author name and description

        var descriptionResults = await _repository.SearchBooksAsync("programming");
        Assert.Single(descriptionResults);
        Assert.Equal("Programming Guide", descriptionResults[0].Title);
    }

    [Fact]
    public async Task UpdateBook_ShouldUpdateAllFields()
    {
        // Arrange
        var book = new Book(
            "Original Title",
            "Original Author",
            Isbn.Create("978-0-306-40615-7"),
            200,
            DateTime.Now,
            1,
            "Original Genre",
            "Original Description"
        );

        var createdBook = await _repository.CreateBookAsync(book);

        // Act
        createdBook.UpdateDetails(
            "Updated Title",
            "Updated Author",
            "Updated Genre",
            "Updated Description"
        );

        await _repository.UpdateBookAsync(createdBook);
        var updatedBook = await _repository.GetBookByIdAsync(createdBook.Id);

        // Assert
        Assert.NotNull(updatedBook);
        Assert.Equal("Updated Title", updatedBook.Title);
        Assert.Equal("Updated Author", updatedBook.Author);
        Assert.Equal("Updated Genre", updatedBook.Genre);
        Assert.Equal("Updated Description", updatedBook.Description);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
