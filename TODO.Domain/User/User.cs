using TODO.Domain.Base;
using TODO.Domain.TODO;

namespace TODO.Domain.Users
{
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    

        public virtual ICollection<ToDo> ToDos { get; set; }
    }
}
