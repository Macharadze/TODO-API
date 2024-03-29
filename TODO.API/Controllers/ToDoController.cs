using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TODO.API.Infrastructure.Localization;
using TODO.Application.IToDo;
using TODO.Application.Subtasks.Request;
using TODO.Application.ToDos.Request;

namespace TODO.API.Controllers
{
    /// <summary>
    /// Controller for managing ToDos.
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [Asp.Versioning.ApiVersion(1.0)]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoInterface _toDoInterface;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoController"/> class.
        /// </summary>
        /// <param name="toDoInterface">The service for managing ToDos.</param>
        public ToDoController(IToDoInterface toDoInterface)
        {
            _toDoInterface = toDoInterface;
        }

        /// <summary>
        /// Adds a new ToDo item.
        /// </summary>
        /// <param name="token">Cancellation token.</param>
        /// <param name="request">The ToDo request containing information about the ToDo item to add.</param>
        /// <returns>An ActionResult indicating the result of the operation.</returns>
        [HttpPost, Authorize(Roles = "Customer")]
        public async Task<ActionResult> AddToDo(CancellationToken token, ToDoRequest request)
        {
            try
            {
                await _toDoInterface.Create(token, request);
                return Ok(Language.Create);
            }
            catch (Exception)
            {
                return BadRequest(Language.Conflict);
            }
        }

        /// <summary>
        /// Retrieves a ToDo item by ID.
        /// </summary>
        /// <param name="token">Cancellation token.</param>
        /// <param name="id">The ID of the ToDo item to retrieve.</param>
        /// <returns>An ActionResult containing the requested ToDo item.</returns>
        [HttpGet("Todo/{id}"), Authorize(Roles = "Customer")]
        public async Task<ActionResult<ToDoRequest>> Get(CancellationToken token, string id)
        {
            try
            {
                var todo = await _toDoInterface.Get(token, id);
                if (todo == null)
                    return NotFound(Language.NotFound);

                return Ok(todo.Adapt<ToDoRequest>());
            }
            catch (Exception)
            {
                return BadRequest(Language.Conflict);
            }
        }

        /// <summary>
        /// Retrieves subtasks of a ToDo item by ID.
        /// </summary>
        /// <param name="token">Cancellation token.</param>
        /// <param name="id">The ID of the ToDo item.</param>
        /// <returns>An ActionResult containing the subtasks of the requested ToDo item.</returns>
        [HttpGet("ToDo/{id}/subtask"), Authorize(Roles = "Customer")]
        public async Task<ActionResult<ResponseSubTask>> GetSubTasks(CancellationToken token, string id)
        {
            try
            {
                var subtasks = await _toDoInterface.GetSubtasks(token, id);
                if (subtasks == null)
                    return NotFound(Language.NotFound);

                return Ok(subtasks.Adapt<List<ResponseSubTask>>());
            }
            catch (Exception)
            {
                return BadRequest(Language.Conflict);
            }
        }

        /// <summary>
        /// Deletes a ToDo item.
        /// </summary>
        /// <remarks>
        /// Requires the user to be authenticated and have the 'Customer' role.
        /// </remarks>
        /// <param name="token">Cancellation token.</param>
        /// <param name="id">ID of the ToDo item to delete.</param>
        /// <returns>
        /// ActionResult with status code 200 (OK) and a message indicating successful deletion,
        /// or NotFound status code with a message if the item was not found.
        /// </returns>
        [HttpDelete("ToDo"), Authorize(Roles = "Customer")]
        public async Task<ActionResult> Delete(CancellationToken token, string id)
        {
            try
            {
                await _toDoInterface.Delete(token, id);
                return Ok(Language.Delete);

            }
            catch (Exception e)
            {
                return NotFound(Language.NotFound);
            }
        }


        /// <summary>
        /// Updates a ToDo item.
        /// </summary>
        /// <remarks>
        /// Requires the user to be authenticated and have the 'Customer' role.
        /// </remarks>
        /// <param name="token">Cancellation token.</param>
        /// <param name="id">ID of the ToDo item to update.</param>
        /// <param name="request">Request containing the updated ToDo information.</param>
        /// <returns>
        /// ActionResult with status code 200 (OK) and a message indicating successful update,
        /// or BadRequest status code with an error message if the update fails.
        /// </returns>
        [HttpPut("todos/{id}"), Authorize(Roles = "Customer")]
        public async Task<ActionResult> UpdateToDo(CancellationToken token, string id, ToDoRequest request)
        {
            try
            {
                var user = await _toDoInterface.Get(token, id);
                user.Title = request.Title;
                user.TargetCompletionDate = request.TargetCompletionDate;
                await _toDoInterface.Update(token, user);
                return Ok(Language.Update);
            }
            catch (Exception e)
            {
                return NotFound(Language.NotFound);
            }
        }

        /// <summary>
        /// Partially updates a ToDo item.
        /// </summary>
        /// <remarks>
        /// Requires the user to be authenticated and have the 'Customer' role.
        /// </remarks>
        /// <param name="id">ID of the ToDo item to partially update.</param>
        /// <param name="patchDocument">JsonPatchDocument containing partial updates to the ToDo item.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>
        /// IActionResult with status code 200 (OK) and a message indicating successful partial update,
        /// or BadRequest status code with an error message if the partial update fails.
        /// </returns>
        [HttpPatch("todos/{id}"), Authorize(Roles = "Customer")]
        public async Task<IActionResult> PartiallyUpdateTodo(string id, [FromBody] JsonPatchDocument<ToDoRequest> patchDocument, CancellationToken cancellationToken)
        {
            try
            {
                var todo = await _toDoInterface.Get(cancellationToken, id);
                var request = todo.Adapt<ToDoRequest>();
                patchDocument.ApplyTo(request);

                todo.TargetCompletionDate = request.TargetCompletionDate;
                todo.Title = request.Title;
                todo.ModifiedAt = DateTime.UtcNow;
                await _toDoInterface.Update(cancellationToken, todo);

                return Ok(Language.Update);
            }
            catch (Exception)
            {
                return BadRequest(Language.NotFound);
            }
        }

        /// <summary>
        /// Marks a ToDo item as done.
        /// </summary>
        /// <remarks>
        /// Requires the user to be authenticated and have the 'Customer' role.
        /// </remarks>
        /// <param name="token">Cancellation token.</param>
        /// <param name="id">ID of the ToDo item to mark as done.</param>
        /// <returns>
        /// ActionResult with status code 200 (OK) and a message indicating successful marking as done,
        /// or BadRequest status code with an error message if the marking fails.
        /// </returns>
        [HttpPut("todos/{id}/done"), Authorize(Roles = "Customer")]
        public async Task<ActionResult> MarkAsDone(CancellationToken token, string id)
        {
            try
            {
                var user = await _toDoInterface.Get(token, id);
                await _toDoInterface.Update(token, user, Domain.Enums.Status.MarkedAsDone);
                return Ok(Language.Update);
            }
            catch (Exception e)
            {
                return NotFound(Language.NotFound);
            }
        }

    }
}
