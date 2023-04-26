using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Tql.Agdata.NamedAddress.Domain.Aggregates.AddressAggregate;
using Tql.Agdata.NamedAddress.Infrastructure.Services;
using Xunit;

namespace Tql.Agdata.NamedAddress.Test.Services;

/// <summary>
/// Tests the <see cref="AddressEfService" /> class.
/// </summary>
public class AddressEfServiceTest
{
    private readonly List<Address>? addresses;
    private readonly MockHelper mockHelper = new();
    private AddressEfService? service;

    public AddressEfServiceTest()
    {
        addresses = new List<Address>
        {
            new(1, "Address 1", "123 Main Street", null, "Atlanta", "GA", "30100", "USA"),
            new(2, "Address 2", "234 Main Street", null, "Atlanta", "GA", "30100", "USA"),
            new(3, "Address 3", "345 Main Street", null, "Atlanta", "GA", "30100", "USA")
        };
    }

    [Fact]
    public async Task GetAll_ReturnsAddresses_WhenCached()
    {
        service = new AddressEfService(
            mockHelper.UnitOfWorkMock.Object,
            mockHelper.AddressRepositoryMock.Object,
            mockHelper.AddressValidatorMock.Object,
            new FakeCache(addresses));

        var result = await service.GetAll(CancellationToken.None).ConfigureAwait(false);
        result.IsSuccessful.Should().BeTrue();
        result.Data?.Count().Should().Be(3);
    }

    [Fact]
    public async Task GetAll_ReturnsAddresses_WhenNotCached()
    {
        service = new AddressEfService(
            mockHelper.UnitOfWorkMock.Object,
            mockHelper.AddressRepositoryMock.Object,
            mockHelper.AddressValidatorMock.Object,
            new FakeCache(null));

        mockHelper.AddressRepositoryMock.Setup(m => m.GetAll(It.IsAny<CancellationToken>())).ReturnsAsync(addresses!);

        var result = await service.GetAll(CancellationToken.None).ConfigureAwait(false);
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public async Task GetByName_ReturnsAddress_WhenCached()
    {
        service = new AddressEfService(
            mockHelper.UnitOfWorkMock.Object,
            mockHelper.AddressRepositoryMock.Object,
            mockHelper.AddressValidatorMock.Object,
            new FakeCache(addresses));

        var result = await service.GetByName("Address 1", CancellationToken.None).ConfigureAwait(false);
        result.IsSuccessful.Should().BeTrue();
        result.Data?.Id.Should().Be(1);
    }

    public class FakeCache : IMemoryCache
    {
        private readonly List<Address>? addresses;

        public FakeCache(List<Address>? addresses)
        {
            this.addresses = addresses;
        }

        public void Dispose()
        {
        }

        public bool TryGetValue(object key, out object? value)
        {
            value = addresses;
            return true;
        }

        public ICacheEntry CreateEntry(object key)
        {
            throw new NotImplementedException();
        }

        public void Remove(object key)
        {
        }
    }
}