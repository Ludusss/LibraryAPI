using Domain.Common;
using Domain.Events;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Book : BaseEntity
{
    public string Title { get; private set; } = null!;
    public string Author { get; private set; } = null!;
    public Isbn Isbn { get; private set; } = null!;
    public int PageCount { get; private set; }
    public DateTime PublicationDate { get; private set; }
    public int TotalCopies { get; private set; }
    public int AvailableCopies { get; private set; }
    public string Genre { get; private set; } = null!;
    public string Description { get; private set; } = null!;

    private readonly List<Borrowing> _borrowings = new();
    public IReadOnlyCollection<Borrowing> Borrowings => _borrowings.AsReadOnly();

    public Book() { } // For EF Core

    public Book(
        string title,
        string author,
        Isbn isbn,
        int pageCount,
        DateTime publicationDate,
        int totalCopies,
        string genre,
        string description
    )
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title cannot be null or empty.", nameof(title));
        }

        if (string.IsNullOrWhiteSpace(author))
        {
            throw new ArgumentException("Author cannot be null or empty.", nameof(author));
        }

        if (pageCount <= 0)
        {
            throw new ArgumentException("Page count must be positive.", nameof(pageCount));
        }

        if (totalCopies <= 0)
        {
            throw new ArgumentException("Total copies must be positive.", nameof(totalCopies));
        }

        Title = title;
        Author = author;
        Isbn = isbn;
        PageCount = pageCount;
        PublicationDate = publicationDate;
        TotalCopies = totalCopies;
        AvailableCopies = totalCopies;
        Genre = genre;
        Description = description;
    }

    public bool IsAvailable => AvailableCopies > 0;

    public void BorrowCopy()
    {
        if (AvailableCopies <= 0)
        {
            throw new InvalidOperationException("No copies available for borrowing.");
        }

        AvailableCopies--;

        AddDomainEvent(new BookBorrowedEvent(Id, Title, Author));
    }

    public void ReturnCopy()
    {
        if (AvailableCopies >= TotalCopies)
        {
            throw new InvalidOperationException("Cannot return more copies than total.");
        }

        AvailableCopies++;

        AddDomainEvent(new BookReturnedEvent(Id, Title, Author));
    }

    public void UpdateCopies(int newTotalCopies)
    {
        if (newTotalCopies < 0)
        {
            throw new ArgumentException("Total copies cannot be negative.", nameof(newTotalCopies));
        }

        var difference = newTotalCopies - TotalCopies;
        TotalCopies = newTotalCopies;
        AvailableCopies = Math.Max(0, AvailableCopies + difference);
    }

    public void UpdateDetails(string title, string author, string genre, string description)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title cannot be null or empty.", nameof(title));
        }

        if (string.IsNullOrWhiteSpace(author))
        {
            throw new ArgumentException("Author cannot be null or empty.", nameof(author));
        }

        Title = title;
        Author = author;
        Genre = genre;
        Description = description;
    }

    public int GetBorrowedCopies()
    {
        return TotalCopies - AvailableCopies;
    }

    public double CalculateAverageReadingRate()
    {
        var completedBorrowings = _borrowings.Where(b => b.IsReturned).ToList();

        if (!completedBorrowings.Any())
        {
            return 0;
        }

        var totalReadingRates = completedBorrowings
            .Select(b => b.CalculateReadingRate(PageCount))
            .Where(rate => rate > 0)
            .ToList();

        return totalReadingRates.Any() ? totalReadingRates.Average() : 0;
    }
}
