using Microsoft.Extensions.DependencyInjection;
using BookApiProject.Models;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;


public abstract class IntegrationTestBase : IClassFixture<CustomWebApplicationFactory>
{
    protected readonly HttpClient _client;
    protected readonly CustomWebApplicationFactory _factory;

    protected IntegrationTestBase(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    protected BookDbContext GetDbContext() =>
        _factory.Services.CreateScope().ServiceProvider.GetRequiredService<BookDbContext>();

    protected string GenerateAdminJwt()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "TestAdmin"),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("3mkd6ndkfyt5mdhhgjt3jks856hhdbnf245njd"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    protected string GenerateUserJwt()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "TestUser"),
            new Claim(ClaimTypes.Role, "User")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("3mkd6ndkfyt5mdhhgjt3jks856hhdbnf245njd"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    protected string GenerateModeratorJwt()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "TesModerator"),
            new Claim(ClaimTypes.Role, "Moderator")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("3mkd6ndkfyt5mdhhgjt3jks856hhdbnf245njd"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    protected async Task SeedAuthorsAsync(BookDbContext db)
    {
        var authors = new[]
        {
            new Author
            {
                FirstName = "Anna",
                LastName = "Jackson",
                BirthDate = new DateOnly(1976, 2, 13),
                Bio = "New author test"
            },
            new Author
            {
                FirstName = "Tom",
                LastName = "Holland",
                BirthDate = new DateOnly(1987, 8, 4),
                Bio = "New test author"
            }
        };

        db.Authors.AddRange(authors);
        await db.SaveChangesAsync();
    }

    protected async Task ResetDatabaseAsync(bool seedAuthors = false, bool seedBooks = false)
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<BookDbContext>();

        await db.Database.EnsureDeletedAsync();
        await db.Database.EnsureCreatedAsync();

        if (seedAuthors)
        await SeedAuthorsAsync(db);

        //if (seedBooks)
            //await SeedBooksAsync(db);
    }

}