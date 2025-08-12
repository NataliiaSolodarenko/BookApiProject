using Microsoft.AspNetCore.Mvc;
using BookApiProject.BookDTOs;
using BookApiProject.Services;
using BookApiProject.ExceptionFilters;
using BookApiProject.ValidationFilters;

namespace BookApiProject.Controllers;

[ApiController]
[Route("api/[controller]")]
[TypeFilter(typeof(BookExceptionFilter))]
[TypeFilter(typeof(AuthorExceptionFilter))]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    /// <summary>
    /// Retrieves all books.
    /// </summary>
    /// <response code="200">Returns a list of all books.</response>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookReadDto>>> GetAllAsync() => Ok(await _bookService.GetAllAsync());

    /// <summary>
    /// Retrieves a specific book by ID.
    /// </summary>
    /// <param name="id">ID of the book.</param>
    /// <response code="200">Returns the book details.</response>
    /// <response code="404">Book not found.</response>
    [HttpGet("{id}")]
    [ValidateId]
    public async Task<ActionResult<BookReadDto>> GetByIdAsync(int id)
    {
        var book = await _bookService.GetByIdAsync(id);
        return book == null ? NotFound() : Ok(book);
    }

    /// <summary>
    /// Creates a new book.
    /// </summary>
    /// <param name="newBook">Book details to create.</param>
    /// <response code="201">Book created successfully.</response>
    /// <response code="400">Invalid book data.</response>
    /// <response code="404">Author not found.</response>
    [HttpPost]
    public async Task<ActionResult<BookReadDto>> CreateAsync(BookCreateDto newBook)
    {
        var created = await _bookService.CreateAsync(newBook);
        return Created($"/api/books/{created.Id}", created);
    }

    /// <summary>
    /// Updates an existing book.
    /// </summary>
    /// <param name="id">ID of the book to update.</param>
    /// <param name="updatedBook">Updated book details.</param>
    /// <response code="204">Book updated successfully.</response>
    /// <response code="404">Book not found.</response>
    [HttpPut("{id}")]
    [ValidateId]
    public async Task<IActionResult> UpdateAsync(int id, BookUpdateDto updatedBook)
    {
        var success = await _bookService.UpdateAsync(id, updatedBook);
        return success ? NoContent() : NotFound();
    }

    /// <summary>
    /// Deletes a book.
    /// </summary>
    /// <param name="id">ID of the book to delete.</param>
    /// <response code="204">Book deleted successfully.</response>
    /// <response code="404">Book not found.</response>
    [HttpDelete("{id}")]
    [ValidateId]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var success = await _bookService.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }
}
