namespace TODO.Application.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string? message) : base("User does not find"+message)
        {
        }
    }
}
