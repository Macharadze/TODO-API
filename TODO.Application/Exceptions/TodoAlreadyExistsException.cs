namespace TODO.Application.Exceptions
{
    public class TodoAlreadyExistsException : Exception
    {
        public TodoAlreadyExistsException(string? message) : base("Todo already exists "+message)
        {
        }
    }
}
