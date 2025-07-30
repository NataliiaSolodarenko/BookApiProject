/// <summary>
/// Defines operations for managing authors.
/// </summary>
public interface IAuthorService
{
    IEnumerable<AuthorReadDto> GetAll();
    AuthorReadDto GetById(int id);
    AuthorReadDto Create(AuthorCreateDto newAuthor);
    bool Update(int id, AuthorUpdateDto updatedAuthor);
    bool Delete(int id);
}


