using FluentAssertions;
using Reapit.Services.Demo.Common.Identifier;

namespace Reapit.Services.Demo.Common.Test.Identifier;

public class GuidProviderTests
{
    [Fact]
    public void Now_ReturnsNewGuid_WhenNoContextConfigured()
        => GuidProvider.New.Should().NotBeEmpty();
    
    [Fact]
    public void Now_ReturnsConfiguredTime_WhenContextConfigured()
    {
        var fixedGuid = new Guid("7e0c1088-c1ef-4eff-8711-5638e9d99f4f");
        using var ambientContext = new GuidProviderContext(fixedGuid);
        GuidProvider.New.Should().Be(fixedGuid);
    }
    
    [Fact]
    public void Now_OnlyAppliesToScope_WhenContextConfiguredInUsingContext()
    {
        var fixedGuid = new Guid("2d4efedc-dfa7-49fa-8d96-00e486059f71");
        using (var ambientContext = new GuidProviderContext(fixedGuid))
        {
            GuidProvider.New.Should().Be(fixedGuid);            
        }
        
        GuidProvider.New.Should().NotBe(fixedGuid);
    }
}