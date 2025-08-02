using AutoMapper;
using MyApp.Exceptions;
using Microsoft.EntityFrameworkCore;
using BookApiProject.Models;

/// <summary>
/// Service for managing books.
/// Provides CRUD operations for books and validates author existence.
/// </summary>
public class BookService : IBookService
{
    private readonly BookDbContext _context;
    private readonly IMapper _mapper;

    public BookService(BookDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves all books from the data store.
    /// </summary>
    public async Task<IEnumerable<BookReadDto>> GetAllAsync()
    {
        var books = await _context.Books.ToListAsync();
        return _mapper.Map<IEnumerable<BookReadDto>>(books);
    }

    /// <summary>
    /// Retrieves a specific book by ID.
    /// Throws BookNotFoundException if the book does not exist.
    /// </summary>
    public async Task<BookReadDto> GetByIdAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
            throw new BookNotFoundException(id);

        return _mapper.Map<BookReadDto>(book);
    }

    /// <summary>
    /// Creates a new book.
    /// Validates that the referenced author exists.
    /// </summary>
    public async Task<BookReadDto> CreateAsync(BookCreateDto newBookDto)
    {
        var book = _mapper.Map<Book>(newBookDto);

        if (await _context.Authors.FindAsync(book.AuthorId) == null)
            throw new AuthorNotFoundException(book.AuthorId);

        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return _mapper.Map<BookReadDto>(book);
    }

    /// <summary>
    /// Updates an existing book's details.
    /// If author is changed, validates that the new author exists.
    /// </summary>
    public async Task<bool> UpdateAsync(int id, BookUpdateDto updatedDto)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
            throw new BookNotFoundException(id);

        book.Title = updatedDto.Title;
        book.Genre = updatedDto.Genre;

        if (book.AuthorId != updatedDto.AuthorId)
        {
            if (await _context.Authors.FindAsync(updatedDto.AuthorId) == null)
                throw new AuthorNotFoundException(updatedDto.AuthorId);

            book.AuthorId = updatedDto.AuthorId;
        } 

        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Deletes a book by ID.
    /// </summary>
    public async Task<bool> DeleteAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
            throw new BookNotFoundException(id);

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        return true;
    }
}
