/// <summary>
/// Defines operations for managing books.
/// </summary>
public interface IBookService
{
    Task<IEnumerable<BookReadDto>> GetAllAsync();
    Task<BookReadDto> GetByIdAsync(int id);
    Task<BookReadDto> CreateAsync(BookCreateDto newBookDto);
    Task<bool> UpdateAsync(int id, BookUpdateDto updatedDto);
    Task<bool> DeleteAsync(int id);
}
