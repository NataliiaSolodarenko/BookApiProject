/// <summary>
/// Defines authentication and user account management operations.
/// </summary>
public interface IAuthService
{
    string? Login(LoginDto loginInfo);
    bool Register(RegisterDto registerInfo);
    bool DeleteUserWithUsername(DeleteUserWithUsernameDto userDeleteInfo);
    bool DeleteUserWithEmail(DeleteUserWithEmailDto userDeleteInfo);
}