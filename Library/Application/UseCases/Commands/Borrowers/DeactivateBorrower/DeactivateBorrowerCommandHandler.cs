using Application.DTOs;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Commands.Borrowers.DeactivateBorrower;

public class DeactivateBorrowerCommandHandler(IBorrowerRepository repository, IMapper mapper)
    : IRequestHandler<DeactivateBorrowerCommand, BorrowerDto>
{
    public async Task<BorrowerDto> Handle(
        DeactivateBorrowerCommand request,
        CancellationToken cancellationToken
    )
    {
        var borrower = await repository.GetBorrowerByIdAsync(request.Id);

        if (borrower == null)
            throw new KeyNotFoundException($"Borrower with ID {request.Id} not found");

        borrower.Deactivate();

        var updatedBorrower = await repository.UpdateBorrowerAsync(borrower);
        return mapper.Map<BorrowerDto>(updatedBorrower);
    }
}
