using TODO.Application.Users.Request;
using TODO.Domain.Enums;
using TODO.Domain.TODO;
using TODO.Domain.Users;

namespace TODO.Application.IUser
{
    public interface IUserInterface
    {
        Task<bool> Exists(CancellationToken cancellationToken, string username);
        Task Create(CancellationToken cancellationToken, UserLogin register);
        Task<string> Login(CancellationToken cancellationToken, UserLogin login);
        Task<List<ToDo>> GetAllToDos(CancellationToken token, Status status = 0);
        Task<User> GetCurrentUser(CancellationToken cancellationToken);

    }
}
