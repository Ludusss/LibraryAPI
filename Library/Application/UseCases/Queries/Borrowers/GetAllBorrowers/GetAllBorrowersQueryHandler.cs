using Application.DTOs;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Queries.Borrowers.GetAllBorrowers;

public class GetAllBorrowersQueryHandler(IBorrowerRepository repository, IMapper mapper)
    : IRequestHandler<GetAllBorrowersQuery, List<BorrowerDto>>
{
    public async Task<List<BorrowerDto>> Handle(
        GetAllBorrowersQuery request,
        CancellationToken cancellationToken
    )
    {
        var borrowers = await repository.GetAllBorrowersAsync();
        return mapper.Map<List<BorrowerDto>>(borrowers);
    }
}
