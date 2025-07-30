using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyApp.Exceptions;

/// <summary>
/// Exception filter for handling book-related exceptions.
/// Maps domain-specific book errors to appropriate HTTP responses.
/// </summary>
public class BookExceptionFilter : IExceptionFilter
{
    private readonly ILogger<BookExceptionFilter> _logger;

    public BookExceptionFilter(ILogger<BookExceptionFilter> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Handles exceptions thrown during book operations.
    /// </summary>
    public void OnException(ExceptionContext context)
    {
        // Log the exception
        _logger.LogError(context.Exception, "Error in BookController");

        // Handle case when the book was not found
        if (context.Exception is BookNotFoundException)
        {
            context.Result = new NotFoundObjectResult(new
            {
                error = "Book not found.",
                detail = context.Exception.Message
            })
            {
                StatusCode = 404
            };
        }
        // Handle unexpected book-related errors
        else
        {
            context.Result = new ObjectResult(new
            {
                error = "An error occurred while processing the book.",
                detail = context.Exception.Message
            })
            {
                StatusCode = 500
            };
        }

        context.ExceptionHandled = true;
    }
}