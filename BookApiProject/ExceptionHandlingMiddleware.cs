namespace BookApiProject;

/// <summary>
/// Middleware for handling unhandled exceptions in the request pipeline.
/// Logs the error and returns a standardized JSON response.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionHandlingMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="logger">Logger for recording exception details.</param>
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invokes the middleware.
    /// Wraps the request processing in a try-catch to handle unhandled exceptions.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            // Continue request pipeline execution
            await _next(context);
        }
        catch (Exception ex)
        {
            // Log the unhandled exception
            _logger.LogError(ex, "An unprocessed error has occurred!");

            // Return standardized error response
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Generates and sends the error response to the client.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <param name="exception">The exception that occurred.</param>
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 501; // Not Implemented (used here as a generic error code)

        var response = new
        {
            error = "Something went wrong.",
            details = exception.Message
        };

        return context.Response.WriteAsJsonAsync(response);
    }
}
