using FluentAssertions;
using Reapit.Services.Demo.Common.Temporal;

namespace Reapit.Services.Demo.Common.Test.Temporal;

public class DateTimeOffsetProviderTests
{
    [Fact]
    public void Now_ReturnsCurrentTime_WhenNoContextConfigured()
        => DateTimeOffsetProvider.Now.Should().BeCloseTo(DateTimeOffset.Now, TimeSpan.FromMilliseconds(3));
    
    [Fact]
    public void Now_ReturnsConfiguredTime_WhenContextConfigured()
    {
        var fixedTimestamp = new DateTimeOffset(2016, 4, 16, 7, 53, 14, TimeSpan.FromHours(-5));
        using var ambientContext = new DateTimeOffsetProviderContext(fixedTimestamp);
        DateTimeOffsetProvider.Now.Should().Be(fixedTimestamp);
    }
    
    [Fact]
    public void Now_OnlyAppliesToScope_WhenContextConfiguredInUsingContext()
    {
        var fixedTimestamp = new DateTimeOffset(2016, 4, 16, 7, 53, 14, TimeSpan.FromHours(-5));
        
        using (var _ = new DateTimeOffsetProviderContext(fixedTimestamp))
        {
            DateTimeOffsetProvider.Now.Should().Be(fixedTimestamp);
        }
        
        DateTimeOffsetProvider.Now.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMilliseconds(3));
    }
}