using Application.DTOs;
using MediatR;

namespace Application.UseCases.Commands.Borrowers.DeactivateBorrower;

public sealed record DeactivateBorrowerCommand(int Id) : IRequest<BorrowerDto>;
