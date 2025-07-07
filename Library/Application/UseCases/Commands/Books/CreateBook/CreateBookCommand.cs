using Application.DTOs;
using MediatR;

namespace Application.Commands.Books.CreateBook;

public sealed record CreateBookCommand(
    string Title,
    string Author,
    string Isbn,
    int PageCount,
    DateTime PublicationDate,
    int TotalCopies,
    string Genre,
    string Description
) : IRequest<BookDto>;
