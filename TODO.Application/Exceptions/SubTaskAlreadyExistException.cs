namespace TODO.Application.Exceptions
{
    public class SubTaskAlreadyExistException : Exception
    {
        public SubTaskAlreadyExistException(string? message) : base("subtask already exists "+message)
        {
        }
    }
}
