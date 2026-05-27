using FPIS.Domain.Mappings;
using FPIS.Domain.Models;
using Xunit;

namespace FPIS.Domain.Tests.Mappings;

public class ZoneMappingsTests
{
    [Fact]
    public void ToViewModel_CopiesAllFields()
    {
        var id = Guid.NewGuid();
        var zone = new Zone { Id = id, Name = "Standing", Capacity = 500, Price = 80m };

        var vm = zone.ToViewModel(capacityRemaining: 123);

        Assert.Equal(id, vm.Id);
        Assert.Equal("Standing", vm.Name);
        Assert.Equal(500, vm.Capacity);
        Assert.Equal(80m, vm.Price);
        Assert.Equal(123, vm.CapacityRemaining);
    }

    [Theory]
    [InlineData(int.MinValue)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(int.MaxValue)]
    public void ToViewModel_AcceptsBoundaryCapacityRemaining(int remaining)
    {
        var zone = new Zone { Capacity = 100 };
        var vm = zone.ToViewModel(remaining);
        Assert.Equal(remaining, vm.CapacityRemaining);
        Assert.Equal(100, vm.Capacity);
    }
}
