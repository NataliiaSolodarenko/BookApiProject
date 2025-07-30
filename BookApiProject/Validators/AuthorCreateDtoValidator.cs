using FluentValidation;

/// <summary>
/// Validator for <see cref="AuthorCreateDto"/>.
/// Ensures that author data meets business rules before creation.
/// </summary>
public class AuthorCreateDtoValidator : AbstractValidator<AuthorCreateDto>
{
    public AuthorCreateDtoValidator()
    {
        // Author must be born in the past
        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Today)
            .WithMessage("Date of birth must be in the past.");
        
        // Author must be at least 18 years old
        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Today.AddYears(-18))
            .WithMessage("Author must be at least 18 years old.");
    }
}
