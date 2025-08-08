/// <summary>
/// Defines authentication and user account management operations.
/// </summary>
public interface IAuthService
{
    Task<string?> LoginAsync(LoginDto loginInfo);
    Task<bool> RegisterAsync(RegisterDto registerInfo);
    Task<bool> DeleteUserWithUsernameAsync(DeleteUserWithUsernameDto userDeleteInfo);
    Task<bool> DeleteUserWithEmailAsync(DeleteUserWithEmailDto userDeleteInfo);
}