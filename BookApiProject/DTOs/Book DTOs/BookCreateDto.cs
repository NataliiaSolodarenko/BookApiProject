using System.ComponentModel.DataAnnotations;

/// <summary>
/// DTO for creating a new book.
/// </summary>
public class BookCreateDto
{
    /// <summary>
    /// Title of the book (max 200 characters).
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Genre of the book (max 100 characters).
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Genre { get; set; } = string.Empty;

    /// <summary>
    /// ID of the author who wrote the book.
    /// </summary>
    [Required]
    public int AuthorId { get; set; }
}