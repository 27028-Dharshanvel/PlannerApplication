using PlannerApplication.Models;
namespace PlannerApplication.Repositories
{
    public class TaskRepository
    {
        private List<ToDoTask> tasks;
        private TaskJson taskjson = new TaskJson();

        public TaskRepository()
        {
            tasks = taskjson.GetTasks();
        }

        public void AddTask(ToDoTask task)
        {
            if (task.Id == 0)
            {
                task.Id = tasks.Count > 0 ? tasks.Max(t => t.Id) + 1 : 1;
            }
            tasks.Add(task);
            taskjson.SaveTasks(tasks);
        }

        public List<ToDoTask> GetTasks()
        {
            return tasks;
        }
    }
}