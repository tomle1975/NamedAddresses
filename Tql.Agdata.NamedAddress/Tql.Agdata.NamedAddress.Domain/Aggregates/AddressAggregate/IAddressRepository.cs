using Tql.Agdata.NamedAddress.Domain.SeedWork;

namespace Tql.Agdata.NamedAddress.Domain.Aggregates.AddressAggregate;

/// <summary>
/// Represents a repository that manages CRUD operations of addresses.
/// </summary>
public interface IAddressRepository : IRepository<Address, long>
{
    /// <summary>
    /// Get address by name.
    /// </summary>
    Task<Address?> GetByName(string name, CancellationToken cancellation);
}