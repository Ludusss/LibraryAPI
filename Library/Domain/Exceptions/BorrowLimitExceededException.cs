using Domain.Common;

namespace Domain.Exceptions;

public class BorrowLimitExceededException : DomainException
{
    public BorrowLimitExceededException(int borrowerId, int maxLimit, int currentCount)
        : base(
            $"Borrower {borrowerId} has reached their borrow limit. Maximum: {maxLimit}, Current: {currentCount}"
        ) { }

    public BorrowLimitExceededException(string borrowerName, int maxLimit)
        : base($"Borrower '{borrowerName}' has reached their borrow limit of {maxLimit} books.") { }
}
