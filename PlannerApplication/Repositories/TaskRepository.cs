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
            if (task.Id == Guid.Empty) task.Id = Guid.NewGuid();
            tasks.Add(task);
            taskjson.SaveTasks(tasks);
        }

        public List<ToDoTask> GetTasks()
        {
            return tasks;
        }

        public bool UpdateTask(ToDoTask task)
        {
            ToDoTask? existingTask = tasks.FirstOrDefault(t => t.Id == task.Id);
            if (existingTask == null) return false;

            existingTask.Name = task.Name;
            existingTask.Description = task.Description;
            existingTask.TargetStartDate = task.TargetStartDate;
            existingTask.TargetEndDate = task.TargetEndDate;
            existingTask.Status = task.Status;
            taskjson.SaveTasks(tasks);
            return true;
        }

        public bool DeleteTask(Guid id)
        {
            ToDoTask? task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return false;

            tasks.Remove(task);
            taskjson.SaveTasks(tasks);
            return true;
        }
    }
}