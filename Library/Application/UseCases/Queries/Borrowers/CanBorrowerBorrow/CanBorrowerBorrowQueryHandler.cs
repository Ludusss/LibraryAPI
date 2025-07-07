using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Queries.Borrowers.CanBorrowerBorrow;

public class CanBorrowerBorrowQueryHandler(IBorrowerRepository repository)
    : IRequestHandler<CanBorrowerBorrowQuery, bool>
{
    public async Task<bool> Handle(
        CanBorrowerBorrowQuery request,
        CancellationToken cancellationToken
    )
    {
        var borrower = await repository.GetBorrowerByIdAsync(request.BorrowerId);

        if (borrower == null)
            return false;

        return borrower.CanBorrow;
    }
}
