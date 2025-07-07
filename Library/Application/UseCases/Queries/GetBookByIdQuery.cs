using Application.DTOs;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Queries;

public record GetBookByIdQuery(int Id) : IRequest<BookDto>;

public class GetBookByIdQueryHandler(IBookRepository repository, IMapper mapper)
    : IRequestHandler<GetBookByIdQuery, BookDto>
{
    public async Task<BookDto> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        var book = await repository.GetBookByIdAsync(request.Id);

        if (book == null)
            throw new KeyNotFoundException($"Book with ID {request.Id} not found");

        return mapper.Map<BookDto>(book);
    }
}
