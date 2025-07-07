using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Queries.Borrowers.GetBorrowerReadingRate;

public class GetBorrowerReadingRateQueryHandler(IBorrowerRepository repository)
    : IRequestHandler<GetBorrowerReadingRateQuery, double>
{
    public async Task<double> Handle(
        GetBorrowerReadingRateQuery request,
        CancellationToken cancellationToken
    )
    {
        var borrower = await repository.GetBorrowerByIdAsync(request.BorrowerId);

        if (borrower == null)
            throw new KeyNotFoundException($"Borrower with ID {request.BorrowerId} not found");

        return borrower.GetAverageReadingRate();
    }
}
