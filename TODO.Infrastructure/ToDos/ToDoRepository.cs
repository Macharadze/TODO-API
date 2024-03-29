using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TODO.Application.Exceptions;
using TODO.Application.IToDo;
using TODO.Application.IUser;
using TODO.Application.ToDos.Request;
using TODO.Domain.Enums;
using TODO.Domain.Subtasks;
using TODO.Domain.TODO;
using TODO.Domain.Users;
using TODO.Infrastructure.Base;
using TODO.Infrastructure.Users;
using TODO.Persistence.Context;

namespace TODO.Infrastructure.ToDos
{
    public class ToDoRepository : BaseRepository<ToDo>, IToDoInterface
    {
        private readonly IUserInterface _userInterface;
        public ToDoRepository(ApplicationDBcontext context, IHttpContextAccessor httpContext,IUserInterface userInterface) : base(context, httpContext)
        {
            _userInterface = userInterface;
        }


        public async Task Create(CancellationToken cancellation, ToDoRequest request)
        {
            if (Exists(cancellation, request.Title).Result)
                throw new TodoAlreadyExistsException("");

            var todo = new ToDo
            {
                TargetCompletionDate = request.TargetCompletionDate,
                Title = request.Title,
               Owner = await _userInterface.GetCurrentUser(cancellation)
            };
            await base.AddAsync(cancellation, todo);
            await _context.SaveChangesAsync(true, cancellation);
        }

        public async Task Delete(CancellationToken token, string id)
        {
            var todos = await _userInterface.GetAllToDos(token);
            var todo = todos.Where(i => i.Id.ToString().ToLower().Equals(id.ToLower())).FirstOrDefault();
            if(todo is null)
                throw new TodoDoesNotExistsException("");

            todo.Status = Status.Deleted;
            foreach(var item in todo.Subtasks)
                item.Status = Status.Deleted;

            await base.UpdateAsync(token, todo);
            await _context.SaveChangesAsync(true,token);
        }

        public async Task<bool> Exists(CancellationToken cancellationToken, string title)
        {
            return await base.AnyAsync(cancellationToken, i => i.Title.ToLower().Equals(title.ToLower()));
        }

        public async Task<ToDo> Get(CancellationToken token, string id)
        {
            var todos = await _userInterface.GetAllToDos(token);
            var todo = todos.Where(i => i.Id.ToString().ToLower().Equals(id.ToLower())).FirstOrDefault();
          if(todo is null)
                throw new TodoDoesNotExistsException("");
          return todo;
        }

        public async Task<List<Subtask>> GetSubtasks(CancellationToken token, string id)
        {
            var todo = await Get(token, id);
            var subtask = await _dbSet.Include(i => i.Subtasks).Where(i => i.Id.ToString().Equals(todo.Id.ToString())).FirstOrDefaultAsync();
            if (subtask is null || subtask.Subtasks is null)
                throw new TodoDoesNotExistsException("does not exist");
            var tasks = subtask.Subtasks.ToList();
            return tasks;
        }

        public async Task Update(CancellationToken token, ToDo request,Status status = Status.Updated)
        {
            request.Status = status;
            request.ModifiedAt = DateTime.UtcNow;

           await base.UpdateAsync(token,request);
            await _context.SaveChangesAsync(true, token); 
        }
    }
}
