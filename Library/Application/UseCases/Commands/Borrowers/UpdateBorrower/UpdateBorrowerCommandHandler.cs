using Application.DTOs;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Commands.Borrowers.UpdateBorrower;

public class UpdateBorrowerCommandHandler(IBorrowerRepository repository, IMapper mapper)
    : IRequestHandler<UpdateBorrowerCommand, BorrowerDto>
{
    public async Task<BorrowerDto> Handle(
        UpdateBorrowerCommand request,
        CancellationToken cancellationToken
    )
    {
        var borrower = await repository.GetBorrowerByIdAsync(request.Id);

        if (borrower == null)
            throw new KeyNotFoundException($"Borrower with ID {request.Id} not found");

        borrower.UpdateProfile(request.FirstName, request.LastName, request.PhoneNumber);

        borrower.UpdateBorrowLimit(request.MaxBorrowLimit);

        var updatedBorrower = await repository.UpdateBorrowerAsync(borrower);
        return mapper.Map<BorrowerDto>(updatedBorrower);
    }
}
