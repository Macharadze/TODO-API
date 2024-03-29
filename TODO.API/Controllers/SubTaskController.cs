using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TODO.API.Infrastructure.Localization;
using TODO.Application.ISubTask;
using TODO.Application.Subtasks.Request;

namespace TODO.API.Controllers
{
    /// <summary>
    /// Controller for managing subtasks.
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [Asp.Versioning.ApiVersion(1.0)]
    [ApiController]
    public class SubTaskController : ControllerBase
    {
        private readonly ISubTaskInterface _subTaskInterface;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubTaskController"/> class.
        /// </summary>
        /// <param name="subTaskInterface">The service for managing subtasks.</param>
        public SubTaskController(ISubTaskInterface subTaskInterface)
        {
            _subTaskInterface = subTaskInterface;
        }

        /// <summary>
        /// Adds a new subtask.
        /// </summary>
        /// <param name="cancellation">Cancellation token.</param>
        /// <param name="request">The subtask request containing information about the subtask to add.</param>
        /// <returns>An ActionResult indicating the result of the operation.</returns>
        [HttpPost("subtask"), Authorize(Roles = "Customer")]
        public async Task<ActionResult> AddSubtask(CancellationToken cancellation, SubtaskRequest request)
        {
            try
            {
                await _subTaskInterface.Create(cancellation, request);
                return Ok(Language.Create);
            }
            catch (Exception)
            {
                return BadRequest(Language.Conflict);
            }
        }
        /// <summary>
        /// Updates a subtask.
        /// </summary>
        /// <param name="cancellation">Cancellation token.</param>
        /// <param name="title">The title of the todo.</param>
        /// <param name="request">The subtask details to update.</param>
        /// <returns>An ActionResult indicating the result of the operation.</returns>
        [HttpPut("subtask")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult> Update(CancellationToken cancellation, string title, SubtaskRequest request)
        {
            try
            {
                await _subTaskInterface.Update(cancellation, title, request);
                return Ok(Language.Update);
            }
            catch (Exception)
            {
                return BadRequest(Language.NotFound);
            }
        }

        /// <summary>
        /// Deletes a subtask.
        /// </summary>
        /// <param name="cancellation">Cancellation token.</param>
        /// <param name="todoID">The ID of the parent todo.</param>
        /// <param name="title">The title of the subtask to delete.</param>
        /// <returns>An ActionResult indicating the result of the operation.</returns>
        [HttpDelete("remove")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult> Delete(CancellationToken cancellation, string todoID, string title)
        {
            try
            {
                await _subTaskInterface.Delete(cancellation, todoID, title);
                return Ok(Language.Delete);
            }
            catch (Exception)
            {
                return BadRequest(Language.NotFound);
            }
        }
    }
}
