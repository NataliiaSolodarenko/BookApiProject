using System.Net.Http.Json;
using BookApiProject.AuthorDTOs;
using System.Net.Http.Headers;
using System.Net;


namespace BookApiProject.IntegrationTests;

public class DeleteAuthorsApiTests : IntegrationTestBase
{
    public DeleteAuthorsApiTests(CustomWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task DeleteAuthor_WhenAuthorExist_ReturnsNoContent()
    {
        await ResetDatabaseAsync(seedAuthors: true);

        int testId = 1;

        var token = GenerateAdminJwt();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync("/api/authors/" + testId);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var db = GetDbContext();
        Assert.False(db.Authors.Any(a => a.Id == testId));
    }

    [Fact]
    public async Task DeleteAuthor_WhenUserJwt_ReturnsNoContent()
    {
        await ResetDatabaseAsync(seedAuthors: true);

        int testId = 1;

        var token = GenerateUserJwt();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync("/api/authors/" + testId);
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);

        var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        Assert.NotNull(error);
        Assert.Equal("Forbidden", error["error"]);
        Assert.Equal("You do not have permission to access this resource.", error["detail"]);
    }

    [Fact]
    public async Task DeleteAuthor_WhenModeratorJwt_ReturnsNoContent()
    {
        await ResetDatabaseAsync(seedAuthors: true);

        int testId = 1;

        var token = GenerateModeratorJwt();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync("/api/authors/" + testId);
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);

        var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        Assert.NotNull(error);
        Assert.Equal("Forbidden", error["error"]);
        Assert.Equal("You do not have permission to access this resource.", error["detail"]);
    }

    [Fact]
    public async Task DeleteAuthor_WhenNoJwt_ReturnsNoContent()
    {
        await ResetDatabaseAsync(seedAuthors: true);

        int testId = 1;

        var response = await _client.DeleteAsync("/api/authors/" + testId);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task DeleteAuthor_WhenAuthorNotFound_ReturnsNoContent()
    {
        await ResetDatabaseAsync(seedAuthors: true);

        int testId = 10;

        var token = GenerateAdminJwt();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync("/api/authors/" + testId);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        Assert.NotNull(error);
        Assert.Equal("Author not found.", error["error"]);
        Assert.Equal($"Author with ID {testId} not found.", error["detail"]);
    }

    [Fact]
    public async Task DeleteAuthor_WhenAuthorIdIsInvalid_ReturnsNoContent()
    {
        await ResetDatabaseAsync(seedAuthors: true);

        int testId = -1;

        var token = GenerateAdminJwt();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync("/api/authors/" + testId);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var errorMessage = await response.Content.ReadAsStringAsync();
        Assert.Equal("Id must be 0 or greater.", errorMessage);
    }
}