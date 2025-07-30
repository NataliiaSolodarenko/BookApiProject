using System.ComponentModel.DataAnnotations;

/// <summary>
/// DTO for creating a new author.
/// </summary>
public class AuthorCreateDto
{
    /// <summary>
    /// Full name of the author (max 100 characters).
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Short biography of the author.
    /// </summary>
    public string Bio { get; set; } = string.Empty;

    /// <summary>
    /// Date of birth of the author.
    /// </summary>
    public DateTime DateOfBirth { get; set; }
}