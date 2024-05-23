using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebAdaptive.Models;
using WebAdaptive.Services.UserService;

namespace WebAdaptive.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]

    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        // GET /user
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsers();
                var response = new ResponseModel<List<UserModel>>
                {
                    Message = "Success",
                    StatusCode = HttpStatusCode.OK,
                    Data = users
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<object>
                {
                    Message = $"Internal server error: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null
                };
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }

        // GET /user/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserById(id);
                if (user == null)
                    return NotFound();

                var response = new ResponseModel<UserModel>
                {
                    Message = "Success",
                    StatusCode = HttpStatusCode.OK,
                    Data = user
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var responseError = new ResponseModel<object>
                {
                    Message = $"Internal server error: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null
                };
                return StatusCode((int)HttpStatusCode.InternalServerError, responseError);
            }
        }

        // POST /user
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserModel user)
        {
            try
            {
                if (user == null)
                    return BadRequest();

                await _userService.AddUser(user);
                var responseCreated = new ResponseModel<UserModel>
                {
                    Message = "User created successfully.",
                    StatusCode = HttpStatusCode.Created,
                    Data = user
                };
                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, responseCreated);
            }
            catch (Exception ex)
            {
                var responseError = new ResponseModel<object>
                {
                    Message = $"Internal server error: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null
                };
                return StatusCode((int)HttpStatusCode.InternalServerError, responseError);
            }
        }

        // PUT /user/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UserModel user)
        {
            try
            {
                if (user == null || id != user.Id)
                    return BadRequest();

                await _userService.UpdateUser(id, user);
                var responseNoContent = new ResponseModel<object>
                {
                    Message = "User updated successfully.",
                    StatusCode = HttpStatusCode.NoContent,
                    Data = null
                };
                return NoContent();
            }
            catch (Exception ex)
            {
                var responseError = new ResponseModel<object>
                {
                    Message = $"Internal server error: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null
                };
                return StatusCode((int)HttpStatusCode.InternalServerError, responseError);
            }
        }

        // DELETE /user/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _userService.DeleteUser(id);
                var responseNoContent = new ResponseModel<object>
                {
                    Message = "User deleted successfully.",
                    StatusCode = HttpStatusCode.NoContent,
                    Data = null
                };
                return NoContent();
            }
            catch (Exception ex)
            {
                var responseError = new ResponseModel<object>
                {
                    Message = $"Internal server error: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null
                };
                return StatusCode((int)HttpStatusCode.InternalServerError, responseError);
            }
        }
    }
}
