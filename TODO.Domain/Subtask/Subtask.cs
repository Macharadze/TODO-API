using TODO.Domain.Base;
using TODO.Domain.TODO;

namespace TODO.Domain.Subtasks
{
    public class Subtask : BaseEntity
    {
        public string Title { get; set; }

        public virtual ToDo ToDo { get; set; }
    }
}

