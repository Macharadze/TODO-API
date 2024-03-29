namespace TODO.Application.Exceptions
{
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException(string? message) : base("user already exists " + message)
        {
        }
    }
}
