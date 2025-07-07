using Domain.Common;

namespace Domain.Exceptions;

public class BookNotAvailableException : DomainException
{
    public BookNotAvailableException(int bookId)
        : base($"Book with ID {bookId} is not available for borrowing.") { }

    public BookNotAvailableException(string bookTitle)
        : base($"Book '{bookTitle}' is not available for borrowing.") { }
}
