using Microsoft.EntityFrameworkCore;

namespace Tql.Agdata.NamedAddress.Domain.SeedWork;

/// <summary>
/// Represents a unit of work that manages transactional persistence of one or more repositories.
/// </summary>
public interface IUnitOfWork<out TContext> where TContext : DbContext, new()
{
    /// <summary>
    /// Gets database context.
    /// </summary>
    TContext Context { get; }

    /// <summary>
    /// Creates a database transaction for atomic operations.
    /// </summary>
    Task CreateTransaction(CancellationToken cancellation);

    /// <summary>
    /// Saves changes permanently in the database when all the transactions are completed successfully.
    /// </summary>
    Task Commit(CancellationToken cancellation);

    /// <summary>
    /// Rolls back the database changes to its previous state.
    /// </summary>
    Task Rollback(CancellationToken cancellation);

    /// <summary>
    /// Saves changes in the database.
    /// </summary>
    Task Save(CancellationToken cancellation);
}