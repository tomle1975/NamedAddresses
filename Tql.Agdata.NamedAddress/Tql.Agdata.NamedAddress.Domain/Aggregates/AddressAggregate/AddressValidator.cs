using FluentValidation;

namespace Tql.Agdata.NamedAddress.Domain.Aggregates.AddressAggregate;

/// <summary>
/// Represents a domain validator for the <see cref="Address" /> entity.
/// </summary>
public class AddressValidator : AbstractValidator<Address>
{
    public AddressValidator()
    {
        RuleFor(x => x.Name).Length(1, 50);

        RuleFor(x => x.StreetLine1).Length(1, 150);

        RuleFor(x => x.StreetLine2).Length(1, 150).When(x => x.StreetLine2 != null);

        RuleFor(x => x.City).Length(1, 25);

        RuleFor(x => x.State).Length(2, 15);

        RuleFor(x => x.ZipCode).Length(2, 15);

        RuleFor(x => x.Country).Length(1, 15);
    }
}