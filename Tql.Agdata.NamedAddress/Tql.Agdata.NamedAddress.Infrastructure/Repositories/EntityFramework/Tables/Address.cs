namespace Tql.Agdata.NamedAddress.Infrastructure.Repositories.EntityFramework.Tables;

/// <summary>
/// Represents address table.
/// </summary>
public class Address
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string StreetLine1 { get; set; }

    public string? StreetLine2 { get; set; }

    public string City { get; set; }

    public string State { get; set; }

    public string ZipCode { get; set; }

    public string Country { get; set; }
}