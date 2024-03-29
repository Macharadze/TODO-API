using TODO.Domain.Enums;
using TODO.Domain.TODO;

namespace TODO.Application.Logs.Request
{
    public class LogRequest
    {
        public string KeyValues { get; set; }
        public string TableName { get; set; }
        public List<LogHelper> Logs { get; set; }
       // public List<Dictionary<string,object>>? OldValues { get; set; }
     //   public List<Dictionary<string,object>>? NewValues { get; set; }
    }
}
