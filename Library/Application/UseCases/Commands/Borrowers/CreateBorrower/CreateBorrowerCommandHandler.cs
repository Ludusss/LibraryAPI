using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using MediatR;

namespace Application.UseCases.Commands.Borrowers.CreateBorrower;

public class CreateBorrowerCommandHandler(IBorrowerRepository repository, IMapper mapper)
    : IRequestHandler<CreateBorrowerCommand, BorrowerDto>
{
    public async Task<BorrowerDto> Handle(
        CreateBorrowerCommand request,
        CancellationToken cancellationToken
    )
    {
        // Check if email already exists
        var emailExists = await repository.EmailExistsAsync(request.Email);
        if (emailExists)
            throw new InvalidOperationException(
                $"A borrower with email {request.Email} already exists"
            );

        var email = Email.Create(request.Email);

        var borrower = new Borrower(
            request.FirstName,
            request.LastName,
            email,
            request.PhoneNumber,
            request.MaxBorrowLimit
        );

        var createdBorrower = await repository.CreateBorrowerAsync(borrower);
        return mapper.Map<BorrowerDto>(createdBorrower);
    }
}
