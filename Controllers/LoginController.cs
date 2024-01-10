using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Asp.Versioning;
using Newtonsoft.Json;
using ToDoListApp.Models;
using ToDoListApp.Services;

namespace ToDoListApp.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class LoginController : ControllerBase
    {
        private ILoginService _loginService;
        
        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginRequestModel loginRequest)
        {
            try
            {
                var user = _loginService.AuthenticateUser(loginRequest);

                if (user == null)
                {
                    var errorResponse = new ErrorResponseModel
                    {
                        Error = $"{nameof(Login)} Exception",
                        Message = $"User with request {JsonConvert.SerializeObject(loginRequest)} is unauthorized to access",
                        StatusCode = StatusCodes.Status401Unauthorized
                    };
                    return Unauthorized(errorResponse);
                }

                var token = _loginService.GenerateJsonWebToken();

                return Ok(token);
            }
            catch (Exception ex)
            {
                var errorResponse = new ErrorResponseModel
                {
                    Error = $"{nameof(Login)} Exception",
                    Message = ex.Message,
                    StatusCode = StatusCodes.Status400BadRequest
                };

                return BadRequest(errorResponse);
            }
            
        }
    }
}
