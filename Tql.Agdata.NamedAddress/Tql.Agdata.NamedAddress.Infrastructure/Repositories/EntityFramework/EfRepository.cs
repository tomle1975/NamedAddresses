using Tql.Agdata.NamedAddress.Domain.SeedWork;
using Tql.Agdata.NamedAddress.Infrastructure.DataMappings;

namespace Tql.Agdata.NamedAddress.Infrastructure.Repositories.EntityFramework;

/// <inheritdoc cref="IRepository{TEntity,TEntityId}"/>
public class EfRepository<TEntity, TEntityId, TDbEntity> : IRepository<TEntity, TEntityId>, IDisposable
    where TEntity : Entity<TEntityId>, IAggregateRoot<TEntityId>
    where TDbEntity : class
{
    protected readonly IRepositoryDataMapper<TEntity, TDbEntity> RepositoryMapper;
    private bool disposed;

    /// <summary>
    /// Creates a new repository of <see cref="TEntity" /> with unit of work.
    /// </summary>
    public EfRepository(IUnitOfWork<AddressEfDbContext> unitOfWork, IRepositoryDataMapper<TEntity, TDbEntity> repositoryMapper)
        : this(unitOfWork.Context, repositoryMapper)
    {
    }

    /// <summary>
    /// Creates a new repository with address database context.
    /// </summary>
    public EfRepository(AddressEfDbContext context, IRepositoryDataMapper<TEntity, TDbEntity> repositoryMapper)
    {
        disposed = false;
        Context = context;
        RepositoryMapper = repositoryMapper;
    }

    /// <summary>
    /// Gets or sets address database context.
    /// </summary>
    public AddressEfDbContext Context { get; set; }

    /// <inheritdoc cref="IDisposable.Dispose" />
    public void Dispose()
    {
        Context.Dispose();
        disposed = true;
    }

    /// <inheritdoc cref="IRepository{TEntity, TEntityId}.GetAll" />
    public Task<IEnumerable<TEntity>> GetAll(CancellationToken cancellation)
    {
        return cancellation.IsCancellationRequested
            ? Task.FromCanceled<IEnumerable<TEntity>>(cancellation)
            : Task.FromResult(Context.Set<TDbEntity>().AsEnumerable().Select(e => RepositoryMapper.ToEntity(e)));
    }

    /// <inheritdoc cref="IRepository{TEntity, TEntityId}.GetById" />
    public async Task<TEntity?> GetById(TEntityId id, CancellationToken cancellation)
    {
        var dbEntity = await Context.FindAsync<TDbEntity>(id, cancellation).ConfigureAwait(false);
        return dbEntity == null ? null : RepositoryMapper.ToEntity(dbEntity);
    }

    /// <inheritdoc cref="IRepository{TEntity, TEntityId}.Insert" />
    public async Task Insert(TEntity entity, CancellationToken cancellation)
    {
        if (disposed) Context = new AddressEfDbContext();

        var dbEntity = RepositoryMapper.ToDbEntity(entity);
        await Context.AddAsync(dbEntity, cancellation).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IRepository{TEntity, TEntityId}.Update" />
    public async Task Update(TEntity entity, CancellationToken cancellation)
    {
        if (disposed) Context = new AddressEfDbContext();

        var dbEntity = await Context.FindAsync<TDbEntity>(entity.Id, cancellation).ConfigureAwait(false) ?? throw new InvalidOperationException();
        RepositoryMapper.ToDbEntity(entity, dbEntity);
    }

    /// <inheritdoc cref="IRepository{TEntity, TEntityId}.Delete" />
    public async Task Delete(TEntity entity, CancellationToken cancellation)
    {
        if (disposed) Context = new AddressEfDbContext();

        var dbEntity = await Context.FindAsync<TDbEntity>(entity.Id, cancellation).ConfigureAwait(false);
        if (dbEntity == null) return;

        Context.Remove(dbEntity);
    }
}