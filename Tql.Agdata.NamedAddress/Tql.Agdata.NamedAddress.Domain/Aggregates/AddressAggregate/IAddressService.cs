using Tql.Agdata.NamedAddress.Domain.SeedWork;

namespace Tql.Agdata.NamedAddress.Domain.Aggregates.AddressAggregate;

/// <summary>
/// Represents a facade that manages operations around the address domain.
/// </summary>
public interface IAddressService
{
    /// <summary>
    /// Gets all addresses.
    /// </summary>
    Task<Result<IEnumerable<Address>>> GetAll(CancellationToken cancellation);

    /// <summary>
    /// Get address by name.
    /// </summary>
    Task<Result<Address?>> GetByName(string name, CancellationToken cancellation);

    /// <summary>
    /// Adds a new address.
    /// </summary>
    Task<Result<long>> Add(Address address, CancellationToken cancellation);

    /// <summary>
    /// Updates the existing address.
    /// </summary>
    Task<Result<long>> Update(Address address, CancellationToken cancellation);

    /// <summary>
    /// Deletes the existing address.
    /// </summary>
    Task<Result<long>> Delete(Address address, CancellationToken cancellation);
}