using Application.DTOs;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Queries.Borrowers.GetActiveBorrowers;

public class GetActiveBorrowersQueryHandler(IBorrowerRepository repository, IMapper mapper)
    : IRequestHandler<GetActiveBorrowersQuery, List<BorrowerSummaryDto>>
{
    public async Task<List<BorrowerSummaryDto>> Handle(
        GetActiveBorrowersQuery request,
        CancellationToken cancellationToken
    )
    {
        var borrowers = await repository.GetActiveBorrowersAsync();
        return mapper.Map<List<BorrowerSummaryDto>>(borrowers);
    }
}
