using PlannerApplication.Models;
namespace PlannerApplication.Repositories
{
    internal class UserRepository
    {
        private List<User> users;
        UserJson userjson = new UserJson();
        public UserRepository()
        {
            users = new List<User>();
        }
        public void AddUser(User user)
        {
            users.Add(user);
            userjson.SaveUsers(users);
        }

        public List<User> GetUsers()
        {
            return userjson.GetUsers();
        }
    }
}
