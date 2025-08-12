using System.ComponentModel.DataAnnotations;

namespace BookApiProject.AuthDTOs;

/// <summary>
/// DTO for deleting a user by email.
/// </summary>
public class DeleteUserWithEmailDto
{
    /// <summary>
    /// Email of the user to delete.
    /// </summary>
    [Required]
    public string Email { get; set; } = string.Empty;
}