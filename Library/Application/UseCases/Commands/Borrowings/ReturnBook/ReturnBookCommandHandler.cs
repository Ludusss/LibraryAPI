using Application.DTOs;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Commands.Borrowings.ReturnBook;

public class ReturnBookCommandHandler(
    IBorrowingRepository borrowingRepository,
    IBookRepository bookRepository,
    IMapper mapper
) : IRequestHandler<ReturnBookCommand, BorrowingDto>
{
    public async Task<BorrowingDto> Handle(
        ReturnBookCommand request,
        CancellationToken cancellationToken
    )
    {
        var borrowing = await borrowingRepository.GetBorrowingByIdAsync(request.BorrowingId);

        if (borrowing == null)
            throw new KeyNotFoundException($"Borrowing with ID {request.BorrowingId} not found");

        if (borrowing.IsReturned)
            throw new InvalidOperationException($"Book has already been returned");

        // Mark borrowing as returned using domain method
        borrowing.Return();

        // Update book availability using domain method
        var book = borrowing.Book;
        book.ReturnCopy();

        // Persist changes
        await borrowingRepository.UpdateBorrowingAsync(borrowing);
        await bookRepository.UpdateBookAsync(book);

        return mapper.Map<BorrowingDto>(borrowing);
    }
}
