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
            Console.Clear();
            DrawDashBoardBoundary();
            bool inTaskMenu = true;

            string[] menu = { "Add Task", "View All Tasks", "Logout" };
            int select = 0;
            int curSelect = 0;
            Console.SetCursorPosition(3, 3);
            var navCurPosition = Console.GetCursorPosition();

            while (inTaskMenu)
            {   
                for(int i = 0; i < menu.Length; i++)
                {
                    Console.Write("               ");
                    Console.SetCursorPosition(3, 4 + i);
                }
                Console.SetCursorPosition(3, 3);
                for (int i = 0; i < menu.Length; i++)
                {
                    if (i == select)
                    {
                        navCurPosition = Console.GetCursorPosition();
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write($" {menu[i]} ");
                        Console.ResetColor();
                        curSelect = i;
                    }
                    else
                    {
                        Console.SetCursorPosition(navCurPosition.Item1, navCurPosition.Item2+i);
                        Console.Write($" {menu[i]} ");
                    }
                }

                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.UpArrow)
                {
                    select = select == 0 ? menu.Length - 1 : select - 1;
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    select = select == menu.Length - 1 ? 0 : select + 1;
                }
                else if(key == ConsoleKey.Enter)
                {
                    switch (select)
                    {
                        case 0:
                            if (AddTaskHandler(userId))
                            {
                                Console.WriteLine("Task added successfully");
                            }
                            if (!Console.IsInputRedirected) Console.ReadKey();
                            break;
                        case 1:
                            break;
                        case 2:
                            inTaskMenu = false;
                            break;

                    }
                }

//                Console.SetCursorPosition(3, 3);
//                Console.Write(@"Planner App

//   1.Add task
//   2.View all tasks
//   3.Logout");

//                string? choice = Console.ReadLine();
//                switch (choice)
//                {
//                    case "1":
//                        if (AddTaskHandler(userId))
//                        {
//                            Console.WriteLine("Task added successfully");
//                        }
//                        if (!Console.IsInputRedirected) Console.ReadKey();
//                        break;
//                    case "2":
//                        List<ToDoTask> tasks = taskService.ViewTasks(userId);
//                        if (tasks == null || tasks.Count == 0)
//                        {
//                            Console.WriteLine("No tasks found");
//                        }
//                        else
//                        {
//                            foreach (ToDoTask task in tasks)
//                            {
//                                Console.WriteLine(@$"

//Name : {task.Name}
//Desc : {task.Description}
//StartDate : {task.TargetStartDate}
//EndDate : {task.TargetEndDate}
//Status : {task.Status}
//-------------------------------------------------");
//                            }
//                        }
//                        if (!Console.IsInputRedirected) Console.ReadKey();
//                        break;
//                    case "3":
//                        inTaskMenu = false;
//                        break;
//                    default:
//                        Console.WriteLine("Invalid option. Press any key to continue...");
//                        if (!Console.IsInputRedirected) Console.ReadKey();
//                        break;
//              }
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

        public void DrawDashBoardBoundary()
        {
            string horline = new string('\u2500', Console.WindowWidth - 3);
            Console.SetCursorPosition(1, 1);
            Console.Write($"\u250C{horline}\u2510");
            for (int i = 2; i < Console.WindowHeight - 1; i++)
            {
                //Left Boundary line
                Console.SetCursorPosition(1, i);
                Console.Write("\u2502");

                //Sidebar line
                Console.SetCursorPosition(Console.WindowWidth/5, i);
                Console.Write("\u2502");

                //Right boundary line
                Console.SetCursorPosition(Console.WindowWidth - 1, i);
                Console.Write("\u2502");
            }
            Console.SetCursorPosition(1, Console.WindowHeight - 1);
            Console.Write($"\u2514{horline}\u2518");
        }
    }
}