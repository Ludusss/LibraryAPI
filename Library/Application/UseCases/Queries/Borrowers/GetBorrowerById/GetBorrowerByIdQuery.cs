using Application.DTOs;
using MediatR;

namespace Application.UseCases.Queries.Borrowers.GetBorrowerById;

public sealed record GetBorrowerByIdQuery(int Id) : IRequest<BorrowerDto>;
