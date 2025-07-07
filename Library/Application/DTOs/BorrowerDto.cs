namespace Application.DTOs;

public class BorrowerDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime MembershipDate { get; set; }
    public bool IsActive { get; set; }
    public int MaxBorrowLimit { get; set; }
    public int CurrentBorrowedCount { get; set; }
    public bool CanBorrow { get; set; }
    public int TotalBooksRead { get; set; }
    public double AverageReadingRate { get; set; }
}

public class CreateBorrowerDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public int MaxBorrowLimit { get; set; } = 5;
}

public class UpdateBorrowerDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public int MaxBorrowLimit { get; set; }
}

public class BorrowerSummaryDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int CurrentBorrowedCount { get; set; }
    public bool IsActive { get; set; }
} 