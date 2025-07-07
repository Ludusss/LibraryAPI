using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Commands;

public record BorrowBookCommand(int BookId, int BorrowerId, DateTime DueDate) : IRequest<int>;

public class BorrowBookCommandHandler(
    IBookRepository bookRepository,
    IBorrowerRepository borrowerRepository,
    IBorrowingRepository borrowingRepository
) : IRequestHandler<BorrowBookCommand, int>
{
    public async Task<int> Handle(BorrowBookCommand request, CancellationToken cancellationToken)
    {
        var book = await bookRepository.GetBookByIdAsync(request.BookId);
        if (book == null)
            throw new KeyNotFoundException($"Book with ID {request.BookId} not found");

        var borrower = await borrowerRepository.GetBorrowerByIdAsync(request.BorrowerId);
        if (borrower == null)
            throw new KeyNotFoundException($"Borrower with ID {request.BorrowerId} not found");

        if (book.AvailableCopies <= 0)
            throw new BookNotAvailableException(
                $"Book '{book.Title}' is not available for borrowing"
            );

        // Check if borrower has reached their limit
        var activeBorrowings = await borrowingRepository.GetBorrowingsByBorrowerAsync(
            request.BorrowerId
        );
        var activeBorrowingCount = activeBorrowings.Count(b => !b.IsReturned);
        const int maxBorrowLimit = 5; // Assuming max 5 books per borrower

        if (activeBorrowingCount >= maxBorrowLimit)
            throw new BorrowLimitExceededException(
                request.BorrowerId,
                maxBorrowLimit,
                activeBorrowingCount
            );

        // Calculate borrow duration in days
        var borrowDurationDays = (int)(request.DueDate - DateTime.UtcNow).TotalDays;
        if (borrowDurationDays <= 0)
            borrowDurationDays = 14; // Default to 14 days if due date is in the past

        var borrowing = new Borrowing(
            request.BorrowerId,
            request.BookId,
            DateTime.UtcNow,
            borrowDurationDays
        );

        // Update book availability using domain method
        book.BorrowCopy();
        await bookRepository.UpdateBookAsync(book);

        var createdBorrowing = await borrowingRepository.CreateBorrowingAsync(borrowing);
        return createdBorrowing.Id;
    }
}
