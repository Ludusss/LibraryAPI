using FluentValidation;

namespace Application.Commands.Borrowings.BorrowBook;

public class BorrowBookCommandValidator : AbstractValidator<BorrowBookCommand>
{
    public BorrowBookCommandValidator()
    {
        RuleFor(x => x.BorrowerId).GreaterThan(0).WithMessage("Borrower ID must be greater than 0");

        RuleFor(x => x.BookId).GreaterThan(0).WithMessage("Book ID must be greater than 0");

        RuleFor(x => x.BorrowDurationDays)
            .GreaterThan(0)
            .WithMessage("Borrow duration must be greater than 0 days")
            .LessThanOrEqualTo(90)
            .WithMessage("Borrow duration cannot exceed 90 days");
    }
}
