using FluentValidation;
using BookApiProject.BookDTOs;

namespace BookApiProject.Validators;

/// <summary>
/// Validator for <see cref="BookUpdateDto"/>.
/// Inherits validation rules from BookCreateDtoValidator.
/// </summary>
public class BookUpdateDtoValidator : AbstractValidator<BookUpdateDto>
{
    public BookUpdateDtoValidator()
    {
        Include(new BookCreateDtoValidator());
    }
}
