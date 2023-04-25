using Tql.Agdata.NamedAddress.Domain.Aggregates.AddressAggregate;
using Tql.Agdata.NamedAddress.Domain.SeedWork;

namespace Tql.Agdata.NamedAddress.Infrastructure.Services
{
    /// <inheritdoc cref="IAddressService"/>
    public class AddressService : IAddressService
    {
        public Task<Result<IEnumerable<Address>>> GetAll(CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Address>> GetByName(string name, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public Task<Result<DateTime>> Add(Address address, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public Task<Result<DateTime>> Update(Address address, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public Task<Result<DateTime>> Delete(Address address, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }
    }
}