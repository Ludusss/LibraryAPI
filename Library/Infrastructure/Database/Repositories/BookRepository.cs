using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories;

public class BookRepository(LibraryDbContext context) : IBookRepository
{
    // Book operations
    public async Task<Book?> GetBookByIdAsync(int id)
    {
        return await context.Books.Include(b => b.Borrowings).FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<List<Book>> GetAllBooksAsync()
    {
        return await context.Books.Include(b => b.Borrowings).ToListAsync();
    }

    public async Task<List<Book>> GetBooksAvailableAsync()
    {
        return await context.Books.Where(b => b.AvailableCopies > 0).ToListAsync();
    }

    public async Task<List<Book>> SearchBooksAsync(string searchTerm)
    {
        return await context
            .Books.Where(b =>
                b.Title.Contains(searchTerm)
                || b.Author.Contains(searchTerm)
                || b.Description.Contains(searchTerm)
            )
            .ToListAsync();
    }

    public async Task<List<Book>> GetBooksByAuthorAsync(string author)
    {
        return await context.Books.Where(b => b.Author.Contains(author)).ToListAsync();
    }

    public async Task<List<Book>> GetBooksByGenreAsync(string genre)
    {
        return await context.Books.Where(b => b.Genre.Contains(genre)).ToListAsync();
    }

    public async Task<Book> CreateBookAsync(Book book)
    {
        context.Books.Add(book);
        await context.SaveChangesAsync();
        return book;
    }

    public async Task<Book> UpdateBookAsync(Book book)
    {
        context.Books.Update(book);
        await context.SaveChangesAsync();
        return book;
    }

    public async Task<bool> DeleteBookAsync(int id)
    {
        var book = await GetBookByIdAsync(id);
        if (book == null)
            return false;

        context.Books.Remove(book);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> BookExistsAsync(int id)
    {
        return await context.Books.AnyAsync(b => b.Id == id);
    }

    // Book analytics
    public async Task<List<(Book Book, int BorrowCount)>> GetMostBorrowedBooksAsync(int count = 10)
    {
        var result = await context
            .Borrowings.Include(b => b.Book)
            .GroupBy(b => b.Book)
            .Select(g => new { Book = g.Key, BorrowCount = g.Count() })
            .OrderByDescending(x => x.BorrowCount)
            .Take(count)
            .ToListAsync();

        return result.Select(x => (x.Book, x.BorrowCount)).ToList();
    }

    public async Task<List<Book>> GetBooksAlsoBorrowedByUsersWhoReadAsync(int bookId)
    {
        // Get users who borrowed the specified book
        var userIds = await context
            .Borrowings.Where(b => b.BookId == bookId)
            .Select(b => b.BorrowerId)
            .Distinct()
            .ToListAsync();

        // Get other books borrowed by these users
        var relatedBooks = await context
            .Borrowings.Include(b => b.Book)
            .Where(b => userIds.Contains(b.BorrowerId) && b.BookId != bookId)
            .Select(b => b.Book)
            .Distinct()
            .ToListAsync();

        return relatedBooks;
    }

    public async Task<double> CalculateBookReadingRateAsync(int bookId)
    {
        var book = await context
            .Books.Include(b => b.Borrowings)
            .FirstOrDefaultAsync(b => b.Id == bookId);

        return book?.CalculateAverageReadingRate() ?? 0;
    }

    public async Task<int> GetBorrowCountForBookAsync(int bookId)
    {
        return await context.Borrowings.CountAsync(b => b.BookId == bookId);
    }
}
