using Application.DTOs;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Queries.Books.GetAllBooks;

public class GetAllBooksQueryHandler(IBookRepository repository, IMapper mapper) : IRequestHandler<GetAllBooksQuery, List<BookDto>>
{
    public async Task<List<BookDto>> Handle(
        GetAllBooksQuery request,
        CancellationToken cancellationToken
    )
    {
        var books = await repository.GetAllBooksAsync();
        return mapper.Map<List<BookDto>>(books);
    }
}
