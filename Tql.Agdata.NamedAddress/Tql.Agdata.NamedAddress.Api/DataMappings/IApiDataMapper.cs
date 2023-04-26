namespace Tql.Agdata.NamedAddress.Api.DataMappings
{
    /// <summary>
    /// Represents a data mapper that is responsible for data mapping from Domain/Infrastructure to API.
    /// </summary>
    public interface IApiDataMapper<TEntity, TApiDto>
    {
        /// <summary>
        /// Maps the given <see cref="ToApiDto" /> to <see cref="TEntity" />.
        /// </summary>
        TEntity ToEntity(TApiDto dto);

        /// <summary>
        /// Maps from <see cref="TEntity" /> to <see cref="TApiDto" />.
        /// </summary>
        TApiDto ToApiDto(TEntity entity);
    }
}