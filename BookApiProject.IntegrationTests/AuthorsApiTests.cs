using System.Net.Http.Json;
using BookApiProject.AuthorDTOs;
using System.Net.Http.Headers;
using System.Net;


namespace BookApiProject.IntegrationTests;

public class AuthorsApiTests : IntegrationTestBase
{
    public AuthorsApiTests(CustomWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task GetAllAuthors_ReturnsSeededData()
    {
        await ResetDatabaseAsync();

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
    public async Task GetAuthorById_ReturnsSeededData()
    {
        await ResetDatabaseAsync();

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
    public async Task CreateAuthor_ReturnsCreatedAuthor()
    {
        await ResetDatabaseAsync();

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
    public async Task UpdateAuthor_ReturnsNoContent()
    {
        await ResetDatabaseAsync();
        
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
    public async Task DeleteAuthor_ReturnsNoContent()
    {
        await ResetDatabaseAsync();

        int testId = 1;

        var token = GenerateAdminJwt();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync("/api/authors/" + testId);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var db = GetDbContext();
        Assert.False(db.Authors.Any(a => a.Id == testId));
    }
}
