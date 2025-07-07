using Application.DTOs;
using Application.UseCases.Queries.Books.GetMostBorrowedBooks;
using Domain.Interfaces;
using MediatR;

namespace Application.Queries.Books.GetMostBorrowedBooks;

public class GetMostBorrowedBooksQueryHandler(IBookRepository repository)
    : IRequestHandler<GetMostBorrowedBooksQuery, List<MostBorrowedBookDto>>
{
    public async Task<List<MostBorrowedBookDto>> Handle(
        GetMostBorrowedBooksQuery request,
        CancellationToken cancellationToken
    )
    {
        var mostBorrowedBooks = await repository.GetMostBorrowedBooksAsync(request.Count);

        return mostBorrowedBooks
            .Select(x => new MostBorrowedBookDto
            {
                BookId = x.Book.Id,
                Title = x.Book.Title,
                Author = x.Book.Author,
                ISBN = x.Book.Isbn.Value,
                BorrowCount = x.BorrowCount,
                AverageReadingRate = x.Book.CalculateAverageReadingRate(),
            })
            .ToList();
    }
}
