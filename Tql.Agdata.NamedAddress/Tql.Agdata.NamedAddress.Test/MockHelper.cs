using Moq;
using Tql.Agdata.NamedAddress.Domain.Aggregates.AddressAggregate;
using Tql.Agdata.NamedAddress.Infrastructure.DataMappings;
using Db = Tql.Agdata.NamedAddress.Infrastructure.Repositories.EntityFramework.Tables;

namespace Tql.Agdata.NamedAddress.Test;

/// <summary>
/// Represents a helper that is responsible for mocking using Moq framework.
/// </summary>
public class MockHelper
{
    /// <summary>
    /// Gets <see cref="IRepositoryDataMapper{TEntity,TDbEntity}" /> mock.
    /// </summary>
    public Mock<IRepositoryDataMapper<Address, Db.Address>> RepositoryDataMapperMock => new();
}