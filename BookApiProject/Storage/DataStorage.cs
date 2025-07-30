using BCrypt.Net;

/// <summary>
/// Static in-memory data storage for Authors, Books, and Users.
/// Used as a simple data source instead of a database.
/// </summary>
public static class DataStorage
{
    /// <summary>
    /// List of authors available in the system.
    /// </summary>
    public static List<Author> Authors = new List<Author>
    {
        new Author
        {
            Id = 1,
            FullName = "George Orwell",
            Bio = "Author of 1984 and Animal Farm",
            DateOfBirth = new DateTime(1903, 6, 25)
        },
        new Author
        {
            Id = 2,
            FullName = "Harper Lee",
            Bio = "Author of To Kill a Mockingbird",
            DateOfBirth = new DateTime(1926, 4, 28)
        }
    };

    /// <summary>
    /// List of books available in the system.
    /// </summary>
    public static List<Book> Books = new List<Book>
    {
        new Book { Id = 1, Title = "1984", Genre = "Dystopian", AuthorId = 1 },
        new Book { Id = 2, Title = "To Kill a Mockingbird", Genre = "Classic", AuthorId = 2 }
    };

    /// <summary>
    /// List of users registered in the system.
    /// Includes a default admin user with a hashed password.
    /// </summary>
    public static List<User> Users = new List<User>
    {
        new User
        {
            Id = 1,
            Username = "admin",
            Email = "admin@gmail.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
            DateOfBirth = new DateTime(2005, 8, 21),
            Role = "Admin"
        }
    };
}
