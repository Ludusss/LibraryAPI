using FluentValidation;

namespace Application.UseCases.Commands.Borrowers.UpdateBorrower;

public class UpdateBorrowerCommandValidator : AbstractValidator<UpdateBorrowerCommand>
{
    public UpdateBorrowerCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Borrower ID must be greater than 0");

        RuleFor(x => x.FirstName)
            .MaximumLength(50)
            .WithMessage("First name must not exceed 50 characters");

        RuleFor(x => x.LastName)
            .MaximumLength(50)
            .WithMessage("Last name must not exceed 50 characters");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20)
            .WithMessage("Phone number must not exceed 20 characters");

        RuleFor(x => x.MaxBorrowLimit)
            .GreaterThan(0)
            .WithMessage("Max borrow limit must be greater than 0")
            .LessThanOrEqualTo(10)
            .WithMessage("Max borrow limit cannot exceed 10 books");
    }
}
