using Application.DTOs;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Queries.Borrowers.GetBorrowerById;

public class GetBorrowerByIdQueryHandler(IBorrowerRepository repository, IMapper mapper)
    : IRequestHandler<GetBorrowerByIdQuery, BorrowerDto>
{
    public async Task<BorrowerDto> Handle(
        GetBorrowerByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var borrower = await repository.GetBorrowerByIdAsync(request.Id);

        if (borrower == null)
            throw new KeyNotFoundException($"Borrower with ID {request.Id} not found");

        return mapper.Map<BorrowerDto>(borrower);
    }
}
