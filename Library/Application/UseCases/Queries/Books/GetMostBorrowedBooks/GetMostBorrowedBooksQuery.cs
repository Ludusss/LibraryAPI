using Application.DTOs;
using MediatR;

namespace Application.UseCases.Queries.Books.GetMostBorrowedBooks;

public sealed record GetMostBorrowedBooksQuery(
    int Count = 10,
    DateTime? StartDate = null,
    DateTime? EndDate = null
) : IRequest<List<MostBorrowedBookDto>>;
