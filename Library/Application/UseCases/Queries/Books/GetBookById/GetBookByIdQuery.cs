using Application.DTOs;
using MediatR;

namespace Application.UseCases.Queries.Books.GetBookById;

public sealed record GetBookByIdQuery(int Id) : IRequest<BookDto>
{
    public int Id { get; set; } = Id;
}
