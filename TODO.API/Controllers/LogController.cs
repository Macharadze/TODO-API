using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TODO.API.Infrastructure.Localization;
using TODO.Application.IAction;
using TODO.Application.Logs.Request;

namespace TODO.API.Controllers
{
    /// <summary>
    /// Controller for retrieving logs related to actions performed in the application.
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [Asp.Versioning.ApiVersion(1.0)]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly IActionLogInterface _actionLog;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogController"/> class.
        /// </summary>
        /// <param name="actionLog">The service for accessing action logs.</param>
        public LogController(IActionLogInterface actionLog)
        {
            _actionLog = actionLog;
        }

        /// <summary>
        /// Retrieves logs based on the provided entity ID and table name.
        /// </summary>
        /// <param name="token">Cancellation token.</param>
        /// <param name="entityID">The ID of the entity for which logs are requested.</param>
        /// <param name="tableName">The name of the table associated with the entity.</param>
        /// <returns>An ActionResult containing the requested logs.</returns>
        
        [HttpGet("{tableName}/{entityID}/logs"),Authorize(Roles ="Customer")]
        public async Task<ActionResult<LogRequest>> Get(CancellationToken token, string entityID, string tableName)
        {
            try
            {
                return Ok(await _actionLog.GetLogs(token, entityID, tableName));
            }
            catch (Exception ex)
            {
                return BadRequest(Language.NotFound);

            }
        }

    }
}
