using Application.DTOs;
using MediatR;

namespace Application.UseCases.Queries.Books.GetAllBooks;

public sealed record GetAllBooksQuery : IRequest<List<BookDto>>;
