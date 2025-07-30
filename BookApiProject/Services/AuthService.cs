using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using MyApp.Exceptions;

/// <summary>
/// Service for authentication and user management.
/// Handles login, registration, and user deletion.
/// </summary>
public class AuthService : IAuthService
{
    private readonly IMapper _mapper;
    private readonly JwtSettings _jwtSettings;

    public AuthService(IMapper mapper, IOptions<JwtSettings> jwtSettings)
    {
        _mapper = mapper;
        _jwtSettings = jwtSettings.Value;
    }

    /// <summary>
    /// Attempts to log in a user and returns a JWT token if successful.
    /// Throws exceptions for invalid username or password.
    /// </summary>
    public string? Login(LoginDto loginInfo)
    {
        var user = DataStorage.Users.FirstOrDefault(u => u.Username == loginInfo.Username);
        if (user == null)
            throw new UserWithUsernameDoesNotExist();

        if (!BCrypt.Net.BCrypt.Verify(loginInfo.Password, user.PasswordHash))
            throw new PasswordIncorrect();

        return GenerateToken(user);
    }

    /// <summary>
    /// Registers a new user.
    /// Throws exceptions if the username or email is already in use.
    /// </summary>
    public bool Register(RegisterDto registerInfo)
    {
        if (DataStorage.Users.Any(u => u.Username == registerInfo.Username))
            throw new UsernameIsAlreadyInUse();

        if (DataStorage.Users.Any(u => u.Email == registerInfo.Email))
            throw new EmailIsAlreadyInUse();

        var user = _mapper.Map<User>(registerInfo);
        user.Id = DataStorage.Users.Any()
            ? DataStorage.Users.Max(u => u.Id) + 1
            : 1;
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerInfo.Password);

        DataStorage.Users.Add(user);
        return true;
    }

    /// <summary>
    /// Deletes a user account by username.
    /// Requires the correct password for verification.
    /// </summary>
    public bool DeleteUserWithUsername(DeleteUserWithUsernameDto userDeleteInfo)
    {
        var user = DataStorage.Users.FirstOrDefault(u => u.Username == userDeleteInfo.Username);
        if (user == null)
            throw new UserWithUsernameDoesNotExist();

        if (!BCrypt.Net.BCrypt.Verify(userDeleteInfo.Password, user.PasswordHash))
            throw new PasswordIncorrect();

        DataStorage.Users.Remove(user);
        return true;
    }

    /// <summary>
    /// Deletes a user account by email.
    /// Does not require password verification.
    /// </summary>
    public bool DeleteUserWithEmail(DeleteUserWithEmailDto userDeleteInfo)
    {
        var user = DataStorage.Users.FirstOrDefault(u => u.Email == userDeleteInfo.Email);
        if (user == null)
            throw new UserWithEmailDoesNotExist();

        DataStorage.Users.Remove(user);
        return true;
    }

    /// <summary>
    /// Generates a JWT token for the authenticated user.
    /// </summary>
    private string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
