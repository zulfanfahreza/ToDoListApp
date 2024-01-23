using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using ToDoListApp.Models;
using ToDoListApp.Utilities;

namespace ToDoListApp.Services
{
    public class LoginService : ILoginService
    {
        private IConfiguration _config;
        private ILogging _logger;
        public LoginService(IConfiguration config, ILogging logger)
        {
            _config = config;
            _logger = logger;
        }

        public LoginRequestModel AuthenticateUser(LoginRequestModel request)
        {
            _logger.LogDebug("LoginService.AuthenticateUser", $"Start authenticating user with request: {JsonConvert.SerializeObject(request)}");
            LoginRequestModel user = null;

            if (request.Username == _config["Authentication:Username"] &&  request.Password == _config["Authentication:Password"])
            {
                user = new LoginRequestModel
                {
                    Username = request.Username,
                    Password = request.Password,
                };
            }

            return user;
        }

        public string GenerateJsonWebToken()
        {
            _logger.LogDebug("LoginService.GenerateJsonWebToken", "Start generating Json Web Token");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var securityToken = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                null,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            _logger.LogDebug("LoginService.GenerateJsonWebToken", $"Generated Token: {JsonConvert.SerializeObject(token)}");
            return token;
        }
    }
}
