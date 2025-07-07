using Application.DTOs;
using MediatR;

namespace Application.UseCases.Commands.Borrowers.UpdateBorrower;

public sealed record UpdateBorrowerCommand(
    int Id,
    string? FirstName,
    string? LastName,
    string? PhoneNumber,
    int MaxBorrowLimit
) : IRequest<BorrowerDto>;
