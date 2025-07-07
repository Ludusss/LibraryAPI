using FluentValidation;

namespace Application.Commands.Books.CreateBook;

public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required")
            .MaximumLength(200)
            .WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Author)
            .NotEmpty()
            .WithMessage("Author is required")
            .MaximumLength(100)
            .WithMessage("Author must not exceed 100 characters");

        RuleFor(x => x.Isbn)
            .NotEmpty()
            .WithMessage("ISBN is required")
            .Must(BeValidIsbn)
            .WithMessage("ISBN format is invalid");

        RuleFor(x => x.PageCount).GreaterThan(0).WithMessage("Page count must be greater than 0");

        RuleFor(x => x.PublicationDate)
            .LessThanOrEqualTo(DateTime.Today)
            .WithMessage("Publication date cannot be in the future");

        RuleFor(x => x.TotalCopies)
            .GreaterThan(0)
            .WithMessage("Total copies must be greater than 0");

        RuleFor(x => x.Genre)
            .NotEmpty()
            .WithMessage("Genre is required")
            .MaximumLength(50)
            .WithMessage("Genre must not exceed 50 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithMessage("Description must not exceed 1000 characters");
    }

    private bool BeValidIsbn(string isbn)
    {
        if (string.IsNullOrWhiteSpace(isbn))
            return false;

        // Remove any hyphens or spaces
        var cleanIsbn = isbn.Replace("-", "").Replace(" ", "");

        // Check for ISBN-10 or ISBN-13
        return cleanIsbn.Length == 10 || cleanIsbn.Length == 13;
    }
}
