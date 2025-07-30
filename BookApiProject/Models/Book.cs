/// <summary>
/// Represents a book in the system.
/// </summary>
public class Book
{
    /// <summary>
    /// Unique identifier of the book.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Title of the book.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Genre or category of the book.
    /// </summary>
    public string Genre { get; set; } = string.Empty;

    /// <summary>
    /// ID of the author who wrote the book.
    /// </summary>
    public int AuthorId { get; set; }
}