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
    /// First name of the author.
    /// </summary>
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// Last name of the author.
    /// </summary>
    public string LastName { get; set; } = null!;

    /// <summary>
    /// Short biography of the author.
    /// </summary>
    public string Bio { get; set; } = string.Empty;

    /// <summary>
    /// Date of birth of the author.
    /// </summary>
    public DateOnly BirthDate { get; set; }
}