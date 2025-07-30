namespace MyApp.Exceptions
{
    /// <summary>
    /// Exception thrown when an author with the specified ID could not be found.
    /// </summary>
    public class AuthorNotFoundException : Exception
    {
        public AuthorNotFoundException(int id)
            : base($"Author with ID {id} not found.") { }
    }
}