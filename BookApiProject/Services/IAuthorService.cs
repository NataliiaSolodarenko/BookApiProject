/// <summary>
/// Defines operations for managing authors.
/// </summary>
public interface IAuthorService
{
    Task<IEnumerable<AuthorReadDto>> GetAllAsync();
    Task<AuthorReadDto> GetByIdAsync(int id);
    Task<AuthorReadDto> CreateAsync(AuthorCreateDto newAuthorDto);
    Task<bool> UpdateAsync(int id, AuthorUpdateDto updatedDto);
    Task<bool> DeleteAsync(int id);
}


