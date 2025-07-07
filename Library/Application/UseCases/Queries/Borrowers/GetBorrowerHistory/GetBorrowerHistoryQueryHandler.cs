using Application.DTOs;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Queries.Borrowers.GetBorrowerHistory;

public class GetBorrowerHistoryQueryHandler(IBorrowingRepository repository, IMapper mapper)
    : IRequestHandler<GetBorrowerHistoryQuery, List<BorrowingHistoryDto>>
{
    public async Task<List<BorrowingHistoryDto>> Handle(
        GetBorrowerHistoryQuery request,
        CancellationToken cancellationToken
    )
    {
        var history = await repository.GetBorrowerHistoryAsync(
            request.BorrowerId,
            request.StartDate,
            request.EndDate
        );
        return mapper.Map<List<BorrowingHistoryDto>>(history);
    }
}
