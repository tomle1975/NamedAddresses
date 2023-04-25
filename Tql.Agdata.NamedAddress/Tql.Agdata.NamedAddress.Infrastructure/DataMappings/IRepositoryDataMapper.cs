namespace Tql.Agdata.NamedAddress.Infrastructure.DataMappings;

/// <summary>
/// Represents a data mapper that is responsible for mapping data between <see cref="TEntity" /> and
/// <see cref="TDbEntity" />.
/// </summary>
public interface IRepositoryDataMapper<TEntity, TDbEntity>
{
    /// <summary>
    /// Maps the given <see cref="TDbEntity" /> to <see cref="TEntity" />.
    /// </summary>
    TEntity ToEntity(TDbEntity entity);

    /// <summary>
    /// Maps from <see cref="TEntity" /> to <see cref="TDbEntity" />.
    /// </summary>
    TDbEntity ToDbEntity(TEntity entity);

    /// <summary>
    /// Maps the given <see cref="TEntity" /> to the referenced <see cref="TDbEntity" />.
    /// </summary>
    void ToDbEntity(TEntity entity, TDbEntity dbEntity);
}