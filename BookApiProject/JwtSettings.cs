namespace BookApiProject;

/// <summary>
/// Configuration settings for JWT authentication.
/// Loaded from the "JwtSettings" section in appsettings.json.
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// The secret key used to sign JWT tokens.
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// The token issuer (e.g., the API name).
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// The expected token audience (e.g., the API consumers).
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Token expiration time in minutes.
    /// </summary>
    public int ExpiresInMinutes { get; set; }
}
