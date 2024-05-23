using WebAdaptive.Models;

namespace WebAdaptive.Services.UserService
{
    public interface IUserService
    {
        Task<List<UserModel>> GetAllUsers();
        Task<UserModel> GetUserById(int id);
        Task AddUser(UserModel user);
        Task UpdateUser(int id, UserModel user);
        Task DeleteUser(int id);
    }
}
