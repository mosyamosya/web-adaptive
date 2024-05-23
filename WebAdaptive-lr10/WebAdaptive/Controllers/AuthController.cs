using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using WebAdaptive.Models;
using WebAdaptive.Services.AuthService;
using WebAdaptive.Services.UserService;

namespace WebAdaptive.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;



        public AuthController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register(UserModel newUser)
        {
            try
            {
                var existingUser = _userService.UserExists(newUser.Username, newUser.Email);
                if (existingUser)
                {
                    return BadRequest("User with these username or email already exists.");
                }
                var user = await _authService.RegisterUser(newUser);
                _userService.AddUser(user);

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

        [HttpPost("Login")]
        public async Task<ActionResult> Login(UserModel userModel)
        {
            try
            {
                var validUser = await _userService.GetUserByName(userModel);

                await Console.Out.WriteLineAsync("User Model: " + JsonSerializer.Serialize(userModel));


                if (!validUser)
                {
                    return Unauthorized("Invalid username or password");
                }
                var token = _authService.GenerateJwtToken(userModel.Username);

                if (token == null)
                {
                    return Unauthorized("Invalid Attempt...");
                }
                var response = new ResponseModel<object>
                {
                    Message = "Success",
                    StatusCode = HttpStatusCode.OK,
                    Data = token
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
    }
}
