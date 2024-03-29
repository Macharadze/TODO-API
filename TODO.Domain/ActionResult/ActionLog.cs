
using TODO.Domain.Enums;

namespace TODO.Domain.ActionResult
{
    public class ActionLog 
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public DateTime Date { get; set; } = DateTime.Now;
        public Status OperationType { get; set; }
        public string KeyValues { get; set; }
        public string TableName { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
    }
}

