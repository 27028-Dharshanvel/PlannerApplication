using PlannerApplication.Repositories;
using PlannerApplication.Models;
namespace PlannerApplication.Services
{
    public class AuthService
    {
        UserRepository userRepository = new UserRepository();
        public Result AddUser(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return Result.Failure("Username and password cannot be empty");
            }

            if (userRepository.GetUsers().Any(p => string.Equals(p.Name, username, StringComparison.OrdinalIgnoreCase)))
            {
                return Result.Failure("Username already exists ...Try another username");
            }

            User user = new User(username, password);

            userRepository.AddUser(user);
            return Result.Success();
        }

        public GuidResult LoginUser(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return GuidResult.Failure("Invalid username or password");
            }

            var user = userRepository.GetUsers().FirstOrDefault(p => string.Equals(p.Name, username, StringComparison.OrdinalIgnoreCase) && p.Password == password);
            if (user == null)
            {
                return GuidResult.Failure("Invalid username or password");
            }
            return GuidResult.Success(user.Id);
        }
    }
}