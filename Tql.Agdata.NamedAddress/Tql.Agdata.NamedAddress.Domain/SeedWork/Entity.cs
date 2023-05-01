namespace Tql.Agdata.NamedAddress.Domain.SeedWork;

/// <summary>
/// Represents a domain model entity.
/// </summary>
public abstract class Entity<TEntityId>
{
    /// <summary>
    /// Gets the entity's identifier.
    /// </summary>
    public TEntityId? Id { get; set; }

    /// <summary>
    /// Determines if the entity is transient or not based on its identifier.
    /// </summary>
    /// <returns></returns>
    public bool IsTransient()
    {
        return Id == null || Id.Equals(default(TEntityId));
    }

    /// <inheritdoc cref="Object" />
    public override bool Equals(object? obj)
    {
        if (obj is not Entity<TEntityId> item) return false;

        if (ReferenceEquals(this, item)) return true;

        if (GetType() != item.GetType()) return false;

        if (item.IsTransient() || IsTransient()) return false;

        return item.Id != null && item.Id.Equals(Id);
    }

    /// <inheritdoc cref="Object.GetHashCode" />
    public override int GetHashCode()
    {
        return Id == null ? base.GetHashCode() : Id.GetHashCode();
    }

    /// <summary>
    /// Evaluates if 2 entities are equal.
    /// </summary>
    public static bool operator ==(Entity<TEntityId>? left, Entity<TEntityId>? right)
    {
        return left?.Equals(right) ?? Equals(right, null);
    }

    /// <summary>
    /// Evaluates if 2 entities are not equal.
    /// </summary>
    public static bool operator !=(Entity<TEntityId>? left, Entity<TEntityId>? right)
    {
        return !(left == right);
    }
}