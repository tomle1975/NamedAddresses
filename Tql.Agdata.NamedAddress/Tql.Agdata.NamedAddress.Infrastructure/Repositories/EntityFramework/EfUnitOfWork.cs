using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Tql.Agdata.NamedAddress.Domain.SeedWork;

namespace Tql.Agdata.NamedAddress.Infrastructure.Repositories.EntityFramework;

/// <inheritdoc cref="IUnitOfWork{TContext}"/>
public class EfUnitOfWork<TContext> : IUnitOfWork<TContext>, IDisposable where TContext : DbContext, new()
{
    private bool disposed;
    private IDbContextTransaction? transaction;

    public EfUnitOfWork()
    {
        Context = new TContext();
    }

    /// <inheritdoc cref="IDisposable.Dispose" />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc cref="IUnitOfWork{TContext}.Context" />
    public TContext Context { get; }

    /// <inheritdoc cref="IUnitOfWork{TContext}.CreateTransaction" />
    public async Task CreateTransaction(CancellationToken cancellation)
    {
        transaction = await Context.Database.BeginTransactionAsync(cancellation).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IUnitOfWork{TContext}.Commit" />
    public async Task Commit(CancellationToken cancellation)
    {
        if (transaction == null) return;

        await transaction.CommitAsync(cancellation).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IUnitOfWork{TContext}.Rollback" />
    public async Task Rollback(CancellationToken cancellation)
    {
        if (transaction == null) return;

        await transaction.RollbackAsync(cancellation).ConfigureAwait(false);
        await transaction.DisposeAsync().ConfigureAwait(false);
    }

    /// <inheritdoc cref="IUnitOfWork{TContext}.Save" />
    public async Task Save(CancellationToken cancellation)
    {
        await Context.SaveChangesAsync(cancellation).ConfigureAwait(false);
    }

    /// <summary>
    /// Disposes unmanaged resources like files, database connections, and etc at any time.
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposed && disposing) Context.Dispose();

        disposed = true;
    }
}