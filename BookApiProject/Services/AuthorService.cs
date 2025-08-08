using AutoMapper;
using MyApp.Exceptions;
using Microsoft.EntityFrameworkCore;
using BookApiProject.Models;

/// <summary>
/// Service for managing authors.
/// Provides CRUD operations for authors and handles mapping between DTOs and entities.
/// </summary>
public class AuthorService : IAuthorService
{
    private readonly BookDbContext _context;
    private readonly IMapper _mapper;

    public AuthorService(BookDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves all authors from the data base.
    /// </summary>
    public async Task<IEnumerable<AuthorReadDto>> GetAllAsync()
    {
        var authors = await _context.Authors.AsNoTracking().ToListAsync();
        return _mapper.Map<IEnumerable<AuthorReadDto>>(authors);
    }

    /// <summary>
    /// Retrieves a specific author by ID.
    /// Throws AuthorNotFoundException if the author does not exist.
    /// </summary>
    public async Task<AuthorReadDto> GetByIdAsync(int id)
    {
        var author = await _context.Authors.FindAsync(id);
        if (author == null)
            throw new AuthorNotFoundException(id);

        return _mapper.Map<AuthorReadDto>(author);
    }

    /// <summary>
    /// Creates a new author.
    /// </summary>
    public async Task<AuthorReadDto> CreateAsync(AuthorCreateDto newAuthorDto)
    {
        var author = _mapper.Map<Author>(newAuthorDto);

        _context.Authors.Add(author);
        await _context.SaveChangesAsync();
        return _mapper.Map<AuthorReadDto>(author);
    }

    /// <summary>
    /// Updates an existing author's details.
    /// </summary>
    public async Task<bool> UpdateAsync(int id, AuthorUpdateDto updatedDto)
    {
        var author = await _context.Authors.FindAsync(id);
        if (author == null)
            throw new AuthorNotFoundException(id);

        author.FirstName = updatedDto.FirstName;
        author.LastName = updatedDto.LastName;
        author.Bio = updatedDto.Bio;
        author.BirthDate = updatedDto.BirthDate;

        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Deletes an author and detaches their books.
    /// </summary>
    public async Task<bool> DeleteAsync(int id)
    {
        var author = await _context.Authors.Include(a => a.Books).FirstOrDefaultAsync(a => a.Id == id);
        if (author == null)
            throw new AuthorNotFoundException(id);

        // Set AuthorId to 0 for books written by the deleted author
        foreach (var book in author.Books)
        {
            book.AuthorId = 0;
        }

        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();

        return true;
    }
}
