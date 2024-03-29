using TODO.Domain.ActionResult;
using TODO.Domain.Base;
using TODO.Domain.Subtasks;
using TODO.Domain.Users;

namespace TODO.Domain.TODO
{
    public class ToDo : BaseEntity
    {
        public string Title { get; set; }
        public DateTime? TargetCompletionDate { get; set; }

        public virtual User Owner { get; set; }
        public virtual ICollection<Subtask> Subtasks { get; set; }
        public virtual ICollection<ActionLog> ActionLogs { get; set; }
    }
}
