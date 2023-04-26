namespace Tql.Agdata.NamedAddress.Api.Models;

public record AddressDto(long Id, string Name, string StreetLine1, string? StreetLine2, string City, string ZipCode, string State, string Country);