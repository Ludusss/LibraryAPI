using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Commands.Borrowers.DeleteBorrower;

public class DeleteBorrowerCommandHandler(IBorrowerRepository repository)
    : IRequestHandler<DeleteBorrowerCommand, bool>
{
    public async Task<bool> Handle(
        DeleteBorrowerCommand request,
        CancellationToken cancellationToken
    )
    {
        return await repository.DeleteBorrowerAsync(request.Id);
    }
}
