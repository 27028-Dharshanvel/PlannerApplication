using PlannerApplication.Models;
namespace PlannerApplication.Repositories
{
    public class TaskRepository
    {
        private List<ToDoTask> tasks = new List<ToDoTask>();

        public void AddTask(ToDoTask task)
        {
            tasks.Add(task);
        }

        public List<ToDoTask> GetTasks()
        {
            return tasks;
        }
    }
}
