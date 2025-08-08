using FluentValidation;

/// <summary>
/// Validator for <see cref="RegisterDto"/>.
/// Ensures that user registration data is valid.
/// </summary>
public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        // Ensure date of birth is realistic
        RuleFor(x => x.DateOfBirth)
            .GreaterThan(DateOnly.FromDateTime(DateTime.Today.AddYears(-130)))
            .WithMessage("Your date of birth must be real.");

        // Ensure email is valid
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Your email must be correct.");
    }
}
