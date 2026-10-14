using PlannerApplication.Models;
using PlannerApplication.Repositories;
namespace PlannerApplication.Services
{
    public class TaskService
    {
        TaskRepository taskRepository;

        public TaskService(TaskRepository taskRepository) 
        {
            this.taskRepository = taskRepository;
        }
        public Result AddTask(string name, string desc, DateOnly startdate, DateOnly enddate, Guid userId)
        {
            ToDoTask task = new ToDoTask(name, desc, startdate, enddate, userId);
            taskRepository.AddTask(task);
            return Result.Success();
        }

        public List<ToDoTask> ViewTasks(Guid userid)
        {
            List<ToDoTask> tasks = taskRepository.GetTasks().ToList();
            return tasks.FindAll(p => p.UserId == userid);
        }
    }
}
