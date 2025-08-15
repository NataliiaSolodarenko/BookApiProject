using System.Net.Http.Json;
using BookApiProject.AuthorDTOs;
using System.Net.Http.Headers;
using System.Net;


namespace BookApiProject.IntegrationTests;

public class GetAuthorsApiTests : IntegrationTestBase
{
    public GetAuthorsApiTests(CustomWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task GetAllAuthors_WhenDatabaseIsFull_ReturnsSeededData()
    {
        await ResetDatabaseAsync(seedAuthors: true);

        var authors = await _client.GetFromJsonAsync<List<AuthorReadDto>>("/api/authors");
        Assert.NotNull(authors);
        Assert.Equal(2, authors.Count);
        Assert.Contains(authors, a => a.FirstName == "Anna");
        Assert.Contains(authors, a => a.LastName == "Jackson");
        Assert.Contains(authors, a => a.BirthDate == new DateOnly(1976, 2, 13));
        Assert.Contains(authors, a => a.Bio == "New author test");
        Assert.Contains(authors, a => a.FirstName == "Tom");
        Assert.Contains(authors, a => a.LastName == "Holland");
        Assert.Contains(authors, a => a.BirthDate == new DateOnly(1987, 8, 4));
        Assert.Contains(authors, a => a.Bio == "New test author");
    }

    [Fact]
    public async Task GetAllAuthors_WhenDatabaseIsEmpty_ReturnsEmptyList()
    {
        await ResetDatabaseAsync();

        var response = await _client.GetAsync("/api/authors");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var authors = await response.Content.ReadFromJsonAsync<List<AuthorReadDto>>();
        Assert.NotNull(authors);
        Assert.Empty(authors);
    }

    [Fact]
    public async Task GetAuthorById_WhenAuthorExist_ReturnsSeededData()
    {
        await ResetDatabaseAsync(seedAuthors: true);

        int testId = 1;

        var author = await _client.GetFromJsonAsync<AuthorReadDto>("/api/authors/" + testId);
        Assert.NotNull(author);
        Assert.Equal(testId, author.Id);
        Assert.Equal("Anna", author.FirstName);
        Assert.Equal("Jackson", author.LastName);
        Assert.Equal(new DateOnly(1976, 2, 13), author.BirthDate);
        Assert.Equal("New author test", author.Bio);
    }

    [Fact]
    public async Task GetAuthorById_WhenAuthorIdIsInvalid_ReturnsBadRequest()
    {
        await ResetDatabaseAsync(seedAuthors: true);

        int testId = -1;

        var response = await _client.GetAsync("/api/authors/" + testId);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var errorMessage = await response.Content.ReadAsStringAsync();
        Assert.Equal("Id must be 0 or greater.", errorMessage);
    }

    [Fact]
    public async Task GetAuthorById_WhenAuthorNotFound_ReturnsNotFound()
    {
        await ResetDatabaseAsync(seedAuthors: true);

        int testId = 10;

        var response = await _client.GetAsync("/api/authors/" + testId);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        Assert.NotNull(error);
        Assert.Equal("Author not found.", error["error"]);
        Assert.Equal($"Author with ID {testId} not found.", error["detail"]);
    }
}