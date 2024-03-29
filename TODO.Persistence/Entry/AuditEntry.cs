using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using TODO.Domain.ActionResult;
using TODO.Domain.Enums;

namespace TODO.Persistence.Entry
{
    public class AuditEntry
    {
        private readonly EntityEntry _entityEntry;
        public AuditEntry(EntityEntry entry)
        {
            _entityEntry = entry;
        }

        public EntityEntry Entry { get; }
        public string TableName { get; set; }
        public Status Status { get; set; }

        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
        public List<PropertyEntry> TemporaryProperties { get; } = new List<PropertyEntry>();

        public bool HasTemporaryProperties => TemporaryProperties.Any();

        public ActionLog ToAudit()
        {
            var audit = new ActionLog();
            audit.TableName = TableName;
            audit.KeyValues = JsonConvert.SerializeObject(KeyValues);
            audit.Date = DateTime.UtcNow;
            audit.OperationType = Status;
            audit.OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues);
            audit.NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues);
            return audit;
        }

    }
}
