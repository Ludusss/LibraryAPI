using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using MediatR;

namespace Application.Commands.Books.CreateBook;

public class CreateBookCommandHandler(IBookRepository repository, IMapper mapper)
    : IRequestHandler<CreateBookCommand, BookDto>
{
    public async Task<BookDto> Handle(
        CreateBookCommand request,
        CancellationToken cancellationToken
    )
    {
        var isbn = Isbn.Create(request.Isbn);

        var book = new Book(
            request.Title,
            request.Author,
            isbn,
            request.PageCount,
            request.PublicationDate,
            request.TotalCopies,
            request.Genre,
            request.Description
        );

        var createdBook = await repository.CreateBookAsync(book);

        return mapper.Map<BookDto>(createdBook);
    }
}
