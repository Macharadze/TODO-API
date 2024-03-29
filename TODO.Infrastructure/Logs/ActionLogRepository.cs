using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TODO.Application.IAction;
using TODO.Application.Logs.Request;
using TODO.Domain.ActionResult;
using TODO.Infrastructure.Base;
using TODO.Persistence.Context;

namespace TODO.Infrastructure.Logs
{
    public class ActionLogRepository : BaseRepository<ActionLog>, IActionLogInterface
    {
        public ActionLogRepository(ApplicationDBcontext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        public async Task<LogRequest> GetLogs(CancellationToken cancellation, string entityID, string tableName)
        {
            var logs = await GetLogs(cancellation);
            if (logs is null)
                throw new Exception("does not exist");

            var result = await GetTargetLogs(cancellation, logs, entityID, tableName);

            var values = await GetValues(cancellation, result);

            var request = new LogRequest
            {
                TableName = tableName,
                KeyValues = entityID,
                Logs = values
            };


            return request;
        }

        private async Task<List<LogHelper>> GetValues(CancellationToken token, IEnumerable<IGrouping<object, TodoLog>> logs) =>
          await Task.Run(() =>
           logs.SelectMany(group =>
             group.Select(log => new LogHelper
             {
                 OldValues = log.oldvalue ?? default,
                 NewValues = log.newvalue ?? default
             })).ToList());

        private async Task<IEnumerable<IGrouping<object, TodoLog>>> GetTargetLogs(CancellationToken token, List<TodoLog> logs, string entityID, string tableName) =>
            await Task.Run(() => logs.Where(item => item.id != null
            && item.id.TryGetValue("Id", out var idValue) && idValue.ToString().ToLower().Equals(entityID.ToLower())
            && item.TableName.ToLower().Equals(tableName.ToLower())).GroupBy(i => i.id["Id"]));


        private async Task<List<TodoLog>> GetLogs(CancellationToken token) =>
           await _dbSet
             .Select(i => new TodoLog
             {
                 id = !string.IsNullOrEmpty(i.KeyValues) ? JsonConvert.DeserializeObject<Dictionary<string, object>>(i.KeyValues) : null,
                 TableName = i.TableName,
                 oldvalue = !string.IsNullOrEmpty(i.OldValues) ? JsonConvert.DeserializeObject<Dictionary<string, object>>(i.OldValues) : null,
                 newvalue = !string.IsNullOrEmpty(i.NewValues) ? JsonConvert.DeserializeObject<Dictionary<string, object>>(i.NewValues) : null,
             }).ToListAsync();


    }
}
