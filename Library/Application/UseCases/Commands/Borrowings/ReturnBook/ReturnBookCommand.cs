using Application.DTOs;
using MediatR;

namespace Application.UseCases.Commands.Borrowings.ReturnBook;

public sealed record ReturnBookCommand(int BorrowingId) : IRequest<BorrowingDto>;
