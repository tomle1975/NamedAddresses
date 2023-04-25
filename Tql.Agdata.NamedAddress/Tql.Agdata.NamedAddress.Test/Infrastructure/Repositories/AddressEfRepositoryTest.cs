using FluentAssertions;
using Tql.Agdata.NamedAddress.Domain.Aggregates.AddressAggregate;
using Tql.Agdata.NamedAddress.Infrastructure.DataMappings;
using Tql.Agdata.NamedAddress.Infrastructure.Repositories.EntityFramework;
using Xunit;

namespace Tql.Agdata.NamedAddress.Test.Infrastructure.Repositories
{
    /// <summary>
    /// Tests the <see cref="AddressEfRepository"/> class.
    /// </summary>
    public class AddressEfRepositoryTest
    {
        private readonly AddressEfRepository repository;

        public AddressEfRepositoryTest()
        {
            repository = new AddressEfRepository(new AddressEfDbContext(), new AddressRepositoryDataMapper());
        }

        [Fact]
        public async Task GetByName_ReturnsAddress_WhenCalled()
        {
            var address = new Address(0, "Golf course 4", "567 Main course", null, "Atlanta", "GA", "30102", "USA");
            await repository.Insert(address, CancellationToken.None).ConfigureAwait(false);
            await repository.Context.SaveChangesAsync().ConfigureAwait(false);

            var foundAddress = repository.GetByName(address.Name, CancellationToken.None).ConfigureAwait(false);
            foundAddress.Should().NotBeNull();
        }
    }
}