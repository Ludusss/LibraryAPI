using MediatR;

namespace Domain.Common;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
} 