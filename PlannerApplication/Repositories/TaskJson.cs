using PlannerApplication.Models;
using System.Text.Json;

namespace PlannerApplication.Repositories
{
    internal class TaskJson
    {
        private readonly string _filepath = "tasks.json";
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
            Converters = { new DateOnlyJsonConverter() },
        };

        public void SaveTasks(List<ToDoTask> tasks)
        {
            if (tasks == null) return;

            string content = JsonSerializer.Serialize(tasks, _jsonOptions);
            File.WriteAllText(_filepath, content);
        }

        public List<ToDoTask> GetTasks()
        {
            if (!File.Exists(_filepath))
            {
                return new List<ToDoTask>();
            }

            string content = File.ReadAllText(_filepath);
            return JsonSerializer.Deserialize<List<ToDoTask>>(content) ?? new List<ToDoTask>();
        }
    }
}
