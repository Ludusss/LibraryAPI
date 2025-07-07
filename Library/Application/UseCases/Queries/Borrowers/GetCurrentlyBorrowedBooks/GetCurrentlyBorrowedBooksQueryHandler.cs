using Application.DTOs;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Queries.Borrowers.GetCurrentlyBorrowedBooks;

public class GetCurrentlyBorrowedBooksQueryHandler(IBorrowingRepository repository, IMapper mapper)
    : IRequestHandler<GetCurrentlyBorrowedBooksQuery, List<BookSummaryDto>>
{
    public async Task<List<BookSummaryDto>> Handle(
        GetCurrentlyBorrowedBooksQuery request,
        CancellationToken cancellationToken
    )
    {
        var borrowings = await repository.GetBorrowingsByBorrowerAsync(request.BorrowerId);
        var activeBorrowings = borrowings.Where(b => !b.IsReturned).ToList();

        var books = activeBorrowings.Select(b => b.Book).ToList();
        return mapper.Map<List<BookSummaryDto>>(books);
    }
}
