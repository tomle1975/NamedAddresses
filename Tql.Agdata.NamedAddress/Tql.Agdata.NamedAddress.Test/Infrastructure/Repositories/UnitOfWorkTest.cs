using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tql.Agdata.NamedAddress.Infrastructure.Repositories.EntityFramework;
using Xunit;

namespace Tql.Agdata.NamedAddress.Test.Infrastructure.Repositories;

/// <summary>
/// Tests the <see cref="EfUnitOfWork{TContext}" /> class.
/// </summary>
public class UnitOfWorkTest
{
    private readonly EfUnitOfWork<TestContext> unitOfWork = new();

    [Fact]
    public void CreateTransaction_SetsTransaction_WhenCreated()
    {
        unitOfWork.CreateTransaction(CancellationToken.None).ContinueWith(_ =>
        {
            unitOfWork.Context.Database.CurrentTransaction.Should().NotBeNull();
            unitOfWork.Context.Database.CurrentTransaction?.Rollback();
        });
    }

    [Fact]
    public void Commit_ChangesAreSaved_WhenCommitted()
    {
        unitOfWork.CreateTransaction(CancellationToken.None).ContinueWith(_ =>
        {
            unitOfWork.Context.Database.CurrentTransaction.Should().NotBeNull();
            unitOfWork.Commit(CancellationToken.None).ContinueWith(_ => { unitOfWork.Context.Database.CurrentTransaction.Should().BeNull(); });
        });
    }

    [Fact]
    public void Rollback_ChangesUndone_WhenRolledBack()
    {
        unitOfWork.CreateTransaction(CancellationToken.None).ContinueWith(_ =>
        {
            unitOfWork.Context.Database.CurrentTransaction.Should().NotBeNull();
            unitOfWork.Rollback(CancellationToken.None).ContinueWith(_ => { unitOfWork.Context.Database.CurrentTransaction.Should().BeNull(); });
        });
    }

    public class TestContext : DbContext
    {
    }
}