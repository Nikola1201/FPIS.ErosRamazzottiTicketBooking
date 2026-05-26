using FPIS.Domain.Models;
using Xunit;

namespace FPIS.Domain.Tests;

public class SolutionWiringTests
{
    [Fact]
    public void NewConcert_HasEmptyGuidId()
    {
        var concert = new Concert();
        Assert.Equal(Guid.Empty, concert.Id);
    }
}
