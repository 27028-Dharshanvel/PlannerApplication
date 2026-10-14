using PlannerApplication.Models;
using PlannerApplication.Services;

namespace PlannerApplication.View
{
    public class TaskMenu
    {
        private Guid UserId;

        public TaskMenu(TaskService taskService)
        {
            this.taskService = taskService;
        }
        TaskService taskService;
        public void DisplayTaskMenu(Guid userId)
        {
            bool inTaskMenu = true;
            while (inTaskMenu)
            {
                Console.Clear();
                Console.WriteLine(@" --------Task menu----------

1.Add task
2.View all tasks");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        if (AddTaskHandler(userId))
                        {
                            Console.WriteLine("Task added successfully");
                        }
                        break;
                    case "2":
                        List<ToDoTask> tasks = taskService.ViewTasks(userId);
                        if(tasks == null)
                        {
                            Console.WriteLine("No tasks found");
                            break;
                        }
                        foreach (ToDoTask task in tasks)
                        {
                            Console.WriteLine(@$"Name : {task.Name}
Desc : {task.Description}
StartDate : {task.TargetStartDate}
EndDate : {task.TargetEndDate}
Status : {task.Status}
-------------------------------------------------");
                        }
                        break;
                    case "3":
                        inTaskMenu = false;
                        break;
                }
                Console.ReadKey();
            }
        }

        public bool AddTaskHandler(Guid userId)
        {
            Console.WriteLine("Enter task name : ");
            string name = Console.ReadLine();
            Console.WriteLine("Enter task desc : ");
            string desc = Console.ReadLine();
            Console.WriteLine("Enter start date : ");
            DateOnly startdate = DateOnly.Parse(Console.ReadLine());
            Console.WriteLine("Enter End date : ");
            DateOnly enddate = DateOnly.Parse(Console.ReadLine());

            taskService.AddTask(name, desc, startdate, enddate, userId);
            return true;
        }

    }
}
