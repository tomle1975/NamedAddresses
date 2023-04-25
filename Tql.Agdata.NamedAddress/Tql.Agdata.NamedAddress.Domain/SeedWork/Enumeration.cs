using System.Reflection;

namespace Tql.Agdata.NamedAddress.Domain.SeedWork;

/// <summary>
/// Represents an enumeration type.
/// </summary>
public abstract class Enumeration : IComparable
{
    protected Enumeration(int id, string name)
    {
        Name = name;
        Id = id;
    }

    #region Helper Methods

    private static TEnumeration Parse<TEnumeration, TValue>(TValue value, string description,
        Func<TEnumeration, bool> predicate) where TEnumeration : Enumeration
    {
        var matchingItem = GetAll<TEnumeration>().FirstOrDefault(predicate);
        return matchingItem ??
               throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(TEnumeration)}");
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the name of enumeration.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the identifier of enumeration.
    /// </summary>
    public int Id { get; }

    #endregion

    #region Behavioral Methods

    /// <summary>
    /// Gets all enumerated values.
    /// </summary>
    public static IEnumerable<TEnumeration> GetAll<TEnumeration>() where TEnumeration : Enumeration
    {
        var fields =
            typeof(TEnumeration).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
        return fields.Select(f => f.GetValue(null)).Cast<TEnumeration>();
    }

    /// <summary>
    /// Gets the absolute difference between 2 enumerations based on its identifiers.
    /// </summary>
    public static int AbsoluteDifference(Enumeration firstValue, Enumeration secondValue)
    {
        var absoluteDifference = Math.Abs(firstValue.Id - secondValue.Id);
        return absoluteDifference;
    }

    /// <summary>
    /// Converts an enumerated identifier/value to its type.
    /// </summary>
    public static TEnumeration FromValue<TEnumeration>(int value) where TEnumeration : Enumeration
    {
        var matchingItem = Parse<TEnumeration, int>(value, "value", item => item.Id == value);
        return matchingItem;
    }

    /// <summary>
    /// Converts an enumerated name to its type.
    /// </summary>
    /// <typeparam name="TEnumeration"></typeparam>
    /// <param name="displayName"></param>
    /// <returns></returns>
    public static TEnumeration FromDisplayName<TEnumeration>(string displayName) where TEnumeration : Enumeration
    {
        var matchingItem = Parse<TEnumeration, string>(displayName, "display name", item => item.Name == displayName);
        return matchingItem;
    }

    #endregion

    #region Overridden Methods

    /// <inheritdoc cref="Object.ToString" />
    public override string ToString()
    {
        return Name;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Enumeration otherValue) return false;

        var typeMatches = GetType() == obj.GetType();
        var valueMatches = Id.Equals(otherValue.Id);
        return typeMatches && valueMatches;
    }

    /// <inheritdoc cref="Object.GetHashCode" />
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <inheritdoc cref="IComparable.CompareTo" />
    public int CompareTo(object? other)
    {
        return other != null
            ? Id.CompareTo(((Enumeration)other).Id)
            : throw new InvalidOperationException($"'{nameof(other)}' is not a valid Enumeration.");
    }

    #endregion
}