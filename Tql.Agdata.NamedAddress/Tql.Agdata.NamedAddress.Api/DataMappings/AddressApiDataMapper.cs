using Tql.Agdata.NamedAddress.Api.Models;
using Tql.Agdata.NamedAddress.Domain.Aggregates.AddressAggregate;

namespace Tql.Agdata.NamedAddress.Api.DataMappings
{
    public class AddressApiDataMapper : IApiDataMapper<Address, AddressDto>
    {
        /// <inheritdoc cref="IApiDataMapper{TEntity,TApiDto}.ToEntity"/>
        public Address ToEntity(AddressDto dto)
        {
            return new Address(dto.Id, dto.Name, dto.StreetLine1, dto.StreetLine2, dto.City, dto.State, dto.ZipCode, dto.Country);
        }

        /// <inheritdoc cref="IApiDataMapper{TEntity,TApiDto}.ToApiDto"/>
        public AddressDto ToApiDto(Address entity)
        {
            return new AddressDto(entity.Id, entity.Name, entity.StreetLine1, entity.StreetLine2, entity.City, entity.State, entity.ZipCode, entity.Country);
        }
    }
}