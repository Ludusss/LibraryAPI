using Application.DTOs;
using MediatR;

namespace Application.UseCases.Queries.Borrowers.GetAllBorrowers;

public sealed record GetAllBorrowersQuery : IRequest<List<BorrowerDto>>;
