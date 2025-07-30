using Microsoft.AspNetCore.Mvc;

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
    /// Test method that throws an exception.
    /// </summary>
    /// <response code="500">Always throws an exception (for testing).</response>
    [HttpGet("test-error")]
    public IActionResult ThrowError()
    {
        throw new Exception("Test error");
    }

    /// <summary>
    /// Retrieves all books.
    /// </summary>
    /// <response code="200">Returns a list of all books.</response>
    [HttpGet]
    public ActionResult<IEnumerable<BookReadDto>> GetAll() => Ok(_bookService.GetAll());

    /// <summary>
    /// Retrieves a specific book by ID.
    /// </summary>
    /// <param name="id">ID of the book.</param>
    /// <response code="200">Returns the book details.</response>
    /// <response code="404">Book not found.</response>
    [HttpGet("{id}")]
    [ValidateId]
    public ActionResult<BookReadDto> GetById(int id)
    {
        var book = _bookService.GetById(id);
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
    public ActionResult<BookReadDto> Create(BookCreateDto newBook)
    {
        var created = _bookService.Create(newBook);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
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
    public IActionResult Update(int id, BookUpdateDto updatedBook)
    {
        var success = _bookService.Update(id, updatedBook);
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
    public IActionResult Delete(int id)
    {
        var success = _bookService.Delete(id);
        return success ? NoContent() : NotFound();
    }
}
