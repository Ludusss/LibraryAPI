using Domain.Entities;

namespace Domain.Interfaces;

public interface IBookRepository
{
    // Book operations
    Task<Book?> GetBookByIdAsync(int id);
    Task<List<Book>> GetAllBooksAsync();
    Task<List<Book>> GetBooksAvailableAsync();
    Task<List<Book>> SearchBooksAsync(string searchTerm);
    Task<List<Book>> GetBooksByAuthorAsync(string author);
    Task<List<Book>> GetBooksByGenreAsync(string genre);
    Task<Book> CreateBookAsync(Book book);
    Task<Book> UpdateBookAsync(Book book);
    Task<bool> DeleteBookAsync(int id);
    Task<bool> BookExistsAsync(int id);

    // Book analytics
    Task<List<(Book Book, int BorrowCount)>> GetMostBorrowedBooksAsync(int count = 10);
    Task<List<Book>> GetBooksAlsoBorrowedByUsersWhoReadAsync(int bookId);
    Task<double> CalculateBookReadingRateAsync(int bookId);
    Task<int> GetBorrowCountForBookAsync(int bookId);
}
