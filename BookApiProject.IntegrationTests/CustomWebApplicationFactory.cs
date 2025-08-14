using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BookApiProject;
using BookApiProject.Models;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;


public class CustomWebApplicationFactory : WebApplicationFactory<EntryPoint>
{
    private SqliteConnection? _connection;
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("IntegrationTest");

        builder.ConfigureServices(services =>
        {
            services.PostConfigure<JwtBearerOptions>("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes("3mkd6ndkfyt5mdhhgjt3jks856hhdbnf245njd"))
                };
            });
            
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<BookDbContext>));
            if (dbContextDescriptor != null)
            {
                services.Remove(dbContextDescriptor);
            }

            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            services.AddDbContext<BookDbContext>(options =>
            {
                options.UseSqlite(_connection);
            });

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<BookDbContext>();
            db.Database.EnsureCreated();

            SeedTestData(db);
        });
    }

    private void SeedTestData(BookDbContext db)
    {
        db.Authors.AddRange(
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
        );
        db.SaveChanges();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            _connection?.Dispose();
        }
    }
}