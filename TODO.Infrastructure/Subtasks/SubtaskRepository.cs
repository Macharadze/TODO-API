using Microsoft.AspNetCore.Http;
using TODO.Application.Exceptions;
using TODO.Application.ISubTask;
using TODO.Application.IToDo;
using TODO.Application.IUser;
using TODO.Application.Subtasks.Request;
using TODO.Domain.Subtasks;
using TODO.Domain.TODO;
using TODO.Domain.Users;
using TODO.Infrastructure.Base;
using TODO.Persistence.Context;

namespace TODO.Infrastructure.Subtasks
{
    public class SubtaskRepository : BaseRepository<Subtask>, ISubTaskInterface
    {
        private readonly IToDoInterface _toDoInterface;
        public SubtaskRepository(ApplicationDBcontext context, IHttpContextAccessor httpContextAccessor, IToDoInterface toDoInterface) : base(context, httpContextAccessor)
        {
            _toDoInterface = toDoInterface;
        }

        public async Task Create(CancellationToken cancellation, SubtaskRequest request)
        {
            if (Exists(cancellation, request.Title).Result)
                throw new SubTaskAlreadyExistException("");

            var owner = await GetOwner(cancellation, request.ToDoID);
            if (owner is null)
                throw new TodoDoesNotExistsException("");

            var subtask = new Subtask
            {
                ToDo = owner,
                Title = request.Title,
            };
            await base.AddAsync(cancellation, subtask);
            await _context.SaveChangesAsync(true, cancellation);

        }

        public async Task Delete(CancellationToken cancellationToken,string todoID, string SubId)
        {
            if (!BelongsToCurrentUser(cancellationToken, todoID, SubId).Result)
                throw new TodoDoesNotExistsException("wrong date");

            var task = await Get(cancellationToken, SubId);
            if (task is null)
                throw new SubTaskDoesNotExistsException("");

            task.Status = Domain.Enums.Status.Deleted;
            await base.UpdateAsync(cancellationToken, task);
            await _context.SaveChangesAsync(true, cancellationToken);
        }

        public async Task<bool> Exists(CancellationToken cancellationToken, string title)
        {
            return await base.AnyAsync(cancellationToken, i => i.Title.ToLower().Equals(title.ToLower()));
        }

        public async Task<Subtask> Get(CancellationToken cancellationToken, string SubId)
        {
            return await base.GetAsync(cancellationToken, i => i.Id.ToString().ToLower().Equals(SubId.ToLower()));
        }
        public async Task<Subtask> GetByName(CancellationToken cancellationToken, string title)
        {
            return await base.GetAsync(cancellationToken, i => i.Title.ToLower().Equals(title.ToLower()));
        }

        public async Task<ToDo> GetOwner(CancellationToken cancellationToken, string id)
        {
            return await _toDoInterface.Get(cancellationToken, id);
        }

        public async Task Update(CancellationToken cancellationToken,string title, SubtaskRequest request)
        {
            if (!Exists(cancellationToken,title).Result)
                throw new SubTaskDoesNotExistsException("");

            if (!BelongsToCurrentUser(cancellationToken, request.ToDoID, title).Result)
                throw new TodoDoesNotExistsException("wrong date");

            var updatedToDo =  await GetByName(cancellationToken, title);
            updatedToDo.Title = request.Title;
            updatedToDo.ModifiedAt = DateTime.UtcNow;
            updatedToDo.Status = Domain.Enums.Status.Updated;
            await base.UpdateAsync(cancellationToken, updatedToDo);
            await _context.SaveChangesAsync(true, cancellationToken);

        }

        private async Task<bool> BelongsToCurrentUser(CancellationToken token,string todoID,string title)
        {
            var tasks = await _toDoInterface.GetSubtasks(token, todoID);
            return tasks.Any(i => i.Title.ToLower().Equals(title));
        }

        
    }
}
