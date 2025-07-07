using Application.DTOs;
using MediatR;

namespace Application.UseCases.Queries.Borrowers.GetBorrowerHistory;

public sealed record GetBorrowerHistoryQuery(
    int BorrowerId,
    DateTime? StartDate = null,
    DateTime? EndDate = null
) : IRequest<List<BorrowingHistoryDto>>;
