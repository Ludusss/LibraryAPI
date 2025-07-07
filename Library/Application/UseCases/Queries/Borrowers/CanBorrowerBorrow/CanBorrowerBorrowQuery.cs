using MediatR;

namespace Application.UseCases.Queries.Borrowers.CanBorrowerBorrow;

public sealed record CanBorrowerBorrowQuery(int BorrowerId) : IRequest<bool>;
