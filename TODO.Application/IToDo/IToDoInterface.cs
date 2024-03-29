using TODO.Application.ToDos.Request;
using TODO.Domain.Enums;
using TODO.Domain.Subtasks;
using TODO.Domain.TODO;

namespace TODO.Application.IToDo
{
    public interface IToDoInterface
    {
        Task Create(CancellationToken cancellation, ToDoRequest request);
        Task<bool> Exists(CancellationToken cancellationToken, string title);
        Task<ToDo> Get(CancellationToken token, string id);
        Task<List<Subtask>> GetSubtasks(CancellationToken token,string id);
        Task Update(CancellationToken token, ToDo request,Status status = Status.Updated);
        Task Delete(CancellationToken token, string id);

    }
}
