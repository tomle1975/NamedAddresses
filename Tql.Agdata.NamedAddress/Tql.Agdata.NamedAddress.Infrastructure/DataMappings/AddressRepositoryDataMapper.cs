using Tql.Agdata.NamedAddress.Domain.Aggregates.AddressAggregate;
using Db = Tql.Agdata.NamedAddress.Infrastructure.Repositories.EntityFramework.Tables;

namespace Tql.Agdata.NamedAddress.Infrastructure.DataMappings;

/// <inheritdoc cref="IRepositoryDataMapper{TEntity,TDbEntity}" />
public class AddressRepositoryDataMapper : IRepositoryDataMapper<Address, Db.Address>
{
    /// <inheritdoc cref="IRepositoryDataMapper{TEntity,TDbEntity}.ToEntity" />
    public Address ToEntity(Db.Address entity)
    {
        return new Address(entity.Id, entity.Name, entity.StreetLine1, entity.StreetLine2, entity.City, entity.State, entity.ZipCode, entity.Country);
    }

    /// <inheritdoc cref="IRepositoryDataMapper{TEntity,TDbEntity}.ToDbEntity(TEntity)" />
    public Db.Address ToDbEntity(Address entity)
    {
        return new Db.Address
        {
            Id = entity.Id,
            Name = entity.Name,
            StreetLine1 = entity.StreetLine1,
            StreetLine2 = entity.StreetLine2,
            City = entity.City,
            State = entity.State,
            ZipCode = entity.ZipCode,
            Country = entity.Country
        };
    }

    /// <inheritdoc cref="IRepositoryDataMapper{TEntity,TDbEntity}.ToDbEntity(TEntity, TDbEntity)" />
    public void ToDbEntity(Address entity, Db.Address dbEntity)
    {
        dbEntity.Name = entity.Name;
        dbEntity.StreetLine1 = entity.StreetLine1;
        dbEntity.StreetLine2 = entity.StreetLine2;
        dbEntity.City = entity.City;
        dbEntity.State = entity.State;
        dbEntity.ZipCode = entity.ZipCode;
        dbEntity.Country = entity.Country;
    }
}