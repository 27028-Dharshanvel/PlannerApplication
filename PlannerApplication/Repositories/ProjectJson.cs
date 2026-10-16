using System.Text.Json;
using PlannerApplication.Models;

namespace PlannerApplication.Repositories
{
    public class ProjectJson
    {
        private readonly string _filepath = "projects.json";
        private readonly JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        public void SaveProjects(List<Project> projects)
        {
            if (projects == null) return;

            string content = JsonSerializer.Serialize(projects, _options);
            File.WriteAllText(_filepath, content);
        }

        public List<Project> GetProjects()
        {
            if (!File.Exists(_filepath))
            {
                return new List<Project>();
            }

            string content = File.ReadAllText(_filepath);
            if (string.IsNullOrWhiteSpace(content))
            {
                return new List<Project>();
            }

            return JsonSerializer.Deserialize<List<Project>>(content, _options) ?? new List<Project>();
        }
    }
}