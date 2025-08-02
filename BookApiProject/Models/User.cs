using System;
using System.Collections.Generic;

namespace BookApiProject.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public DateOnly? DateOfBirth { get; set; }

    public int RoleId { get; set; }

    public virtual Role Role { get; set; } = null!;
}
