using TODO.Application.IAction;
using TODO.Application.ISubTask;
using TODO.Application.IToDo;
using TODO.Application.IUser;
using TODO.Infrastructure.Logs;
using TODO.Infrastructure.Subtasks;
using TODO.Infrastructure.ToDos;
using TODO.Infrastructure.Users;

namespace TODO.API.Infrastructure.Service
{
    public static class AddService
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserInterface, UserRepository>();
            services.AddScoped<IToDoInterface, ToDoRepository>();
            services.AddScoped<ISubTaskInterface, SubtaskRepository>();
            services.AddScoped<IActionLogInterface, ActionLogRepository>();
        }
    }
}
