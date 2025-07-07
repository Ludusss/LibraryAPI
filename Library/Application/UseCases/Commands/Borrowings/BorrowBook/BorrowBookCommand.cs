using Application.DTOs;
using MediatR;

namespace Application.Commands.Borrowings.BorrowBook;

public class BorrowBookCommand : IRequest<BorrowingDto>
{
    public int BorrowerId { get; set; }
    public int BookId { get; set; }
    public int BorrowDurationDays { get; set; } = 14;

    public BorrowBookCommand(int borrowerId, int bookId, int borrowDurationDays = 14)
    {
        BorrowerId = borrowerId;
        BookId = bookId;
        BorrowDurationDays = borrowDurationDays;
    }
}
