namespace TODO.Application.Logs.Request
{
    public class TodoLog
    {
        public string TableName { get; set; }
        public Dictionary<string, object>? id { get; set; }
        public Dictionary<string, object>? oldvalue { get; set; }
        public Dictionary<string, object>? newvalue { get; set; }

    }
}
