using Application.DTOs;
using Domain.ValueObjects;
using MediatR;

namespace Application.UseCases.Commands.Borrowers.CreateBorrower;

public sealed record CreateBorrowerCommand(
    string FirstName,
    string LastName,
    Email Email,
    string PhoneNumber,
    int MaxBorrowLimit = 5
) : IRequest<BorrowerDto>;
