using MediatR;

namespace Application.UseCases.Queries.Borrowers.GetBorrowerReadingRate;

public sealed record GetBorrowerReadingRateQuery(int BorrowerId) : IRequest<double>;
