using FluentAssertions;
using Tql.Agdata.NamedAddress.Domain.SeedWork;
using Xunit;

namespace Tql.Agdata.NamedAddress.Test.Domain.SeedWork;

/// <summary>
/// Tests the <see cref="Result{TData}" /> class.
/// </summary>
public class ResultTest
{
    [Fact]
    public void Result_CreatesNew_WhenInstantiated()
    {
        var result = new Result<int>
        {
            IsSuccessful = true,
            Message = "Test",
            Description = "Testing",
            Data = 1
        };

        result.IsSuccessful.Should().BeTrue();
        result.Message.Should().Be("Test");
        result.Description.Should().Be("Testing");
        result.Errors.Count.Should().Be(0);
        result.Data.Should().Be(1);
    }
}