namespace BookApiProject.BookDTOs;

/// <summary>
/// DTO for reading book details.
/// </summary>
public class BookReadDto
{
    /// <summary>
    /// ID of the book.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Title of the book.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Genre of the book.
    /// </summary>
    public string Genre { get; set; } = string.Empty;

    /// <summary>
    /// ID of the author who wrote the book.
    /// </summary>
    public int AuthorId { get; set; }
}