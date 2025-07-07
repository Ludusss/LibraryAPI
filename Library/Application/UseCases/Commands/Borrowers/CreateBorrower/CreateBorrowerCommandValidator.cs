using FluentValidation;

namespace Application.UseCases.Commands.Borrowers.CreateBorrower;

public class CreateBorrowerCommandValidator : AbstractValidator<CreateBorrowerCommand>
{
    public CreateBorrowerCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .MaximumLength(50)
            .WithMessage("First name must not exceed 50 characters");

        RuleFor(x => x.LastName)
            .MaximumLength(50)
            .WithMessage("Last name must not exceed 50 characters");

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Email must be a valid email address")
            .MaximumLength(100)
            .WithMessage("Email must not exceed 100 characters");

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
