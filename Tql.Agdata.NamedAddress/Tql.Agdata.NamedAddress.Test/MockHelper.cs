using FluentValidation;
using Moq;
using Tql.Agdata.NamedAddress.Domain.Aggregates.AddressAggregate;
using Tql.Agdata.NamedAddress.Domain.SeedWork;
using Tql.Agdata.NamedAddress.Infrastructure.DataMappings;
using Tql.Agdata.NamedAddress.Infrastructure.Repositories.EntityFramework;
using Db = Tql.Agdata.NamedAddress.Infrastructure.Repositories.EntityFramework.Tables;

namespace Tql.Agdata.NamedAddress.Test;

/// <summary>
/// Represents a helper that is responsible for mocking using Moq framework.
/// </summary>
public class MockHelper
{
    /// <summary>
    /// Gets <see cref="IUnitOfWork{TContext}"/> mock.
    /// </summary>
    public Mock<IUnitOfWork<AddressEfDbContext>> UnitOfWorkMock => new();

    /// <summary>
    /// Gets <see cref="IAddressRepository{TContext}"/> mock.
    /// </summary>
    public Mock<IAddressRepository<AddressEfDbContext>> AddressRepositoryMock => new();

    /// <summary>
    /// Gets <see cref="IRepositoryDataMapper{TEntity,TDbEntity}" /> mock.
    /// </summary>
    public Mock<IRepositoryDataMapper<Address, Db.Address>> RepositoryDataMapperMock => new();

    /// <summary>
    /// Gets <see cref="IValidator{T}"/> mock for address validator.
    /// </summary>
    public Mock<IValidator<Address>> AddressValidatorMock => new();
}