using System.ComponentModel.DataAnnotations;

/// <summary>
/// DTO for user login.
/// </summary>
public class LoginDto
{
    /// <summary>
    /// Username used for login.
    /// </summary>
    [Required]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Password used for login.
    /// </summary>
    [Required]
    public string Password { get; set; } = string.Empty;
}