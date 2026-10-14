using PlannerApplication.Repositories;
using PlannerApplication.Models;
namespace PlannerApplication.Services
{
    public class AuthService
    {
        UserRepository userRepository = new UserRepository();
        public Result AddUser(string username, string password)
        {
            if (userRepository.GetUsers().FirstOrDefault(p => p.Name == username) != null)
            {
                return Result.Failure("Username already exists ...Try another username");
            }

            User user = new User(username, password);

            userRepository.AddUser(user);
            return Result.Success();
        }

        public GuidResult LoginUser(string username, string password)
        {
            if (userRepository.GetUsers().FirstOrDefault(p => p.Name == username && p.Password == password) == null)
            {
                return GuidResult.Failure("Invalid username or password");
            }
            return GuidResult.Success(userRepository.GetUsers().FirstOrDefault(p => p.Name == username && p.Password == password).Id);
        }
    }
}
