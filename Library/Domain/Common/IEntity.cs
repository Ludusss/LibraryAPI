namespace Domain.Common;

public interface IEntity
{
    int Id { get; }
}

public interface IEntity<TId> where TId : IEquatable<TId>
{
    TId Id { get; }
} 