using Application.DTOs;
using MediatR;

namespace Application.UseCases.Queries.Borrowers.GetCurrentlyBorrowedBooks;

public sealed record GetCurrentlyBorrowedBooksQuery(int BorrowerId)
    : IRequest<List<BookSummaryDto>>;
