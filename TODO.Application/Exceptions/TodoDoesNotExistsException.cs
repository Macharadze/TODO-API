namespace TODO.Application.Exceptions
{
    public class TodoDoesNotExistsException : Exception
    {
        public TodoDoesNotExistsException(string? message) : base("Todo does not exists"+message)
        {
        }
    }
}
