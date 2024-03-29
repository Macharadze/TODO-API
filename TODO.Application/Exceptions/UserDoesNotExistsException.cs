namespace TODO.Application.Exceptions
{
    public class UserDoesNotExistsException : Exception
    {
        public UserDoesNotExistsException(string? message) : base("User Does not exist: "+message)
        {
        }
    }
}
