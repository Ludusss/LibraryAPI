using Application.DTOs;
using MediatR;

namespace Application.UseCases.Commands.Borrowers.CreateBorrower;

public sealed record CreateBorrowerCommand(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    int MaxBorrowLimit = 5
) : IRequest<BorrowerDto>;
