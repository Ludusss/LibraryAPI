using Domain.Common;

namespace Domain.Events;

public class BookBorrowedEvent(int bookId, string bookTitle, string author) : IDomainEvent
{
    public int BookId { get; } = bookId;
    public string BookTitle { get; } = bookTitle;
    public string Author { get; } = author;
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
