using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using BookApiProject.Models;
using Microsoft.EntityFrameworkCore;
using BookApiProject.AuthDTOs;
using BookApiProject.Exceptions;

namespace BookApiProject.Services;


/// <summary>
/// Service for authentication and user management.
/// Handles login, registration, and user deletion.
/// </summary>
public class AuthService : IAuthService
{
    private readonly BookDbContext _context;
    private readonly IMapper _mapper;
    private readonly JwtSettings _jwtSettings;

    public AuthService(BookDbContext context, IMapper mapper, IOptions<JwtSettings> jwtSettings)
    {
        _context = context;
        _mapper = mapper;
        _jwtSettings = jwtSettings.Value;
    }

    /// <summary>
    /// Attempts to log in a user and returns a JWT token if successful.
    /// Throws exceptions for invalid username or password.
    /// </summary>
    public async Task<string?> LoginAsync(LoginDto loginInfo)
    {
        var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Username == loginInfo.Username);
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
    public async Task<bool> RegisterAsync(RegisterDto registerInfo)
    {
        if (await _context.Users.AnyAsync(u => u.Username == registerInfo.Username))
            throw new UsernameIsAlreadyInUse();

        if (await _context.Users.AnyAsync(u => u.Email == registerInfo.Email))
            throw new EmailIsAlreadyInUse();

        var user = _mapper.Map<User>(registerInfo);
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerInfo.Password);
        user.RoleId = 3;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Deletes a user account by username.
    /// Requires the correct password for verification.
    /// </summary>
    public async Task<bool> DeleteUserWithUsernameAsync(DeleteUserWithUsernameDto userDeleteInfo)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userDeleteInfo.Username);
        if (user == null)
            throw new UserWithUsernameDoesNotExist();

        if (!BCrypt.Net.BCrypt.Verify(userDeleteInfo.Password, user.PasswordHash))
            throw new PasswordIncorrect();

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Deletes a user account by email.
    /// Does not require password verification.
    /// </summary>
    public async Task<bool> DeleteUserWithEmailAsync(DeleteUserWithEmailDto userDeleteInfo)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userDeleteInfo.Email);
        if (user == null)
            throw new UserWithEmailDoesNotExist();

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
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
                new Claim(ClaimTypes.Role, user.Role.RoleName)
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
