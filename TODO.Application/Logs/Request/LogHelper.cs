namespace TODO.Application.Logs.Request
{
    public class LogHelper
    {
        public Dictionary<string, object>? OldValues { get; set; } = new Dictionary<string, object>();
        public Dictionary<string, object>? NewValues { get; set; } = new Dictionary<string, object>();
    }
}
