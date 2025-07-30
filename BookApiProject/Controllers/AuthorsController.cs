using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[TypeFilter(typeof(AuthorExceptionFilter))]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorService _authorService;

    public AuthorsController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    /// <summary>
    /// Retrieves all authors.
    /// </summary>
    /// <response code="200">Returns a list of all authors.</response>
    [HttpGet]
    public ActionResult<IEnumerable<AuthorReadDto>> GetAll() => Ok(_authorService.GetAll());

    /// <summary>
    /// Retrieves a specific author by ID.
    /// </summary>
    /// <param name="id">ID of the author.</param>
    /// <response code="200">Returns the author details.</response>
    /// <response code="404">Author not found.</response>
    [HttpGet("{id}")]
    [ValidateId]
    public ActionResult<AuthorReadDto> GetById(int id)
    {
        var author = _authorService.GetById(id);
        return Ok(author);
    }

    /// <summary>
    /// Creates a new author.
    /// </summary>
    /// <param name="newAuthor">Author details to create.</param>
    /// <response code="201">Author created successfully.</response>
    /// <response code="400">Invalid author data.</response>
    [HttpPost]
    public ActionResult Create(AuthorCreateDto newAuthor)
    {
        var created = _authorService.Create(newAuthor);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Updates an existing author.
    /// </summary>
    /// <param name="id">ID of the author to update.</param>
    /// <param name="updatedAuthor">Updated author details.</param>
    /// <response code="204">Author updated successfully.</response>
    /// <response code="404">Author not found.</response>
    [HttpPut("{id}")]
    [ValidateId]
    public IActionResult Update(int id, AuthorUpdateDto updatedAuthor)
    {
        _authorService.Update(id, updatedAuthor);
        return NoContent();
    }

    /// <summary>
    /// Deletes an author (Admin only).
    /// </summary>
    /// <param name="id">ID of the author to delete.</param>
    /// <response code="204">Author deleted successfully.</response>
    /// <response code="403">Forbidden. Only Admins can delete authors.</response>
    /// <response code="404">Author not found.</response>
    [HttpDelete("{id}")]
    [ValidateId]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        _authorService.Delete(id);
        return NoContent();
    }
}
