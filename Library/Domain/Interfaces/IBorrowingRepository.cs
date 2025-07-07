using Domain.Entities;

namespace Domain.Interfaces;

public interface IBorrowingRepository
{
    // Borrowing operations
    Task<Borrowing?> GetBorrowingByIdAsync(int id);
    Task<List<Borrowing>> GetAllBorrowingsAsync();
    Task<List<Borrowing>> GetActiveBorrowingsAsync();
    Task<List<Borrowing>> GetOverdueBorrowingsAsync();
    Task<List<Borrowing>> GetBorrowingsByBorrowerAsync(int borrowerId);
    Task<List<Borrowing>> GetBorrowingsByBookAsync(int bookId);
    Task<List<Borrowing>> GetBorrowingHistoryAsync(
        DateTime? startDate = null,
        DateTime? endDate = null
    );
    Task<List<Borrowing>> GetBorrowerHistoryAsync(
        int borrowerId,
        DateTime? startDate = null,
        DateTime? endDate = null
    );
    Task<Borrowing> CreateBorrowingAsync(Borrowing borrowing);
    Task<Borrowing> UpdateBorrowingAsync(Borrowing borrowing);
    Task<bool> DeleteBorrowingAsync(int id);
}
