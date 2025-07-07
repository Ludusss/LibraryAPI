using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories;

public class BorrowerRepository(LibraryDbContext context) : IBorrowerRepository
{
    // Borrower operations
    public async Task<Borrower?> GetBorrowerByIdAsync(int id)
    {
        return await context
           .Borrowers.Include(b => b.Borrowings)
           .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<List<Borrower>> GetAllBorrowersAsync()
    {
        return await context.Borrowers.Include(b => b.Borrowings).ToListAsync();
    }

    public async Task<List<Borrower>> GetActiveBorrowersAsync()
    {
        return await context.Borrowers.Where(b => b.IsActive).ToListAsync();
    }

    public async Task<Borrower> CreateBorrowerAsync(Borrower borrower)
    {
        context.Borrowers.Add(borrower);
        await context.SaveChangesAsync();
        return borrower;
    }

    public async Task<Borrower> UpdateBorrowerAsync(Borrower borrower)
    {
        context.Borrowers.Update(borrower);
        await context.SaveChangesAsync();
        return borrower;
    }

    public async Task<bool> DeleteBorrowerAsync(int id)
    {
        var borrower = await GetBorrowerByIdAsync(id);
        if (borrower == null) return false;

        context.Borrowers.Remove(borrower);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> BorrowerExistsAsync(int id)
    {
        return await context.Borrowers.AnyAsync(b => b.Id == id);
    }

    public async Task<bool> EmailExistsAsync(Email email)
    {
        return await context.Borrowers.AnyAsync(b => b.Email == email);
    }

    // Borrower analytics
    public async Task<List<(Borrower Borrower, int BorrowCount)>> GetTopBorrowersAsync(
        int count = 10,
        DateTime? startDate = null,
        DateTime? endDate = null
    )
    {
        var query = context.Borrowings.Include(b => b.Borrower).AsQueryable();

        if (startDate.HasValue) query = query.Where(b => b.BorrowDate >= startDate.Value);

        if (endDate.HasValue) query = query.Where(b => b.BorrowDate <= endDate.Value);

        var result = await query
           .GroupBy(b => b.Borrower)
           .Select(
                g => new
                {
                    Borrower = g.Key,
                    BorrowCount = g.Count()
                }
            )
           .OrderByDescending(x => x.BorrowCount)
           .Take(count)
           .ToListAsync();

        return result.Select(x => (x.Borrower, x.BorrowCount)).ToList();
    }

    public async Task<int> GetBorrowCountForBorrowerAsync(
        int borrowerId,
        DateTime? startDate = null,
        DateTime? endDate = null
    )
    {
        var query = context.Borrowings.Where(b => b.BorrowerId == borrowerId);

        if (startDate.HasValue) query = query.Where(b => b.BorrowDate >= startDate.Value);

        if (endDate.HasValue) query = query.Where(b => b.BorrowDate <= endDate.Value);

        return await query.CountAsync();
    }
}
