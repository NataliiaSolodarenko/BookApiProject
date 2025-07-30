/// <summary>
/// Represents an author in the system.
/// </summary>
public class Author
{
    /// <summary>
    /// Unique identifier of the author.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Full name of the author.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Optional biography of the author.
    /// </summary>
    public string? Bio { get; set; }

    /// <summary>
    /// Date of birth of the author.
    /// </summary>
    public DateTime DateOfBirth { get; set; }
}