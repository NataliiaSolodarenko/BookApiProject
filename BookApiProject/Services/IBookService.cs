/// <summary>
/// Defines operations for managing books.
/// </summary>
public interface IBookService
{
    IEnumerable<BookReadDto> GetAll();
    BookReadDto GetById(int id);
    BookReadDto Create(BookCreateDto newBook);
    bool Update(int id, BookUpdateDto updatedBook);
    bool Delete(int id);
}
