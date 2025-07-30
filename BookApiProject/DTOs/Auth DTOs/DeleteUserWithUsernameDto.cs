using System.ComponentModel.DataAnnotations;

/// <summary>
/// DTO for deleting a user by username, requiring password confirmation.
/// </summary>
public class DeleteUserWithUsernameDto
{
    /// <summary>
    /// Username of the user to delete.
    /// </summary>
    [Required]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Password for confirming the deletion.
    /// </summary>
    [Required]
    public string Password { get; set; } = string.Empty;
}