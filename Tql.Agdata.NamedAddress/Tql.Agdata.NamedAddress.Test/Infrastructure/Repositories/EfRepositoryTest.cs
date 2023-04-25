using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tql.Agdata.NamedAddress.Domain.Aggregates.AddressAggregate;
using Tql.Agdata.NamedAddress.Infrastructure.DataMappings;
using Tql.Agdata.NamedAddress.Infrastructure.Repositories.EntityFramework;
using Xunit;
using Db = Tql.Agdata.NamedAddress.Infrastructure.Repositories.EntityFramework.Tables;

namespace Tql.Agdata.NamedAddress.Test.Infrastructure.Repositories;

/// <summary>
/// Tests the <see cref="EfRepository{TEntity,TEntityId,TDbEntity}" /> class.
/// </summary>
public class EfRepositoryTest
{
    private readonly EfRepository<Address, long, Db.Address> repository;

    public EfRepositoryTest()
    {
        repository = new EfRepository<Address, long, Db.Address>(new AddressEfDbContext(), new AddressRepositoryDataMapper());
    }

    [Fact]
    public void Context_ReturnsContext_WhenConstructed()
    {
        repository.Context.Should().NotBeNull();
    }

    [Fact]
    public async Task Insert_AddressIsInserted_WhenCompleted()
    {
        var address = new Address(0, "Golf course 1", "123 Main course", null, "Atlanta", "GA", "30102", "USA");
        await repository.Insert(address, CancellationToken.None).ConfigureAwait(false);
        await repository.Context.SaveChangesAsync().ConfigureAwait(false);

        var dbAddress = repository.Context.Addresses?.FirstOrDefault(a => a.Name == "Golf course 1");
        dbAddress.Should().NotBeNull();
    }

    [Fact]
    public async Task Update_AddressIsUpdated_WhenCompleted()
    {
        var address = new Address(0, "Golf course 2", "234 Main course", null, "Atlanta", "GA", "30102", "USA");
        await repository.Insert(address, CancellationToken.None).ConfigureAwait(false);
        await repository.Context.SaveChangesAsync().ConfigureAwait(false);

        var dbAddress = repository.Context.Addresses?.First(a => a.Name == "Golf course 2");
        if (dbAddress != null) address.Id = dbAddress.Id;
        await repository.Update(address, CancellationToken.None).ConfigureAwait(false);
        await repository.Context.SaveChangesAsync().ConfigureAwait(false);

        var updatedAddress = repository.Context.Addresses?.FirstOrDefault(a => a.Name == "Golf course 2");
        updatedAddress.Should().NotBeNull();
    }

    [Fact]
    public async Task Delete_AddressIsRemoved_WhenCompleted()
    {
        var address = new Address(0, "Golf course 3", "567 Main course", null, "Atlanta", "GA", "30102", "USA");
        await repository.Insert(address, CancellationToken.None).ConfigureAwait(false);
        await repository.Context.SaveChangesAsync().ConfigureAwait(false);

        var dbAddress = repository.Context.Addresses?.First(a => a.Name == "Golf course 3");
        if (dbAddress != null) address.Id = dbAddress.Id;

        await repository.Delete(address, CancellationToken.None).ConfigureAwait(false);
        await repository.Context.SaveChangesAsync().ConfigureAwait(false);

        var deletedAddress = repository.Context.Addresses?.FirstOrDefault(a => a.Name == "Golf course 3");
        deletedAddress.Should().BeNull();
    }

    [Fact]
    public async Task GetAll_ReturnsAddresses_WhenCalled()
    {
        var address = new Address(0, "Golf course 5", "567 Main course", null, "Atlanta", "GA", "30102", "USA");
        await repository.Insert(address, CancellationToken.None).ConfigureAwait(false);
        await repository.Context.SaveChangesAsync().ConfigureAwait(false);

        var addresses = await repository.GetAll(CancellationToken.None).ConfigureAwait(false);
        addresses.Count().Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetById_ReturnAddress_WhenCalled()
    {
        var address = new Address(0, "Golf course 4", "567 Main course", null, "Atlanta", "GA", "30102", "USA");
        await repository.Insert(address, CancellationToken.None).ConfigureAwait(false);
        await repository.Context.SaveChangesAsync().ConfigureAwait(false);

        if (repository.Context.Addresses != null)
        {
            var insertedAddress = await repository.Context.Addresses
                .Where(a => a.Name == address.Name)
                .FirstAsync(CancellationToken.None)
                .ConfigureAwait(false);
            var foundAddress = await repository.GetById(insertedAddress.Id, CancellationToken.None).ConfigureAwait(false);
            foundAddress.Should().NotBeNull();
        }
    }
}