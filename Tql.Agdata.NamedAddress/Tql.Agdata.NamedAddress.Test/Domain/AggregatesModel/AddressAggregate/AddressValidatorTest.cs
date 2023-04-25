using FluentAssertions;
using Tql.Agdata.NamedAddress.Domain.Aggregates.AddressAggregate;
using Xunit;

namespace Tql.Agdata.NamedAddress.Test.Domain.AggregatesModel.AddressAggregate;

/// <summary>
/// Tests the <see cref="AddressValidator" /> class.
/// </summary>
public class AddressValidatorTest
{
    private readonly AddressValidator validator = new();

    [Theory]
    [InlineData("Address1", "123 Main Street", null, "Powder Springs", "GA", "30127", "USA", true)]
    [InlineData("", "123 Main Street", null, "Powder Springs", "GA", "30127", "USA", false)]
    [InlineData("Address1", "", null, "Powder Springs", "GA", "30127", "USA", false)]
    [InlineData("Address1", "123 Main Street", null, "", "GA", "30127", "USA", false)]
    [InlineData("Address1", "123 Main Street", null, "Powder Springs", "", "30127", "USA", false)]
    [InlineData("Address1", "123 Main Street", null, "Powder Springs", "GA", "", "USA", false)]
    [InlineData("Address1", "123 Main Street", null, "Powder Springs", "GA", "30127", "", false)]
    public void Validate_ReturnsResult_WhenCalled(string? name, string? street1, string street2, string? city, string? state, string? zip, string? country, bool isValid)
    {
        var address = new Address(1, name, street1, street2, city, state, zip, country);
        var result = validator.Validate(address);
        result.IsValid.Should().Be(isValid);
    }
}