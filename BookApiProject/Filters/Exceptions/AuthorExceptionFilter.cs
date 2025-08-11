using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using BookApiProject.Exceptions;

namespace BookApiProject.ExceptionFilters;

/// <summary>
/// Exception filter for handling author-related exceptions.
/// Maps domain-specific author errors to appropriate HTTP responses.
/// </summary>
public class AuthorExceptionFilter : IExceptionFilter
{
    private readonly ILogger<AuthorExceptionFilter> _logger;

    public AuthorExceptionFilter(ILogger<AuthorExceptionFilter> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Handles exceptions thrown during author operations.
    /// </summary>
    public void OnException(ExceptionContext context)
    {
        // Log the exception
        _logger.LogError(context.Exception, "Error in AuthorController");

        // Handle case when the author was not found
        if (context.Exception is AuthorNotFoundException)
        {
            context.Result = new NotFoundObjectResult(new
            {
                error = "Author not found.",
                detail = context.Exception.Message
            })
            {
                StatusCode = 404
            };
        }
        // Handle unexpected author-related errors
        else
        {
            context.Result = new ObjectResult(new
            {
                error = "An error occurred while processing the author.",
                detail = context.Exception.Message
            })
            {
                StatusCode = 500
            };
        }

        context.ExceptionHandled = true;
    }
}