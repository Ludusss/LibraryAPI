using Application.DTOs;
using MediatR;

namespace Application.UseCases.Queries.Borrowers.GetActiveBorrowers;

public sealed record GetActiveBorrowersQuery : IRequest<List<BorrowerSummaryDto>>;
