using System.Net.Http.Json;
using BookApiProject.AuthorDTOs;
using System.Net.Http.Headers;
using System.Net;


namespace BookApiProject.IntegrationTests;

public class PutAuthorsApiTests : IntegrationTestBase
{
    public PutAuthorsApiTests(CustomWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task UpdateAuthor_WhenAuthorExist_ReturnsNoContent()
    {
        await ResetDatabaseAsync(seedAuthors: true);

        int testId = 1;
        var updatedAuthor = new AuthorUpdateDto
        {
            FirstName = "UpdateAnna",
            LastName = "UpdateJackson",
            BirthDate = new DateOnly(1950, 4, 10),
            Bio = "Update author test"
        };

        var response = await _client.PutAsJsonAsync("/api/authors/" + testId, updatedAuthor);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var db = GetDbContext();

        var author = await db.Authors.FindAsync(testId);
        Assert.NotNull(author);
        Assert.Equal("UpdateAnna", author.FirstName);
        Assert.Equal("UpdateJackson", author.LastName);
        Assert.Equal(new DateOnly(1950, 4, 10), author.BirthDate);
        Assert.Equal("Update author test", author.Bio);
    }

    [Fact]
    public async Task UpdateAuthor_WhenUnrequiredParametersAreEmpty_ReturnsNoContent()
    {
        await ResetDatabaseAsync(seedAuthors: true);

        int testId = 1;
        var updatedAuthor = new AuthorUpdateDto
        {
            FirstName = "UpdateAnna",
            LastName = "UpdateJackson"
        };

        var response = await _client.PutAsJsonAsync("/api/authors/" + testId, updatedAuthor);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var db = GetDbContext();

        var author = await db.Authors.FindAsync(testId);
        Assert.NotNull(author);
        Assert.Equal("UpdateAnna", author.FirstName);
        Assert.Equal("UpdateJackson", author.LastName);
        Assert.Equal(DateOnly.MinValue, author.BirthDate);
        Assert.Equal("", author.Bio);
    }

    [Fact]
    public async Task UpdateAuthor_WhenAuthorNotFound_ReturnsNotFound()
    {
        await ResetDatabaseAsync(seedAuthors: true);

        int testId = 10;
        var updatedAuthor = new AuthorUpdateDto
        {
            FirstName = "UpdateAnna",
            LastName = "UpdateJackson"
        };

        var response = await _client.PutAsJsonAsync("/api/authors/" + testId, updatedAuthor);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        Assert.NotNull(error);
        Assert.Equal("Author not found.", error["error"]);
        Assert.Equal($"Author with ID {testId} not found.", error["detail"]);
    }

    [Fact]
    public async Task UpdateAuthor_WhenAuthorIdIsInvalid_ReturnsBadRequest()
    {
        await ResetDatabaseAsync(seedAuthors: true);

        int testId = -1;
        var updatedAuthor = new AuthorUpdateDto
        {
            FirstName = "UpdateAnna",
            LastName = "UpdateJackson"
        };

        var response = await _client.PutAsJsonAsync("/api/authors/" + testId, updatedAuthor);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var errorMessage = await response.Content.ReadAsStringAsync();
        Assert.Equal("Id must be 0 or greater.", errorMessage);
    }

    [Fact]
    public async Task UpdateAuthor_WhenUpdatedFirstNameIsNull_ReturnsBadRequest()
    {
        await ResetDatabaseAsync(seedAuthors: true);
        
        int testId = 1;
        var updatedAuthor = new AuthorUpdateDto
        {
            LastName = "UpdateJackson"
        };

        var response = await _client.PutAsJsonAsync("/api/authors/" + testId, updatedAuthor);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateAuthor_WhenUpdatedLastNameIsNull_ReturnsBadRequest()
    {
        await ResetDatabaseAsync(seedAuthors: true);
        
        int testId = 1;
        var updatedAuthor = new AuthorUpdateDto
        {
            FirstName = "UpdateAnna"
        };

        var response = await _client.PutAsJsonAsync("/api/authors/" + testId, updatedAuthor);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateAuthor_WhenUpdatedAuthorIsNull_ReturnsBadRequest()
    {
        await ResetDatabaseAsync(seedAuthors: true);
        
        int testId = 1;
        var updatedAuthor = new AuthorUpdateDto ();

        var response = await _client.PutAsJsonAsync("/api/authors/" + testId, updatedAuthor);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
