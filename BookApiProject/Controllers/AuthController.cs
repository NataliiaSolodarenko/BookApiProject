using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[TypeFilter(typeof(AuthExceptionFilter))]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Authenticates a user and returns a JWT token.
    /// </summary>
    /// <param name="loginInfo">Login credentials (username and password).</param>
    /// <response code="200">Returns a valid JWT token.</response>
    /// <response code="401">Invalid username or password.</response>
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginInfo)
    {
        var token = await _authService.LoginAsync(loginInfo);
        return Ok(new { Token = token });
    }

    /// <summary>
    /// Registers a new user account.
    /// </summary>
    /// <param name="registerInfo">User registration details.</param>
    /// <response code="200">Registration successful.</response>
    /// <response code="409">Username or email already in use.</response>
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto registerInfo)
    {
        await _authService.RegisterAsync(registerInfo);
        return Ok("Register successful");
    }

    /// <summary>
    /// Deletes a user by username (requires password confirmation).
    /// </summary>
    /// <param name="userDeleteInfo">Username and password for deletion confirmation.</param>
    /// <response code="200">User deleted successfully.</response>
    /// <response code="401">Invalid username or password.</response>
    [HttpPost("deleteWithUsername")]
    public async Task<IActionResult> DeleteUserWithUsernameAsync([FromBody] DeleteUserWithUsernameDto userDeleteInfo)
    {
        await _authService.DeleteUserWithUsernameAsync(userDeleteInfo);
        return Ok("Deletion successful");
    }

    /// <summary>
    /// Deletes a user by email.
    /// </summary>
    /// <param name="userDeleteInfo">Email of the user to delete.</param>
    /// <response code="200">User deleted successfully.</response>
    /// <response code="404">User with this email does not exist.</response>
    [HttpPost("deleteWithEmail")]
    public async Task<IActionResult> DeleteUserWithEmailAsync([FromBody] DeleteUserWithEmailDto userDeleteInfo)
    {
        await _authService.DeleteUserWithEmailAsync(userDeleteInfo);
        return Ok("Deletion successful");
    }
}
