using MediatR;

namespace Application.UseCases.Commands.Borrowers.DeleteBorrower;

public sealed record DeleteBorrowerCommand(int Id) : IRequest<bool>;
