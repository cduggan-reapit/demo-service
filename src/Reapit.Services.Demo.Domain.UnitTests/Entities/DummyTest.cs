using FluentAssertions;
using Reapit.Services.Demo.Common.Temporal;
using Reapit.Services.Demo.Domain.Entities;

namespace Reapit.Services.Demo.Domain.UnitTests.Entities;

public class DummyTest
{
    [Fact]
    public void Ctor_InitializesEntity_WithLanguageDefaults()
    {
        var sut = new Dummy();

        sut.Id.Should().Be(default(Guid));
        sut.Name.Should().Be(default!);
        sut.DateCreated.Should().Be(default);
        sut.DateModified.Should().Be(default);
    }
    
    [Fact]
    public void Ctor_InitializesEntity_WithApplicationDefaults_WhenNameProvided()
    {
        const string name = "dummy name";
        var date = new DateTimeOffset(2024, 09, 03, 13, 08, 51, TimeSpan.FromHours(1));
        using var dateTimeProvider = new DateTimeOffsetProviderContext(date);
        
        var sut = new Dummy(name);

        sut.Id.Should().Be(default(Guid));
        sut.Name.Should().Be(name);
        sut.DateCreated.Should().Be(date.UtcDateTime);
        sut.DateModified.Should().Be(date.UtcDateTime);
    }
}