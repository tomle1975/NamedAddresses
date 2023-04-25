namespace Tql.Agdata.NamedAddress.Domain.SeedWork;

/// <summary>
/// Represents an aggregate root in the domain model.
/// </summary>
public interface IAggregateRoot<TIdentifier>
{
    /// <summary>
    /// Gets the entity's identifier.
    /// </summary>
    public TIdentifier Id { get; set; }
}