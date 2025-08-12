using FluentValidation;
using BookApiProject.BookDTOs;

namespace BookApiProject.Validators;

/// <summary>
/// Validator for <see cref="BookCreateDto"/>.
/// Ensures that book data is valid before creation.
/// </summary>
public class BookCreateDtoValidator : AbstractValidator<BookCreateDto>
{
    public BookCreateDtoValidator()
    {
        // AuthorId must be valid (0 or greater)
        RuleFor(x => x.AuthorId)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Author Id must be 0 or greater.");
    }
}
