namespace MyApp.Exceptions
{
    /// <summary>
    /// Exception thrown when the provided password is incorrect.
    /// </summary>
    public class PasswordIncorrect : Exception
    {
        public PasswordIncorrect() : base("Password is incorrect.") { }
    }

    /// <summary>
    /// Exception thrown when a user with the specified username does not exist.
    /// </summary>
    public class UserWithUsernameDoesNotExist : Exception
    {
        public UserWithUsernameDoesNotExist() : base("User with this login does not exist.") { }
    }

    /// <summary>
    /// Exception thrown when a user with the specified email address does not exist.
    /// </summary>
    public class UserWithEmailDoesNotExist : Exception
    {
        public UserWithEmailDoesNotExist() : base("User with this email does not exist.") { }
    }

    /// <summary>
    /// Exception thrown when the provided username is already taken by another user.
    /// </summary>
    public class UsernameIsAlreadyInUse : Exception
    {
        public UsernameIsAlreadyInUse() : base("Username is already in use.") { }
    }

    /// <summary>
    /// Exception thrown when the provided email address is already taken by another user.
    /// </summary>
    public class EmailIsAlreadyInUse : Exception
    {
        public EmailIsAlreadyInUse() : base("Email is already in use.") { }
    }
}