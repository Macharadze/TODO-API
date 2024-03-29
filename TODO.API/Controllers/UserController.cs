using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TODO.API.Infrastructure.Localization;
using TODO.Application.IUser;
using TODO.Application.ToDos.Request;
using TODO.Application.Users.Request;
using TODO.Domain.Enums;

namespace TODO.API.Controllers
{
    /// <summary>
    /// Controller for managing user-related operations.
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [Asp.Versioning.ApiVersion(1.0)]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserInterface _userInterface;
        /// <summary>
        /// Constructor for UserController.
        /// </summary>
        /// <param name="userInterface">An instance of IUserInterface 
        /// for interacting with user-related operations.</param>
        public UserController(IUserInterface userInterface)
        {
            _userInterface = userInterface;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="user">UserLogin object containing user credentials.</param>
        /// <param name="cancellation">Cancellation token for cancelling the operation.</param>
        [HttpPost("register")]
        public async Task<ActionResult> Register(UserLogin user, CancellationToken cancellation)
        {
            try
            {
                await _userInterface.Create(cancellation, user);
                return Ok("Success");
            }
            catch (Exception ex)
            {
                return BadRequest(Language.Conflict);
            }
        }

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="user">UserLogin object containing user credentials.</param>
        /// <param name="cancellation">Cancellation token for cancelling the operation.</param>
        [HttpPost("Login")]
        public async Task<ActionResult> Login(UserLogin user, CancellationToken cancellation)
        {
            try
            {
                string token = await _userInterface.Login(cancellation, user);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(Language.NotFound);
            }
        }

        /// <summary>
        /// Gets all to-do items for the current user.
        /// </summary>
        /// <param name="token">Cancellation token for cancelling the operation.</param>
        /// <param name="status">Optional status filter for filtering to-do items.</param>
        /// <returns>List of ToDoRequest objects representing to-do items.</returns>
        [HttpGet("ToDos"), Authorize(Roles = "Customer")]
        public async Task<ActionResult<List<ToDoRequest>>> GetTodos(CancellationToken token, Status status = 0)
        {
            try
            {
                var todos = await _userInterface.GetAllToDos(token, status);
                var result = todos.Adapt<List<ToDoRequest>>();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(Language.NotFound);

            }

        }



    }
}
