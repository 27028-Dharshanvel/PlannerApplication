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
            if (string.IsNullOrWhiteSpace(name))
            {
                return Result.Failure("Task name cannot be empty");
            }

            if (enddate < startdate)
            {
                return Result.Failure("Target end date cannot be earlier than target start date");
            }

            if (userId == Guid.Empty)
            {
                return Result.Failure("Invalid user id");
            }

            ToDoTask task = new ToDoTask(name, desc, startdate, enddate, userId);
            taskRepository.AddTask(task);
            return Result.Success();
        }

        public List<ToDoTask> ViewTasks(Guid userid)
        {
            List<ToDoTask> tasks = taskRepository.GetTasks();
            if (tasks == null)
            {
                return new List<ToDoTask>();
            }
            return tasks.FindAll(p => p.UserId == userid);
        }
    }
}