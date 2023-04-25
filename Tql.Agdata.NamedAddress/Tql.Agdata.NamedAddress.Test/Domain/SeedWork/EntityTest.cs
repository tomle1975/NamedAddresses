using FluentAssertions;
using Tql.Agdata.NamedAddress.Domain.Aggregates.AddressAggregate;
using Tql.Agdata.NamedAddress.Domain.SeedWork;
using Xunit;

namespace Tql.Agdata.NamedAddress.Test.Domain.SeedWork;

/// <summary>
/// Tests the <see cref="Entity{TEntityId}" /> class.
/// </summary>
public class EntityTest
{
    private readonly Address address;

    public EntityTest()
    {
        address = new Address(
            1,
            "Address1",
            "123 Main Street",
            null,
            "Powder Springs",
            "GA",
            "30127",
            "USA");
    }

    [Fact]
    public void Entity_IsTransient_WhenIdIsNotInitialized()
    {
        var address1 = new Address(
            0,
            "Address1",
            "123 Main Street",
            null,
            "Powder Springs",
            "GA",
            "30127",
            "USA");

        address1.IsTransient().Should().BeTrue();
    }

    public record NonEntity(string Name, string Description)
    {
    }

    public class EntityType : EntityTest
    {
    }

    #region Equals Tests

    [Fact]
    public void Entity_IsNotEqual_WhenNotEntity()
    {
        var nonEntity = new NonEntity("Test", "test");
        address.Equals(nonEntity).Should().BeFalse();
    }

    [Fact]
    public void Entity_IsEqual_WhenHasSameReference()
    {
        var address2 = address;
        address.Equals(address2).Should().BeTrue();
    }

    [Fact]
    public void Entity_IsNotEqual_WhenNotSameType()
    {
        var entity = new EntityType();
        address.Equals(entity).Should().BeFalse();
    }

    [Fact]
    public void Entity_IsNotEqual_WhenBothAreTransient()
    {
        var address1 = new Address(
            0,
            "Address1",
            "123 Main Street",
            null,
            "Powder Springs",
            "GA",
            "30127",
            "USA");

        var address2 = new Address(
            0,
            "Address1",
            "123 Main Street",
            null,
            "Powder Springs",
            "GA",
            "30127",
            "USA");

        address1.Equals(address2).Should().BeFalse();
        (address1 != address2).Should().BeTrue();
    }

    [Fact]
    public void Entity_IsEqual_WhenIdsAreEqual()
    {
        var address1 = new Address(
            1,
            "Address1",
            "123 Main Street",
            null,
            "Powder Springs",
            "GA",
            "30127",
            "USA");

        var address2 = new Address(
            1,
            "Address1",
            "123 Main Street",
            null,
            "Powder Springs",
            "GA",
            "30127",
            "USA");

        address1.Equals(address2).Should().BeTrue();
    }

    #endregion
}