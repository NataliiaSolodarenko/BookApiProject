using System.ComponentModel.DataAnnotations;

namespace BookApiProject.AuthorDTOs;

/// <summary>
/// DTO for creating a new author.
/// </summary>
public class AuthorCreateDto
{
    /// <summary>
    /// First name of the author.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// Last name of the author.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = null!;

    ///
    /// <summary>
    /// Short biography of the author.
    /// </summary>
    public string Bio { get; set; } = string.Empty;

    /// <summary>
    /// Date of birth of the author.
    /// </summary>
    public DateOnly BirthDate { get; set; }
}