using Domain.Common;
using Domain.Events;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Borrower : BaseEntity
{
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public string PhoneNumber { get; private set; } = null!;
    public DateTime MembershipDate { get; private set; }
    public bool IsActive { get; private set; }
    public int MaxBorrowLimit { get; private set; }

    private readonly List<Borrowing> _borrowings = new();
    public IReadOnlyCollection<Borrowing> Borrowings => _borrowings.AsReadOnly();

    private Borrower() { } // For EF Core

    public Borrower(
        string firstName,
        string lastName,
        Email email,
        string phoneNumber,
        int maxBorrowLimit = 5
    )
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be null or empty.", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be null or empty.", nameof(lastName));

        if (maxBorrowLimit <= 0)
            throw new ArgumentException(
                "Max borrow limit must be positive.",
                nameof(maxBorrowLimit)
            );

        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        MembershipDate = DateTime.UtcNow;
        IsActive = true;
        MaxBorrowLimit = maxBorrowLimit;
    }

    public string FullName => $"{FirstName} {LastName}";

    public int CurrentBorrowedCount => _borrowings.Count(b => !b.IsReturned);

    public bool CanBorrow => IsActive && CurrentBorrowedCount < MaxBorrowLimit;

    public void BorrowBook(Book book)
    {
        if (!CanBorrow)
            throw new InvalidOperationException(
                "Cannot borrow book. Either inactive or at borrow limit."
            );

        if (!book.IsAvailable)
            throw new InvalidOperationException("Book is not available for borrowing.");

        var borrowing = new Borrowing(Id, book.Id, DateTime.UtcNow);
        _borrowings.Add(borrowing);

        book.BorrowCopy();

        AddDomainEvent(new BookBorrowedEvent(book.Id, book.Title, book.Author));
    }

    public void ReturnBook(Book book)
    {
        var borrowing = _borrowings.FirstOrDefault(b => b.BookId == book.Id && !b.IsReturned);

        if (borrowing == null)
            throw new InvalidOperationException(
                "This book is not currently borrowed by this user."
            );

        borrowing.Return();
        book.ReturnCopy();

        AddDomainEvent(new BookReturnedEvent(book.Id, book.Title, book.Author));
    }

    public void UpdateProfile(string? firstName, string? lastName, string? phoneNumber)
    {
        if (!string.IsNullOrWhiteSpace(firstName))
            FirstName = firstName;
        if (!string.IsNullOrWhiteSpace(lastName))
            LastName = lastName;
        if (!string.IsNullOrWhiteSpace(phoneNumber))
            PhoneNumber = phoneNumber;
    }

    public void UpdateBorrowLimit(int newLimit)
    {
        if (newLimit <= 0)
            throw new ArgumentException("Borrow limit must be positive.", nameof(newLimit));

        MaxBorrowLimit = newLimit;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public List<Borrowing> GetBorrowingHistory(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _borrowings.AsQueryable();

        if (startDate.HasValue)
            query = query.Where(b => b.BorrowDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(b => b.BorrowDate <= endDate.Value);

        return query.ToList();
    }

    public int GetTotalBooksRead()
    {
        return _borrowings.Count(b => b.IsReturned);
    }

    public double GetAverageReadingRate()
    {
        var completedBorrowings = _borrowings.Where(b => b.IsReturned).ToList();

        if (!completedBorrowings.Any())
            return 0;

        var rates = completedBorrowings
            .Select(b => b.CalculateReadingRate(0)) // PageCount will be provided separately
            .Where(rate => rate > 0)
            .ToList();

        return rates.Any() ? rates.Average() : 0;
    }
}
