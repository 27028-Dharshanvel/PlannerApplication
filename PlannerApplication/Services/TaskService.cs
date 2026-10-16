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

        public Result EditTask(Guid id, string name, string desc, DateOnly startdate, DateOnly enddate, Guid userId)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure("Task name cannot be empty");
            if (enddate < startdate)
                return Result.Failure("Target end date cannot be earlier than target start date");
            if (userId == Guid.Empty)
                return Result.Failure("Invalid user id");

            ToDoTask? task = ViewTasks(userId).FirstOrDefault(t => t.Id == id);
            if (task == null)
                return Result.Failure("Task not found");

            task.Name = name;
            task.Description = desc;
            task.TargetStartDate = startdate;
            task.TargetEndDate = enddate;
            taskRepository.UpdateTask(task);
            return Result.Success();
        }

        public Result DeleteTask(Guid id, Guid userId)
        {
            if (userId == Guid.Empty)
                return Result.Failure("Invalid user id");
            ToDoTask? task = ViewTasks(userId).FirstOrDefault(t => t.Id == id);
            if (task == null)
                return Result.Failure("Task not found");
            taskRepository.DeleteTask(id);
            return Result.Success();
        }

        public Result UpdateTaskStatus(Guid id, TaskState status, Guid userId)
        {
            if (userId == Guid.Empty)
                return Result.Failure("Invalid user id");
            if (!Enum.IsDefined(status))
                return Result.Failure("Invalid task status");

            ToDoTask? task = ViewTasks(userId).FirstOrDefault(t => t.Id == id);
            if (task == null)
                return Result.Failure("Task not found");

            task.Status = status;
            if (!taskRepository.UpdateTask(task))
                return Result.Failure("Task not found");
            return Result.Success();
        }

        public List<ToDoTask> SortTasks(Guid userId)
        {
            return ViewTasks(userId).OrderBy(task => task.TargetStartDate).ToList();
        }

        public List<ToDoTask> FilterTasksByName(Guid userId, string name)
        {
            return ViewTasks(userId).Where(task => task.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }
}