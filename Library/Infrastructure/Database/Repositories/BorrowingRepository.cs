using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories;

public class BorrowingRepository(LibraryDbContext context) : IBorrowingRepository
{
    // Borrowing operations
    public async Task<Borrowing?> GetBorrowingByIdAsync(int id)
    {
        return await context
           .Borrowings.Include(b => b.Book)
           .Include(b => b.Borrower)
           .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<List<Borrowing>> GetAllBorrowingsAsync()
    {
        return await context
           .Borrowings.Include(b => b.Book)
           .Include(b => b.Borrower)
           .ToListAsync();
    }

    public async Task<List<Borrowing>> GetActiveBorrowingsAsync()
    {
        return await context
           .Borrowings.Include(b => b.Book)
           .Include(b => b.Borrower)
           .Where(b => !b.IsReturned)
           .ToListAsync();
    }

    public async Task<List<Borrowing>> GetOverdueBorrowingsAsync()
    {
        return await context
           .Borrowings.Include(b => b.Book)
           .Include(b => b.Borrower)
           .Where(b => !b.IsReturned && b.DueDate < DateTime.UtcNow)
           .ToListAsync();
    }

    public async Task<List<Borrowing>> GetBorrowingsByBorrowerAsync(int borrowerId)
    {
        return await context
           .Borrowings.Include(b => b.Book)
           .Include(b => b.Borrower)
           .Where(b => b.BorrowerId == borrowerId)
           .ToListAsync();
    }

    public async Task<List<Borrowing>> GetBorrowingsByBookAsync(int bookId)
    {
        return await context
           .Borrowings.Include(b => b.Book)
           .Include(b => b.Borrower)
           .Where(b => b.BookId == bookId)
           .ToListAsync();
    }

    public async Task<List<Borrowing>> GetBorrowingHistoryAsync(
        DateTime? startDate = null,
        DateTime? endDate = null
    )
    {
        var query = context.Borrowings.Include(b => b.Book).Include(b => b.Borrower).AsQueryable();

        if (startDate.HasValue) query = query.Where(b => b.BorrowDate >= startDate.Value);

        if (endDate.HasValue) query = query.Where(b => b.BorrowDate <= endDate.Value);

        return await query.ToListAsync();
    }

    public async Task<List<Borrowing>> GetBorrowerHistoryAsync(
        int borrowerId,
        DateTime? startDate = null,
        DateTime? endDate = null
    )
    {
        var query = context
           .Borrowings.Include(b => b.Book)
           .Include(b => b.Borrower)
           .Where(b => b.BorrowerId == borrowerId);

        if (startDate.HasValue) query = query.Where(b => b.BorrowDate >= startDate.Value);

        if (endDate.HasValue) query = query.Where(b => b.BorrowDate <= endDate.Value);

        return await query.ToListAsync();
    }

    public async Task<Borrowing> CreateBorrowingAsync(Borrowing borrowing)
    {
        context.Borrowings.Add(borrowing);
        await context.SaveChangesAsync();
        return borrowing;
    }

    public async Task<Borrowing> UpdateBorrowingAsync(Borrowing borrowing)
    {
        context.Borrowings.Update(borrowing);
        await context.SaveChangesAsync();
        return borrowing;
    }

    public async Task<bool> DeleteBorrowingAsync(int id)
    {
        var borrowing = await GetBorrowingByIdAsync(id);
        if (borrowing == null) return false;

        context.Borrowings.Remove(borrowing);
        await context.SaveChangesAsync();
        return true;
    }
}
