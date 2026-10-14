using PlannerApplication.Models;
namespace PlannerApplication.Repositories
{
    public class TaskRepository
    {
        private List<ToDoTask> tasks = new List<ToDoTask>();
        TaskJson taskjson = new TaskJson();

        public void AddTask(ToDoTask task)
        {
            tasks.Add(task);
            taskjson.SaveTasks(tasks);
        }


        public List<ToDoTask> GetTasks()
        {
            return taskjson.GetTasks();
        }
    }
}
