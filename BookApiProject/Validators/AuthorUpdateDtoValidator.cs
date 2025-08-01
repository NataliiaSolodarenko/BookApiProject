using FluentValidation;

/// <summary>
/// Validator for <see cref="AuthorUpdateDto"/>.
/// Inherits validation rules from AuthorCreateDtoValidator.
/// </summary>
public class AuthorUpdateDtoValidator : AbstractValidator<AuthorUpdateDto>
{
    public AuthorUpdateDtoValidator()
    {
        Include(new AuthorCreateDtoValidator());
    }
}
