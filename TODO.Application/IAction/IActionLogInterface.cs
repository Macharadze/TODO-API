using TODO.Application.Logs.Request;
using TODO.Domain.Enums;

namespace TODO.Application.IAction
{
    public interface IActionLogInterface
    {
        Task<LogRequest> GetLogs(CancellationToken cancellation, string todoID,string tableName);
    }
}
