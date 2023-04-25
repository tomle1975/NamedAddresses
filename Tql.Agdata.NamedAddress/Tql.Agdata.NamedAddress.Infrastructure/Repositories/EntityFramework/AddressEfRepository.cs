using Microsoft.EntityFrameworkCore;
using Tql.Agdata.NamedAddress.Domain.Aggregates.AddressAggregate;
using Tql.Agdata.NamedAddress.Domain.SeedWork;
using Tql.Agdata.NamedAddress.Infrastructure.DataMappings;
using Db = Tql.Agdata.NamedAddress.Infrastructure.Repositories.EntityFramework.Tables;

namespace Tql.Agdata.NamedAddress.Infrastructure.Repositories.EntityFramework;

/// <inheritdoc cref="IAddressRepository" />
public class AddressEfRepository : EfRepository<Address, long, Db.Address>, IAddressRepository
{
    /// <inheritdoc
    ///     cref="EfRepository{TEntity, TEntityId, TDbEntity}(Tql.Agdata.NamedAddress.Domain.SeedWork.IUnitOfWork{Tql.Agdata.NamedAddress.Infrastructure.Repositories.EntityFramework.AddressEfDbContext}, IRepositoryDataMapper{TEntity,TDbEntity})" />
    public AddressEfRepository(IUnitOfWork<AddressEfDbContext> unitOfWork, IRepositoryDataMapper<Address, Db.Address> repositoryMapper)
        : base(unitOfWork, repositoryMapper)
    {
    }

    /// <inheritdoc
    ///     cref="EfRepository{TEntity, TEntityId,TDbEntity}(AddressEfDbContext, IRepositoryDataMapper{TEntity,TDbEntity})" />
    public AddressEfRepository(AddressEfDbContext context, IRepositoryDataMapper<Address, Db.Address> repositoryMapper) : base(context, repositoryMapper)
    {
    }

    /// <inheritdoc cref="IAddressRepository.GetByName" />
    public async Task<Address?> GetByName(string name, CancellationToken cancellation)
    {
        if (Context.Addresses == null) return null;

        var dbAddress = await Context.Addresses
            .Where(address => address.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            .FirstOrDefaultAsync(cancellation).ConfigureAwait(false);

        return dbAddress == null ? null : RepositoryMapper.ToEntity(dbAddress);
    }
}