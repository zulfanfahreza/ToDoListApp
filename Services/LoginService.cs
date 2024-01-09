using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using ToDoListApp.Models;

namespace ToDoListApp.Services
{
    public class LoginService
    {
        private IConfiguration _config;
        public LoginService(IConfiguration config)
        {
            _config = config;
        }

        public LoginRequestModel AuthenticateUser(LoginRequestModel userInfo)
        {
            LoginRequestModel user = null;

            if (userInfo.Username == "zulfanfahreza" &&  userInfo.Password == "zulfan12345")
            {
                user = new LoginRequestModel
                {
                    Username = userInfo.Username,
                    Password = userInfo.Password,
                };
            }

            return user;
        }

        public string GenerateJsonWebToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var securityToken = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                null,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }
    }
}
