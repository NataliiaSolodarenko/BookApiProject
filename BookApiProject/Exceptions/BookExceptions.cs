namespace BookApiProject.Exceptions;

    /// <summary>
    /// Exception thrown when a book with the specified ID could not be found.
    /// </summary>
    public class BookNotFoundException : Exception
    {
        public BookNotFoundException(int id)
            : base($"Book with ID {id} not found.") { }
    }