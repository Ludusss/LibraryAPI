using Application.DTOs;
using MediatR;

namespace Application.UseCases.Queries.Borrowers.GetTopBorrowers;

public sealed record GetTopBorrowersQuery(
    int Count = 10,
    DateTime? StartDate = null,
    DateTime? EndDate = null
) : IRequest<List<TopBorrowerDto>>;
