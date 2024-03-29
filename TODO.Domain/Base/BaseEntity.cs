using TODO.Domain.Enums;

namespace TODO.Domain.Base
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ModifiedAt { get; set; }
        public Status Status { get; set; }
    }
}
