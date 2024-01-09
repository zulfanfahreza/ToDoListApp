using ToDoListApp.Models;

namespace ToDoListApp.Services
{
    public interface ILoginService
    {
        LoginRequestModel AuthenticateUser(LoginRequestModel request);
        string GenerateJsonWebToken();

    }
}
