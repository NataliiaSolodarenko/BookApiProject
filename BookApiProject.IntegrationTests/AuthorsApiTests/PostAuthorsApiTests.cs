using System.Net.Http.Json;
using BookApiProject.AuthorDTOs;
using System.Net.Http.Headers;
using System.Net;


namespace BookApiProject.IntegrationTests;

public class PostAuthorsApiTests : IntegrationTestBase
{
    public PostAuthorsApiTests(CustomWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task CreateAuthor_ReturnsCreatedAuthor()
    {
        await ResetDatabaseAsync(seedAuthors: true);

        var newAuthor = new AuthorCreateDto
        {
            FirstName = "Lina",
            LastName = "Kostenko",
            BirthDate = new DateOnly(1930, 3, 19),
            Bio = "Ukrainian poet and dissident"
        };

        var response = await _client.PostAsJsonAsync("/api/authors", newAuthor);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var createdAuthor = await response.Content.ReadFromJsonAsync<AuthorReadDto>();
        Assert.NotNull(createdAuthor);
        Assert.Equal(3, createdAuthor.Id);
        Assert.Equal("Lina", createdAuthor.FirstName);
        Assert.Equal("Kostenko", createdAuthor.LastName);
        Assert.Equal(new DateOnly(1930, 3, 19), createdAuthor.BirthDate);
        Assert.Equal("Ukrainian poet and dissident", createdAuthor.Bio);
    }

    [Fact]
    public async Task CreateAuthor_WhenUnrequiredParametersAreEmpty_ReturnsCreatedAuthor()
    {
        await ResetDatabaseAsync(seedAuthors: true);

        var newAuthor = new AuthorCreateDto
        {
            FirstName = "Lina",
            LastName = "Kostenko"
        };

        var response = await _client.PostAsJsonAsync("/api/authors", newAuthor);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var createdAuthor = await response.Content.ReadFromJsonAsync<AuthorReadDto>();
        Assert.NotNull(createdAuthor);
        Assert.Equal(3, createdAuthor.Id);
        Assert.Equal("Lina", createdAuthor.FirstName);
        Assert.Equal("Kostenko", createdAuthor.LastName);
        Assert.Equal(DateOnly.MinValue, createdAuthor.BirthDate);
        Assert.Equal("", createdAuthor.Bio);
    }

    [Fact]
    public async Task CreateAuthor_WhenFirstNameIsNull_ReturnsBadRequest()
    {
        await ResetDatabaseAsync(seedAuthors: true);

        var newAuthor = new AuthorCreateDto
        {
            LastName = "Kostenko"
        };

        var response = await _client.PostAsJsonAsync("/api/authors", newAuthor);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateAuthor_WhenLastNameIsNull_ReturnsBadRequest()
    {
        await ResetDatabaseAsync(seedAuthors: true);

        var newAuthor = new AuthorCreateDto
        {
            FirstName = "Lina"
        };

        var response = await _client.PostAsJsonAsync("/api/authors", newAuthor);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateAuthor_WhenAuthorIsNull_ReturnsBadRequest()
    {
        await ResetDatabaseAsync(seedAuthors: true);

        var newAuthor = new AuthorCreateDto();

        var response = await _client.PostAsJsonAsync("/api/authors", newAuthor);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}