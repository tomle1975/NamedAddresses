namespace Tql.Agdata.NamedAddress.Domain.SeedWork;

/// <summary>
/// Represents a repository that is responsible for the persistence of domain entity.
/// </summary>
public interface IRepository<TContext, TEntity, in TEntityId>
    where TContext : new()
    where TEntity : IAggregateRoot<TEntityId>
{
    /// <summary>
    /// Gets all records of <see cref="TEntity" />.
    /// </summary>
    Task<IEnumerable<TEntity>> GetAll(CancellationToken cancellation);

    /// <summary>
    /// Get <see cref="TEntity" /> by its unique identifier.
    /// </summary>
    Task<TEntity?>? GetById(TEntityId id, CancellationToken cancellation);

    /// <summary>
    /// Inserts a new record of the <see cref="TEntity" />.
    /// </summary>
    Task Insert(TEntity entity, CancellationToken cancellation);

    /// <summary>
    /// Updates the existing record of the <see cref="TEntity" />.
    /// </summary>
    Task Update(TEntity entity, CancellationToken cancellation);

    /// <summary>
    /// Deletes the existing record of the <see cref="TEntity" />.
    /// </summary>
    Task Delete(TEntity entity, CancellationToken cancellation);

    /// <summary>
    /// Gets or sets database context.
    /// </summary>
    TContext Context { get; set; }
}