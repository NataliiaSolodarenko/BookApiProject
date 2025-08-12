using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using BookApiProject.Exceptions;

namespace BookApiProject.ExceptionFilters;

/// <summary>
/// Exception filter for handling authentication-related exceptions.
/// Catches specific authentication errors and returns appropriate HTTP responses.
/// </summary>
public class AuthExceptionFilter : IExceptionFilter
{
    private readonly ILogger<AuthExceptionFilter> _logger;

    public AuthExceptionFilter(ILogger<AuthExceptionFilter> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Handles exceptions thrown during authentication operations.
    /// </summary>
    public void OnException(ExceptionContext context)
    {
        // Log the exception with error level
        _logger.LogError(context.Exception, "Error in AuthController");

        // Handle incorrect password
        if (context.Exception is PasswordIncorrect)
        {
            context.Result = new UnauthorizedObjectResult(new
            {
                error = "Incorrect password.",
                detail = context.Exception.Message
            });
        }
        // Handle case when username does not exist
        else if (context.Exception is UserWithUsernameDoesNotExist)
        {
            context.Result = new UnauthorizedObjectResult(new
            {
                error = "Incorrect username.",
                detail = context.Exception.Message
            });
        }
        // Handle case when email does not exist
        else if (context.Exception is UserWithEmailDoesNotExist)
        {
            context.Result = new UnauthorizedObjectResult(new
            {
                error = "Incorrect email.",
                detail = context.Exception.Message
            });
        }
        // Handle case when email is already in use
        else if (context.Exception is EmailIsAlreadyInUse)
        {
            context.Result = new ConflictObjectResult(new
            {
                error = "Email already in use.",
                detail = context.Exception.Message
            });
        }
        // Handle case when username is already in use
        else if (context.Exception is UsernameIsAlreadyInUse)
        {
            context.Result = new ConflictObjectResult(new
            {
                error = "Username already in use.",
                detail = context.Exception.Message
            });
        }
        // Handle unexpected authentication-related errors
        else
        {
            context.Result = new ObjectResult(new
            {
                error = "An error occurred during authentication."
            })
            {
                StatusCode = 500
            };
        }

        // Mark exception as handled
        context.ExceptionHandled = true;
    }
}