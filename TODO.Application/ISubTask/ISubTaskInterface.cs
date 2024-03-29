
using TODO.Application.Subtasks.Request;
using TODO.Domain.Subtasks;
using TODO.Domain.TODO;

namespace TODO.Application.ISubTask
{
    public interface ISubTaskInterface
    {
        Task Create(CancellationToken cancellation, SubtaskRequest request);
        Task<Subtask> Get(CancellationToken cancellationToken, string SubId);
        Task<Subtask> GetByName(CancellationToken cancellationToken, string title);

        Task<bool> Exists(CancellationToken cancellationToken, string title);
        Task<ToDo> GetOwner(CancellationToken cancellationToken,string id);
        Task Update(CancellationToken cancellationToken,string title, SubtaskRequest request);
        Task Delete(CancellationToken cancellationToken,string todoID, string title);
    }
}
