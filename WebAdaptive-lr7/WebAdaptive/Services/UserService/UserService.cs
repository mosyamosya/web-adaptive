using WebAdaptive.Models;
using Bogus;

namespace WebAdaptive.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly List<UserModel> _users = new List<UserModel>();
        private static int id = 1;
        public UserService()
        {
            try
            {
                var faker = new Faker<UserModel>()
                    .RuleFor(u => u.Id, f => f.UniqueIndex + 1)
                    .RuleFor(u => u.Username, f => f.Internet.UserName())
                    .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.Username))
                    .RuleFor(u => u.Password, f => BCrypt.Net.BCrypt.HashPassword(f.Internet.Password()));

                _users.AddRange(faker.Generate(10));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UserService constructor: {ex.Message}");
            }
        }

        public async Task<List<UserModel>> GetAllUsers()
        {
            try
            {
                return _users;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllUsers method: {ex.Message}");
                return null;
            }
        }

        public async Task<UserModel> GetUserById(int id)
        {
            try
            {
                return _users.FirstOrDefault(u => u.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUserById method: {ex.Message}");
                return null;
            }
        }

        public async Task AddUser(UserModel user)
        {
            try
            {
                if (user == null)
                    throw new ArgumentNullException(nameof(user));

                user.Id = id++;
                _users.Add(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddUser method: {ex.Message}");
            }
        }

        public async Task UpdateUser(int id, UserModel user)
        {
            try
            {
                if (user == null)
                    throw new ArgumentNullException(nameof(user));

                var index = _users.FindIndex(u => u.Id == id);
                if (index != -1)
                {
                    _users[index] = user;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateUser method: {ex.Message}");
            }
        }

        public async Task DeleteUser(int id)
        {
            try
            {
                var user = _users.FirstOrDefault(u => u.Id == id);
                if (user != null)
                {
                    _users.Remove(user);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteUser method: {ex.Message}");
            }
        }
    }
}
