using Domain.Entities;

namespace Domain.Interfaces;

public interface IBorrowerRepository
{
    // Borrower operations
    Task<Borrower?> GetBorrowerByIdAsync(int id);
    Task<List<Borrower>> GetAllBorrowersAsync();
    Task<List<Borrower>> GetActiveBorrowersAsync();
    Task<Borrower> CreateBorrowerAsync(Borrower borrower);
    Task<Borrower> UpdateBorrowerAsync(Borrower borrower);
    Task<bool> DeleteBorrowerAsync(int id);
    Task<bool> BorrowerExistsAsync(int id);
    Task<bool> EmailExistsAsync(string email);

    // Borrower analytics
    Task<List<(Borrower Borrower, int BorrowCount)>> GetTopBorrowersAsync(
        int count = 10,
        DateTime? startDate = null,
        DateTime? endDate = null
    );
    Task<int> GetBorrowCountForBorrowerAsync(
        int borrowerId,
        DateTime? startDate = null,
        DateTime? endDate = null
    );
}
