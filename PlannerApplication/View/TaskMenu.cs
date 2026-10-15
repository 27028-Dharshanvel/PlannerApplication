using PlannerApplication.Models;
using PlannerApplication.Services;

namespace PlannerApplication.View
{
    public class TaskMenu
    {
        TaskService taskService;

        public TaskMenu(TaskService taskService)
        {
            this.taskService = taskService;
        }

        public void DisplayTaskMenu(Guid userId)
        {
            bool inTaskMenu = true;
            while (inTaskMenu)
            {
                if (!Console.IsOutputRedirected)
                {
                    try { Console.Clear(); } catch { }
                }
                Console.WriteLine(@" --------Task menu----------

1.Add task
2.View all tasks
3.Logout");

                string? choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        if (AddTaskHandler(userId))
                        {
                            Console.WriteLine("Task added successfully");
                        }
                        if (!Console.IsInputRedirected) Console.ReadKey();
                        break;
                    case "2":
                        List<ToDoTask> tasks = taskService.ViewTasks(userId);
                        if (tasks == null || tasks.Count == 0)
                        {
                            Console.WriteLine("No tasks found");
                        }
                        else
                        {
                            foreach (ToDoTask task in tasks)
                            {
                                Console.WriteLine(@$"

Name : {task.Name}
Desc : {task.Description}
StartDate : {task.TargetStartDate}
EndDate : {task.TargetEndDate}
Status : {task.Status}
-------------------------------------------------");
                            }
                        }
                        if (!Console.IsInputRedirected) Console.ReadKey();
                        break;
                    case "3":
                        inTaskMenu = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Press any key to continue...");
                        if (!Console.IsInputRedirected) Console.ReadKey();
                        break;
                }
            }
        }

        public bool AddTaskHandler(Guid userId)
        {
            Console.Write("Enter task name : ");
            string? name = Console.ReadLine();
            Console.Write("Enter task desc : ");
            string? desc = Console.ReadLine();
            Console.Write("Enter start date (yyyy-MM-dd) : ");
            if (!DateOnly.TryParse(Console.ReadLine(), out DateOnly startdate))
            {
                Console.WriteLine("Invalid start date format.");
                return false;
            }
            Console.Write("Enter End date (yyyy-MM-dd) : ");
            if (!DateOnly.TryParse(Console.ReadLine(), out DateOnly enddate))
            {
                Console.WriteLine("Invalid end date format.");
                return false;
            }

            Result result = taskService.AddTask(name ?? string.Empty, desc ?? string.Empty, startdate, enddate, userId);
            if (!result.IsSuccess)
            {
                Console.WriteLine(result.ErrorMessage);
                return false;
            }
            return true;
        }

    }
}