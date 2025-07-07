using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Borrowings.BorrowBook;

public class BorrowBookCommandHandler(IBorrowerRepository borrowerRepository, IBookRepository bookRepository, IBorrowingRepository borrowingRepository, IMapper mapper)
    : IRequestHandler<BorrowBookCommand, BorrowingDto>
{
    public async Task<BorrowingDto> Handle(
        BorrowBookCommand request,
        CancellationToken cancellationToken
    )
    {
        var borrower = await borrowerRepository.GetBorrowerByIdAsync(request.BorrowerId);
        if (borrower == null)
        {
            throw new KeyNotFoundException($"Borrower with ID {request.BorrowerId} not found");
        }

        var book = await bookRepository.GetBookByIdAsync(request.BookId);
        if (book == null)
        {
            throw new KeyNotFoundException($"Book with ID {request.BookId} not found");
        }

        if (!borrower.CanBorrow)
        {
            throw new InvalidOperationException("Borrower cannot borrow books. Either inactive or at borrow limit.");
        }

        if (!book.IsAvailable)
        {
            throw new InvalidOperationException("Book is not available for borrowing.");
        }

        var borrowing = new Borrowing(
            request.BorrowerId,
            request.BookId,
            DateTime.UtcNow,
            request.BorrowDurationDays
        );

        borrower.BorrowBook(book);

        var createdBorrowing = await borrowingRepository.CreateBorrowingAsync(borrowing);
        await borrowerRepository.UpdateBorrowerAsync(borrower);
        await bookRepository.UpdateBookAsync(book);

        return mapper.Map<BorrowingDto>(createdBorrowing);
    }
}
