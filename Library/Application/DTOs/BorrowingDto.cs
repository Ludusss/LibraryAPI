namespace Application.DTOs;

public class BorrowingDto
{
    public int Id { get; set; }
    public int BorrowerId { get; set; }
    public int BookId { get; set; }
    public DateTime BorrowDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsReturned { get; set; }
    public bool IsOverdue { get; set; }
    public int OverdueDays { get; set; }
    public decimal LateFee { get; set; }
    public int BorrowDurationDays { get; set; }
    public double ReadingRate { get; set; }
    
    // Navigation properties
    public BorrowerSummaryDto Borrower { get; set; } = new();
    public BookSummaryDto Book { get; set; } = new();
}

public class CreateBorrowingDto
{
    public int BorrowerId { get; set; }
    public int BookId { get; set; }
    public int BorrowDurationDays { get; set; } = 14;
}

public class BorrowingHistoryDto
{
    public int Id { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public DateTime BorrowDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsReturned { get; set; }
    public bool IsOverdue { get; set; }
    public int BorrowDurationDays { get; set; }
    public double ReadingRate { get; set; }
}

public class MostBorrowedBookDto
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int BorrowCount { get; set; }
    public double AverageReadingRate { get; set; }
}

public class TopBorrowerDto
{
    public int BorrowerId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int BorrowCount { get; set; }
    public int TotalBooksRead { get; set; }
    public double AverageReadingRate { get; set; }
} 