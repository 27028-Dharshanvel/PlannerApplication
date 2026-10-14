using System.Text.Json;
using PlannerApplication.Models;

namespace PlannerApplication.Repositories
{
    public class UserJson
    {
        private readonly string _filepath = "users.json";
        private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

        public void SaveUsers(List<User> users)
        {
            if (users == null) return;

            string content = JsonSerializer.Serialize(users, _options);
            File.WriteAllText(_filepath, content);
        }

        public List<User> GetUsers()
        {
            if (!File.Exists(_filepath))
            {
                return new List<User>();
            }

            string content = File.ReadAllText(_filepath);
            return JsonSerializer.Deserialize<List<User>>(content) ?? new List<User>();
        }
    }
}
