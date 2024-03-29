namespace TODO.Application.Exceptions
{
    public class SubTaskDoesNotExistsException : Exception
    {
        public SubTaskDoesNotExistsException(string? message) : base("Subtask does not exists "+message)
        {
        }
    }
}
