using FluentAssertions;
using Tql.Agdata.NamedAddress.Domain.SeedWork;
using Xunit;

namespace Tql.Agdata.NamedAddress.Test.Domain.SeedWork;

/// <summary>
/// Tests the <see cref="Enumeration" /> class.
/// </summary>
public class EnumerationTest
{
    [Fact]
    public void GetAll_ReturnsEnumerations_WhenCardTypeIsUsed()
    {
        Enumeration.GetAll<CardType>().ToList().Count.Should().Be(3);
    }

    [Fact]
    public void AbsoluteDifference_ReturnsIdsDiff_WhenCalled()
    {
        var card1 = CardType.Amex;
        var card2 = CardType.Visa;
        var card3 = CardType.Master;

        Enumeration.AbsoluteDifference(card1, card2).Should().Be(1);
        Enumeration.AbsoluteDifference(card1, card3).Should().Be(2);
        Enumeration.AbsoluteDifference(card2, card3).Should().Be(1);
    }

    [Fact]
    public void FromValue_ReturnsAmex_WhenValueIs1()
    {
        Enumeration.FromValue<CardType>(1).Should().Be(CardType.Amex);
    }

    [Fact]
    public void FromDisplay_ReturnsAmex_WhenNameIsAmex()
    {
        Enumeration.FromDisplayName<CardType>("Amex").Should().Be(CardType.Amex);
    }

    [Fact]
    public void Equals_ReturnsTrue_WhenSameTypeAndId()
    {
        var amex1 = CardType.Amex;
        var amex2 = CardType.Amex;
        amex1.Equals(amex2).Should().BeTrue();
    }

    public class CardType : Enumeration
    {
        public static CardType Amex = new(1, nameof(Amex));

        public static CardType Visa = new(2, nameof(Visa));

        public static CardType Master = new(3, nameof(Master));

        public CardType(int id, string name) : base(id, name)
        {
        }
    }
}