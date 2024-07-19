using DemoApi.Models;

namespace DemoApi.Services
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
        Task<User> Register(string username, string email, string password);
        Task<User> GetById(int id);
    }

}
