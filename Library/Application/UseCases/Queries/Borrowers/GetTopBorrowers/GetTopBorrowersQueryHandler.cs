using Application.DTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Queries.Borrowers.GetTopBorrowers;

public class GetTopBorrowersQueryHandler(IBorrowerRepository repository)
    : IRequestHandler<GetTopBorrowersQuery, List<TopBorrowerDto>>
{
    public async Task<List<TopBorrowerDto>> Handle(
        GetTopBorrowersQuery request,
        CancellationToken cancellationToken
    )
    {
        var topBorrowers = await repository.GetTopBorrowersAsync(
            request.Count,
            request.StartDate,
            request.EndDate
        );

        return topBorrowers
            .Select(x => new TopBorrowerDto
            {
                BorrowerId = x.Borrower.Id,
                FullName = x.Borrower.FullName,
                Email = x.Borrower.Email.Value,
                BorrowCount = x.BorrowCount,
                TotalBooksRead = x.Borrower.GetTotalBooksRead(),
                AverageReadingRate = x.Borrower.GetAverageReadingRate(),
            })
            .ToList();
    }
}
