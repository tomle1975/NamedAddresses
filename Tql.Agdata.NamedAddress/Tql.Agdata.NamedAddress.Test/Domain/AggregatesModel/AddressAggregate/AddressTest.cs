using FluentAssertions;
using Tql.Agdata.NamedAddress.Domain.Aggregates.AddressAggregate;
using Xunit;

namespace Tql.Agdata.NamedAddress.Test.Domain.AggregatesModel.AddressAggregate;

/// <summary>
/// Tests the <see cref="NamedAddress" /> entity model.
/// </summary>
public class AddressTest
{
    [Fact]
    public void NamedAddress_CreatesNewInstance_WhenConstructed()
    {
        var address = new Address(
            1,
            "Address1",
            "123 Main Street",
            null,
            "Powder Springs",
            "GA",
            "30127",
            "USA");

        address.Should().NotBeNull();
        address.Name.Should().Be("Address1");
        address.StreetLine1.Should().Be("123 Main Street");
        address.StreetLine2.Should().BeNull();
        address.City.Should().Be("Powder Springs");
        address.State.Should().Be("GA");
        address.ZipCode.Should().Be("30127");
        address.Country.Should().Be("USA");
    }
}