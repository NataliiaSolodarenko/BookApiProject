using AutoMapper;
using MyApp.Exceptions;

/// <summary>
/// Service for managing books.
/// Provides CRUD operations for books and validates author existence.
/// </summary>
public class BookService : IBookService
{
    private readonly IMapper _mapper;

    public BookService(IMapper mapper)
    {
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves all books from the data store.
    /// </summary>
    public IEnumerable<BookReadDto> GetAll() =>
        _mapper.Map<IEnumerable<BookReadDto>>(DataStorage.Books);

    /// <summary>
    /// Retrieves a specific book by ID.
    /// Throws BookNotFoundException if the book does not exist.
    /// </summary>
    public BookReadDto GetById(int id)
    {
        var book = DataStorage.Books.FirstOrDefault(b => b.Id == id);
        if (book == null)
            throw new BookNotFoundException(id);

        return _mapper.Map<BookReadDto>(book);
    }

    /// <summary>
    /// Creates a new book.
    /// Validates that the referenced author exists.
    /// </summary>
    public BookReadDto Create(BookCreateDto newBook)
    {
        var book = _mapper.Map<Book>(newBook);
        book.Id = DataStorage.Books.Any()
            ? DataStorage.Books.Max(b => b.Id) + 1
            : 1;

        if (!DataStorage.Authors.Any(a => a.Id == book.AuthorId))
            throw new AuthorNotFoundException(book.AuthorId);

        DataStorage.Books.Add(book);
        return _mapper.Map<BookReadDto>(book);
    }

    /// <summary>
    /// Updates an existing book's details.
    /// If author is changed, validates that the new author exists.
    /// </summary>
    public bool Update(int id, BookUpdateDto updatedBook)
    {
        var book = DataStorage.Books.FirstOrDefault(b => b.Id == id);
        if (book == null)
            throw new BookNotFoundException(id);

        book.Title = updatedBook.Title;
        book.Genre = updatedBook.Genre;

        if (book.AuthorId != updatedBook.AuthorId &&
            !DataStorage.Authors.Any(a => a.Id == updatedBook.AuthorId))
        {
            throw new AuthorNotFoundException(updatedBook.AuthorId);
        }

        return true;
    }

    /// <summary>
    /// Deletes a book by ID.
    /// </summary>
    public bool Delete(int id)
    {
        var book = DataStorage.Books.FirstOrDefault(b => b.Id == id);
        if (book == null)
            throw new BookNotFoundException(id);

        DataStorage.Books.Remove(book);
        return true;
    }
}
