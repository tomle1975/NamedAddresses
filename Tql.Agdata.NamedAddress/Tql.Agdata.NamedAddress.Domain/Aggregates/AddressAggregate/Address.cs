using Tql.Agdata.NamedAddress.Domain.SeedWork;

namespace Tql.Agdata.NamedAddress.Domain.Aggregates.AddressAggregate;

/// <summary>
/// Represents a named address as POCO entity.
/// </summary>
public class Address : Entity<long>, IAggregateRoot<long>
{
    /// <summary>
    /// Creates a new instance of the <see cref="Address" />.
    /// </summary>
    public Address(long id, string name, string streetLine1, string? streetLine2, string city, string state,
        string zipCode, string country)
    {
        Id = id;
        Name = name;
        StreetLine1 = streetLine1;
        StreetLine2 = streetLine2;
        City = city;
        State = state;
        ZipCode = zipCode;
        Country = country;
    }

    /// <summary>
    /// Gets the name of the address which is uniquely identifying the address.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets the street number and name; e.g., "123 Main Street".
    /// </summary>
    public string StreetLine1 { get; set; }

    /// <summary>
    /// Gets the additional street line; e.g., P.O. Box number, Apartment number, Suite number, and etc.
    /// </summary>
    public string? StreetLine2 { get; set; }

    /// <summary>
    /// Gets city of the address.
    /// </summary>
    public string City { get; set; }

    /// <summary>
    /// Gets state of the address.
    /// </summary>
    public string State { get; set; }

    /// <summary>
    /// Gets postal code or zip code of the address.
    /// </summary>
    public string ZipCode { get; set; }

    /// <summary>
    /// Gets the country of the address.
    /// </summary>
    public string Country { get; set; }
}