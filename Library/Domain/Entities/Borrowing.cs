using Domain.Common;

namespace Domain.Entities;

public class Borrowing : BaseEntity
{
    public int BorrowerId { get; private set; }
    public int BookId { get; private set; }
    public DateTime BorrowDate { get; private set; }
    public DateTime? ReturnDate { get; private set; }
    public DateTime DueDate { get; private set; }
    public bool IsReturned { get; private set; }
    public bool IsOverdue => !IsReturned && DateTime.UtcNow > DueDate;

    // Navigation properties
    public Borrower Borrower { get; private set; } = null!;
    public Book Book { get; private set; } = null!;

    private Borrowing() { } // For EF Core

    public Borrowing(int borrowerId, int bookId, DateTime borrowDate, int borrowDurationDays = 14)
    {
        if (borrowerId <= 0)
        {
            throw new ArgumentException("Borrower ID must be positive.", nameof(borrowerId));
        }

        if (bookId <= 0)
        {
            throw new ArgumentException("Book ID must be positive.", nameof(bookId));
        }

        BorrowerId = borrowerId;
        BookId = bookId;
        BorrowDate = borrowDate;
        DueDate = borrowDate.AddDays(borrowDurationDays);
        IsReturned = false;
    }

    public void Return()
    {
        if (IsReturned)
        {
            throw new InvalidOperationException("Book is already returned.");
        }

        ReturnDate = DateTime.UtcNow;
        IsReturned = true;
    }

    public void ExtendDueDate(int additionalDays)
    {
        if (IsReturned)
        {
            throw new InvalidOperationException("Cannot extend due date for returned book.");
        }

        if (additionalDays <= 0)
        {
            throw new ArgumentException(
                "Additional days must be positive.",
                nameof(additionalDays)
            );
        }

        DueDate = DueDate.AddDays(additionalDays);
    }

    private int GetBorrowDurationDays()
    {
        var endDate = ReturnDate ?? DateTime.UtcNow;
        return (int)(endDate - BorrowDate).TotalDays;
    }

    public double CalculateReadingRate(int pageCount)
    {
        if (pageCount <= 0 || !IsReturned)
        {
            return 0;
        }

        var readingDays = GetBorrowDurationDays();
        return readingDays > 0 ? (double)pageCount / readingDays : 0;
    }

    private int GetOverdueDays()
    {
        if (!IsOverdue)
        {
            return 0;
        }

        return (int)(DateTime.UtcNow - DueDate).TotalDays;
    }

    public decimal CalculateLateFee(decimal feePerDay = 1.0m)
    {
        if (!IsOverdue)
        {
            return 0;
        }

        return GetOverdueDays() * feePerDay;
    }
}
