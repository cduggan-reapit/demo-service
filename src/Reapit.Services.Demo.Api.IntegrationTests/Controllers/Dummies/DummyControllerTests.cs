using System.Net;
using System.Text;
using System.Text.Json;
using Reapit.Services.Demo.Api.Controllers.Dummies.Models;
using Reapit.Services.Demo.Data.Context;
using Reapit.Services.Demo.Domain.Entities;

namespace Reapit.Services.Demo.Api.IntegrationTests.Controllers.Dummies;

public class DummyControllerTests : IClassFixture<TestApiFactory>
{
    private readonly TestApiFactory _factory;

    public DummyControllerTests(TestApiFactory factory)
        => _factory = factory;
    
    /*
     * GET /api/dummy
     */

    [Fact]
    public async Task GetDummies_ReturnsForbidden_WhenRequiredScopeMissing()
    {
        const string url = "/api/dummy";
        var client = _factory.CreateClient();

        var response = await client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);

        var payload = await response.Content.ReadAsStringAsync();
        var actual = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(payload)
            ?? throw new Exception($"Failed to deserialize response: {payload}");
        
        actual["statusCode"].GetInt32().Should().Be(403);
        actual["dateTime"].GetDateTime().Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }
    
    [Fact]
    public async Task GetDummies_ReturnsOk_WhenRequestSuccessful()
    {
        await InitializeDatabaseAsync();
        
        const string url = "/api/dummy";
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("demo-scopes", "dummy.read");

        var response = await client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    /*
     * GET /api/dummy/{id}
     */
    
    [Fact]
    public async Task GetDummyById_ReturnsForbidden_WhenRequiredScopeMissing()
    {
        var url = $"/api/dummy/{Guid.NewGuid():N}";
        var client = _factory.CreateClient();

        var response = await client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task GetDummyById_ReturnsNotFound_WhenDummyNotFound()
    {
        await InitializeDatabaseAsync();
        
        var url = $"/api/dummy/{Guid.Empty}";
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("demo-scopes", "dummy.read");

        var response = await client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GetDummyById_ReturnsOk_WhenRequestSuccessful()
    {
        var dummy = SeedData.ElementAt(2);
        var expected = new ReadDummyModel($"{dummy.Id:N}", dummy.Name, dummy.DateCreated, dummy.DateModified);
        
        await InitializeDatabaseAsync();
        
        var url = $"/api/dummy/{dummy.Id}";
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("demo-scopes", "dummy.read");

        var response = await client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var actual = JsonSerializer.Deserialize<ReadDummyModel>(await response.Content.ReadAsStringAsync());
        actual.Should().BeEquivalentTo(expected);
    }
    
    /*
     * POST /api/dummy
     */
    
    [Fact]
    public async Task CreateDummy_ReturnsForbidden_WhenRequiredScopeMissing()
    {
        const string name = "name";
        var requestBody = new WriteDummyModel(name);
        
        const string url = "/api/dummy";
        var client = _factory.CreateClient();

        var response = await client.PostAsync(url, new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json"));
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task CreateDummy_ReturnsUnprocessable_WhenValidationFailed()
    {
        var name = new string('a', 101);
        var requestBody = new WriteDummyModel(name);
        
        const string url = "/api/dummy";
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("demo-scopes", "dummy.write");

        var response = await client.PostAsync(url, new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json"));
        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

        var payload = await response.Content.ReadAsStringAsync();
        var content = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(payload)
            ?? throw new Exception($"Failed to deserialize response: {payload}");

        content["statusCode"].GetInt32().Should().Be(422);
        content["errors"].EnumerateArray().Should().HaveCount(1);
    }
    
    [Theory]
    [InlineData("dummy.create")]
    [InlineData("dummy.write")]
    public async Task CreateDummy_ReturnsCreated_WhenRequestValid_AndAnyRequiredScopedProvided(string scope)
    {
        const string name = "people's postcode lottery";
        const string url = "/api/dummy";
        var requestBody = new WriteDummyModel(name);
        
        await InitializeDatabaseAsync();
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("demo-scopes", scope);

        var response = await client.PostAsync(url, new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json"));
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var actual = JsonSerializer.Deserialize<ReadDummyModel>(await response.Content.ReadAsStringAsync());
        actual!.Name.Should().Be(name);
    }
    
    /*
     * Private methods
     */

    private async Task InitializeDatabaseAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var dbContext = scopedServices.GetRequiredService<DemoDbContext>();

        // Make sure it's created
        _ = await dbContext.Database.EnsureCreatedAsync();
        
        // Make sure it's empty
        dbContext.Dummies.RemoveRange(dbContext.Dummies);
        
        // Populate with seed data
        await dbContext.Dummies.AddRangeAsync(SeedData);
        
        // Save the changes
        _ = await dbContext.SaveChangesAsync();
    }
    
    private static IEnumerable<Dummy> SeedData
        => new Dummy[]
        {
            new()
            {
                Id = new Guid("18b5f4ba-5c23-4b25-a771-ce21f14c1bfc"),
                Name = "Dummy One",
                DateCreated = new DateTime(2021, 1, 1, 1, 1, 1, DateTimeKind.Utc),
                DateModified = new DateTime(2022, 1, 1, 1, 1, 1, DateTimeKind.Utc)
            },
            new()
            {
                Id = new Guid("f432fe2b-d541-4968-8585-b80010377f4e"),
                Name = "Dummy Two",
                DateCreated = new DateTime(2021, 2, 2, 2, 2, 2, DateTimeKind.Utc),
                DateModified = new DateTime(2022, 2, 2, 2, 2, 2, DateTimeKind.Utc)
            },
            new()
            {
                Id = new Guid("c8cf7c01-7963-4f5d-b77e-368008d854eb"),
                Name = "Dummy Three",
                DateCreated = new DateTime(2021, 3, 3, 3, 3, 3, DateTimeKind.Utc),
                DateModified = new DateTime(2022, 3, 3, 3, 3, 3, DateTimeKind.Utc)
            },
            new()
            {
                Id = new Guid("6134b138-c346-4be1-bcb6-6c5615287fbe"),
                Name = "Dummy Four",
                DateCreated = new DateTime(2021, 4, 4, 4, 4, 4, DateTimeKind.Utc),
                DateModified = new DateTime(2022, 4, 4, 4, 4, 4, DateTimeKind.Utc)
            },
            new()
            {
                Id = new Guid("12c398d5-ee5b-4500-92d2-06f10dbec2b5"),
                Name = "Dummy Five",
                DateCreated = new DateTime(2021, 5, 5, 5, 5, 5, DateTimeKind.Utc),
                DateModified = new DateTime(2022, 5, 5, 5, 5, 5, DateTimeKind.Utc)
            }
        };
}