/// <summary>
/// Represents an application user.
/// </summary>
public class User
{
    /// <summary>
    /// Unique identifier of the user.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Username of the user (used for login).
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Email address of the user.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Hashed password for authentication.
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Date of birth of the user.
    /// </summary>
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// Role assigned to the user (e.g., User, Admin).
    /// Default role is "User".
    /// </summary>
    public string Role { get; set; } = "User";
}