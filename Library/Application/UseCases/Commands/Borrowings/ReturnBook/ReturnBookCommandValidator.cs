using FluentValidation;

namespace Application.UseCases.Commands.Borrowings.ReturnBook;

public class ReturnBookCommandValidator : AbstractValidator<ReturnBookCommand>
{
    public ReturnBookCommandValidator()
    {
        RuleFor(x => x.BorrowingId)
            .GreaterThan(0)
            .WithMessage("Borrowing ID must be greater than 0");
    }
}
