using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BookApiProject.ValidationFilters;

/// <summary>
/// Action filter that validates the "id" parameter in a controller action.
/// Ensures that the ID value is not negative before the action executes.
/// </summary>
public class ValidateIdAttribute : ActionFilterAttribute
{
    /// <summary>
    /// Called before the action method is executed.
    /// Checks if the "id" argument exists and is greater than or equal to 0.
    /// If the ID is negative, returns a BadRequest response.
    /// </summary>
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Try to retrieve the "id" parameter from the action arguments
        if (context.ActionArguments.TryGetValue("id", out var idObj) && idObj is int id)
        {
            // Validate that the ID is not negative
            if (id < 0)
            {
                context.Result = new BadRequestObjectResult("Id must be 0 or greater.");
            }
        }
    }
}