using AutoMapper;
using MyApp.Exceptions;

/// <summary>
/// Service for managing authors.
/// Provides CRUD operations for authors and handles mapping between DTOs and entities.
/// </summary>
public class AuthorService : IAuthorService
{
    private readonly IMapper _mapper;

    public AuthorService(IMapper mapper)
    {
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves all authors from the data store.
    /// </summary>
    public IEnumerable<AuthorReadDto> GetAll() =>
        _mapper.Map<IEnumerable<AuthorReadDto>>(DataStorage.Authors);

    /// <summary>
    /// Retrieves a specific author by ID.
    /// Throws AuthorNotFoundException if the author does not exist.
    /// </summary>
    public AuthorReadDto GetById(int id)
    {
        var author = DataStorage.Authors.FirstOrDefault(a => a.Id == id);
        if (author == null)
            throw new AuthorNotFoundException(id);

        return _mapper.Map<AuthorReadDto>(author);
    }

    /// <summary>
    /// Creates a new author.
    /// </summary>
    public AuthorReadDto Create(AuthorCreateDto newAuthor)
    {
        var author = _mapper.Map<Author>(newAuthor);
        author.Id = DataStorage.Authors.Any()
            ? DataStorage.Authors.Max(a => a.Id) + 1
            : 1;

        DataStorage.Authors.Add(author);
        return _mapper.Map<AuthorReadDto>(author);
    }

    /// <summary>
    /// Updates an existing author's details.
    /// </summary>
    public bool Update(int id, AuthorUpdateDto updatedAuthor)
    {
        var author = DataStorage.Authors.FirstOrDefault(a => a.Id == id);
        if (author == null)
            throw new AuthorNotFoundException(id);

        author.FullName = updatedAuthor.FullName;
        author.Bio = updatedAuthor.Bio;
        author.DateOfBirth = updatedAuthor.DateOfBirth;

        return true;
    }

    /// <summary>
    /// Deletes an author and detaches their books.
    /// </summary>
    public bool Delete(int id)
    {
        var author = DataStorage.Authors.FirstOrDefault(a => a.Id == id);
        if (author == null)
            throw new AuthorNotFoundException(id);

        // Set AuthorId to 0 for books written by the deleted author
        foreach (var book in DataStorage.Books.Where(b => b.AuthorId == author.Id))
        {
            book.AuthorId = 0;
        }

        DataStorage.Authors.Remove(author);
        return true;
    }
}
