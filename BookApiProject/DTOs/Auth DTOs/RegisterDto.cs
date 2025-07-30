using System.ComponentModel.DataAnnotations;

/// <summary>
/// DTO for user registration.
/// </summary>
public class RegisterDto
{
    /// <summary>
    /// Desired username.
    /// </summary>
    [Required]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// User's email address.
    /// </summary>
    [Required]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Password for the account (minimum 6 characters).
    /// </summary>
    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// User's date of birth.
    /// </summary>
    public DateTime DateOfBirth { get; set; }
}