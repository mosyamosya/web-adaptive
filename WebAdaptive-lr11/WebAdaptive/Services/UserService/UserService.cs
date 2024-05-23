using WebAdaptive.Models;
using Bogus;
using WebAdaptive.Services.AuthService;
using Serilog;

namespace WebAdaptive.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly List<UserModel> _users = new List<UserModel>();
        private static int id = 1;
        private readonly IAuthService _authService;



        public UserService(IAuthService authService)
        {
            _authService = authService;
            if (!_users.Any())
            {
                InitializeUsers();
            }
        }
        private void InitializeUsers()
        {
            try
            {
                var faker = new Faker<UserModel>()
                    .RuleFor(u => u.Id, _ => id++)
                    .RuleFor(u => u.Username, f => f.Internet.UserName())
                    .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.Username))
                    .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                    .RuleFor(u => u.LastName, f => f.Name.LastName())
                    .RuleFor(u => u.DateOfBirth, f => f.Date.Past(18))
                    .RuleFor(u => u.HashedPassword, f => BCrypt.Net.BCrypt.HashPassword(f.Internet.Password()))
                    .RuleFor(u => u.LastLoginDate, f => f.Date.Recent())
                    .RuleFor(u => u.LoginAttempts, f => f.Random.Number(0, 10));


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

        public async Task<bool> GetUserByName(UserModel userModel)
        {
            var username = userModel.Username?.ToLower(); // for case-insensitive comparison
            var user = _users.FirstOrDefault(u => u.Username?.ToLower() == username);

            if (user != null && _authService.HashPassword(userModel.HashedPassword) == user.HashedPassword)
            {
                user.LastLoginDate = DateTime.Now;
                user.LoginAttempts++; 
                await UpdateUser(user.Id, user);

                return true;
            }


            return false;
        }

        public bool UserExists(string username, string email)
        {
            var user = _users.FirstOrDefault(u => u.Username == username || u.Email == email);
            return user != null;
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
