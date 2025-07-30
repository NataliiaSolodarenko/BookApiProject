/// <summary>
/// DTO for reading author details.
/// </summary>
public class AuthorReadDto
{
    /// <summary>
    /// ID of the author.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Full name of the author.
    /// </summary>
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