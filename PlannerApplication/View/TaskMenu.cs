using PlannerApplication.Models;
using PlannerApplication.Services;

namespace PlannerApplication.View
{
    public class TaskMenu
    {
        private readonly TaskService taskService;
        private readonly string[] menu = { "Add Task", "View Tasks", "Logout" };
        private readonly string[] taskActions = { "Edit", "Delete", "Back" };
        private const int ContentY = 5;
        private string searchText = string.Empty;
        private int menuSelection;
        private int taskSelection;
        private int actionSelection;
        private int statusSelection;
        private int mainFocus;
        private bool sortByStartDate;
        private bool statusFocus = true;

        public TaskMenu(TaskService taskService)
        {
            this.taskService = taskService;
        }

        public void DisplayTaskMenu(Guid userId)
        {
            DrawDashBoardBoundary();
            Console.CursorVisible = false;
            bool inTaskMenu = true;
            ToDoTask? focusedTask = null;

            while (inTaskMenu)
            {
                List<ToDoTask> tasks = GetDisplayedTasks(userId);
                if (taskSelection >= tasks.Count) taskSelection = Math.Max(0, tasks.Count - 1);
                DrawMenu();
                if (focusedTask == null)
                    DrawTaskList(tasks);
                else
                    DrawFocusedTask(focusedTask);

                ConsoleKey key = Console.ReadKey(true).Key;
                if (focusedTask != null)
                {
                    if (key == ConsoleKey.Tab) statusFocus = !statusFocus;
                    else if (statusFocus && key == ConsoleKey.UpArrow) statusSelection = statusSelection == 0 ? 2 : statusSelection - 1;
                    else if (statusFocus && key == ConsoleKey.DownArrow) statusSelection = (statusSelection + 1) % 3;
                    else if (!statusFocus && key == ConsoleKey.LeftArrow) actionSelection = actionSelection == 0 ? taskActions.Length - 1 : actionSelection - 1;
                    else if (!statusFocus && key == ConsoleKey.RightArrow) actionSelection = (actionSelection + 1) % taskActions.Length;
                    else if (key == ConsoleKey.Enter && statusFocus)
                    {
                        Result result = taskService.UpdateTaskStatus(focusedTask.Id, (TaskState)statusSelection, userId);
                        if (result.IsSuccess)
                        {
                            focusedTask = taskService.ViewTasks(userId).FirstOrDefault(t => t.Id == focusedTask.Id);
                            if (focusedTask != null) statusSelection = (int)focusedTask.Status;
                        }
                        else
                        {
                            ShowResult(result.ErrorMessage);
                            WaitForKey();
                        }
                    }
                    else if (key == ConsoleKey.Enter && !statusFocus)
                    {
                        if (actionSelection == 0)
                        {
                            Console.CursorVisible = true;
                            EditTaskHandler(focusedTask, userId);
                            Console.CursorVisible = false;
                            focusedTask = taskService.ViewTasks(userId).FirstOrDefault(t => t.Id == focusedTask.Id);
                            if (focusedTask == null) mainFocus = 2;
                        }
                        else if (actionSelection == 1)
                        {
                            Result result = taskService.DeleteTask(focusedTask.Id, userId);
                            ShowResult(result.IsSuccess ? "Task deleted successfully" : result.ErrorMessage);
                            WaitForKey();
                            if (result.IsSuccess)
                            {
                                focusedTask = null;
                                mainFocus = 2;
                            }
                        }
                        else focusedTask = null;
                    }
                    else if (key == ConsoleKey.Escape) focusedTask = null;
                    continue;
                }

                if (mainFocus == 0)
                {
                    if (key == ConsoleKey.UpArrow) menuSelection = menuSelection == 0 ? menu.Length - 1 : menuSelection - 1;
                    else if (key == ConsoleKey.DownArrow) menuSelection = (menuSelection + 1) % menu.Length;
                    else if (key == ConsoleKey.RightArrow || key == ConsoleKey.Tab) mainFocus = 1;
                    else if (key == ConsoleKey.Enter)
                    {
                        if (menuSelection == 0)
                        {
                            Console.CursorVisible = true;
                            AddTaskHandler(userId);
                            Console.CursorVisible = false;
                        }
                        else if (menuSelection == 1) mainFocus = 2;
                        else
                        {
                            Console.Clear();
                            inTaskMenu = false;
                        }
                    }
                }
                else if (key == ConsoleKey.Tab || key == ConsoleKey.RightArrow)
                    mainFocus = (mainFocus + 1) % 3;
                else if (key == ConsoleKey.LeftArrow)
                    mainFocus = (mainFocus + 2) % 3;
                else if (mainFocus == 1 && key == ConsoleKey.Enter)
                    SearchTasks(userId);
                else if (mainFocus == 2 && key == ConsoleKey.S)
                {
                    sortByStartDate = !sortByStartDate;
                    taskSelection = 0;
                }
                else if (mainFocus == 2 && key == ConsoleKey.UpArrow)
                    taskSelection = taskSelection == 0 ? Math.Max(0, tasks.Count - 1) : taskSelection - 1;
                else if (mainFocus == 2 && key == ConsoleKey.DownArrow)
                    taskSelection = tasks.Count == 0 ? 0 : (taskSelection + 1) % tasks.Count;
                else if (mainFocus == 2 && key == ConsoleKey.Enter && tasks.Count > 0)
                {
                    focusedTask = tasks[taskSelection];
                    actionSelection = 0;
                    statusSelection = (int)focusedTask.Status;
                    statusFocus = true;
                }
                else if (mainFocus == 2 && key == ConsoleKey.F)
                    mainFocus = 1;
            }
            Console.CursorVisible = true;
        }

        private List<ToDoTask> GetDisplayedTasks(Guid userId)
        {
            List<ToDoTask> tasks = string.IsNullOrWhiteSpace(searchText)
                ? taskService.ViewTasks(userId)
                : taskService.FilterTasksByName(userId, searchText);
            return sortByStartDate
                ? tasks.OrderBy(task => task.TargetStartDate).ToList()
                : tasks;
        }

        private void DrawMenu()
        {
            for (int i = 0; i < menu.Length; i++)
            {
                Console.SetCursorPosition(3, 3 + i);
                Console.Write(new string(' ', Math.Max(0, Console.WindowWidth / 5 - 5)));
                Console.SetCursorPosition(3, 3 + i);
                if (i == menuSelection)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write($" {menu[i]} ");
                    Console.ResetColor();
                }
                else Console.Write($" {menu[i]} ");
            }
        }

        private void DrawTaskList(List<ToDoTask> tasks)
        {
            ClearContentArea();
            DrawSearchBox();
            int x = ContentX;
            Console.SetCursorPosition(x, ContentY);
            Console.Write("Tasks");
            Console.SetCursorPosition(x, ContentY + 1);
            string sortLabel = sortByStartDate ? "Sort: Start date " : "Sort: Default";
            if (mainFocus == 2) Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"{sortLabel}   (S / Enter to toggle)");
            Console.ResetColor();

            if (tasks.Count == 0)
            {
                Console.SetCursorPosition(x, ContentY + 3);
                Console.Write("No tasks found.");
                return;
            }

            for (int i = 0; i < tasks.Count && ContentY + 3 + i < Console.WindowHeight - 2; i++)
            {
                ToDoTask task = tasks[i];
                Console.SetCursorPosition(x, ContentY + 3 + i);
                if (mainFocus == 2 && i == taskSelection) Console.ForegroundColor = ConsoleColor.Blue;
                string line = $"{i + 1}. {task.Name}   {task.TargetStartDate:yyyy-MM-dd} – {task.TargetEndDate:yyyy-MM-dd}   {task.Status}";
                int width = Math.Max(0, Console.WindowWidth - x - 3);
                Console.Write(line.Length > width ? line[..width] : line);
                Console.ResetColor();
            }
            Console.SetCursorPosition(x, Math.Min(ContentY + tasks.Count + 4, Console.WindowHeight - 2));
            Console.Write("Enter: open task   F: search   Tab: move focus");
        }

        private void DrawSearchBox()
        {
            int width = Math.Min(32, Math.Max(12, Console.WindowWidth - ContentX - 8));
            int x = Console.WindowWidth - width - 5;
            Console.SetCursorPosition(x, 2);
            Console.Write("Search tasks");
            Console.SetCursorPosition(x, 3);
            if (mainFocus == 1) Console.ForegroundColor = ConsoleColor.Blue;
            DrawFieldBox(width);
            Console.ResetColor();
            Console.SetCursorPosition(x + 2, 4);
            Console.Write(searchText.Length > width - 2 ? searchText[..(width - 2)] : searchText);
        }

        private void DrawFocusedTask(ToDoTask task)
        {
            ClearContentArea();
            DrawStatusSelector();
            int x = ContentX;
            Console.SetCursorPosition(x, ContentY);
            Console.Write(task.Name);
            Console.SetCursorPosition(x, ContentY + 2);
            Console.Write($"Description: {task.Description}");
            Console.SetCursorPosition(x, ContentY + 4);
            Console.Write($"Start date: {task.TargetStartDate:yyyy-MM-dd}");
            Console.SetCursorPosition(x, ContentY + 5);
            Console.Write($"End date:   {task.TargetEndDate:yyyy-MM-dd}");
            Console.SetCursorPosition(x, ContentY + 6);
            Console.Write($"Status:     {task.Status}");
            Console.SetCursorPosition(x, ContentY + 9);
            Console.Write(statusFocus ? "Tab: move to actions   : choose status   Enter: save status" : "Tab: move to status   : choose action   Enter: select");
            Console.SetCursorPosition(x, ContentY + 11);
            for (int i = 0; i < taskActions.Length; i++)
            {
                if (!statusFocus && i == actionSelection) Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"  {taskActions[i]}  ");
                Console.ResetColor();
            }
        }

        private void DrawStatusSelector()
        {
            int width = Math.Min(28, Math.Max(16, Console.WindowWidth - ContentX - 8));
            int x = Console.WindowWidth - width - 5;
            Console.SetCursorPosition(x, 2);
            Console.Write("Task status");
            for (int i = 0; i < 3; i++)
            {
                Console.SetCursorPosition(x, 3 + i);
                string option = $"  {(TaskState)i}";
                Console.Write(option.PadRight(width));
                if (i == statusSelection)
                {
                    Console.SetCursorPosition(x, 3 + i);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write($"> {(TaskState)i}".PadRight(width));
                    Console.ResetColor();
                }
            }
        }

        private void AddTaskHandler(Guid userId)
        {
            ClearContentArea();
            int x = ContentX;
            Console.SetCursorPosition(x, ContentY);
            Console.Write("Enter task name : ");
            string? name = Console.ReadLine();
            Console.SetCursorPosition(x, ContentY + 1);
            Console.Write("Enter task desc : ");
            string? desc = Console.ReadLine();
            Console.SetCursorPosition(x, ContentY + 2);
            Console.Write("Enter start date (yyyy-MM-dd) : ");
            if (!DateOnly.TryParse(Console.ReadLine(), out DateOnly startdate))
            {
                ShowResult("Invalid start date format.");
                WaitForKey();
                return;
            }
            Console.SetCursorPosition(x, ContentY + 3);
            Console.Write("Enter end date (yyyy-MM-dd) : ");
            if (!DateOnly.TryParse(Console.ReadLine(), out DateOnly enddate))
            {
                ShowResult("Invalid end date format.");
                WaitForKey();
                return;
            }
            Result result = taskService.AddTask(name ?? string.Empty, desc ?? string.Empty, startdate, enddate, userId);
            ShowResult(result.IsSuccess ? "Task added successfully" : result.ErrorMessage);
            WaitForKey();
        }

        private void SearchTasks(Guid userId)
        {
            int width = Math.Min(32, Math.Max(12, Console.WindowWidth - ContentX - 8));
            int x = Console.WindowWidth - width - 5;
            Console.SetCursorPosition(x + 1, 4);
            Console.Write(new string(' ', width));
            Console.SetCursorPosition(x + 2, 4);
            Console.CursorVisible = true;
            searchText = Console.ReadLine() ?? string.Empty;
            Console.CursorVisible = false;
            taskSelection = 0;
        }

        private void EditTaskHandler(ToDoTask task, Guid userId)
        {
            ClearContentArea();
            int x = ContentX;
            Console.SetCursorPosition(x, ContentY);
            Console.Write($"Task name ({task.Name}) : ");
            string name = Console.ReadLine() ?? string.Empty;
            Console.SetCursorPosition(x, ContentY + 1);
            Console.Write($"Task description ({task.Description}) : ");
            string desc = Console.ReadLine() ?? string.Empty;
            Console.SetCursorPosition(x, ContentY + 2);
            Console.Write($"Start date ({task.TargetStartDate:yyyy-MM-dd}) : ");
            string start = Console.ReadLine() ?? string.Empty;
            Console.SetCursorPosition(x, ContentY + 3);
            Console.Write($"End date ({task.TargetEndDate:yyyy-MM-dd}) : ");
            string end = Console.ReadLine() ?? string.Empty;
            DateOnly startDate = string.IsNullOrWhiteSpace(start) ? task.TargetStartDate : DateOnly.TryParse(start, out DateOnly parsedStart) ? parsedStart : default;
            DateOnly endDate = string.IsNullOrWhiteSpace(end) ? task.TargetEndDate : DateOnly.TryParse(end, out DateOnly parsedEnd) ? parsedEnd : default;
            if (startDate == default || endDate == default)
            {
                ShowResult("Invalid date format.");
                WaitForKey();
                return;
            }
            Result result = taskService.EditTask(task.Id,
                string.IsNullOrWhiteSpace(name) ? task.Name : name,
                string.IsNullOrWhiteSpace(desc) ? task.Description : desc,
                startDate, endDate, userId);
            ShowResult(result.IsSuccess ? "Task updated successfully" : result.ErrorMessage);
            WaitForKey();
        }

        private void ShowResult(string message)
        {
            Console.SetCursorPosition(ContentX, Math.Min(ContentY + 13, Console.WindowHeight - 2));
            Console.Write(new string(' ', Math.Max(0, Console.WindowWidth - ContentX - 3)));
            Console.SetCursorPosition(ContentX, Math.Min(ContentY + 13, Console.WindowHeight - 2));
            Console.Write(message);
        }

        private void ClearContentArea()
        {
            int width = Math.Max(0, Console.WindowWidth - ContentX - 2);
            for (int y = 2; y < Console.WindowHeight - 1; y++)
            {
                Console.SetCursorPosition(ContentX, y);
                Console.Write(new string(' ', width));
            }
        }

        private void DrawFieldBox(int length)
        {
            string line = new string('\u2500', length);
            string blankLine = new string(' ', length);
            (int x, int y) = Console.GetCursorPosition();
            Console.Write($"\u250C{line}\u2510");
            Console.SetCursorPosition(x, y + 1);
            Console.Write($"\u2502{blankLine}\u2502");
            Console.SetCursorPosition(x, y + 2);
            Console.Write($"\u2514{line}\u2518");
        }

        private static void WaitForKey()
        {
            if (!Console.IsInputRedirected) Console.ReadKey(true);
        }

        private static int ContentX => Console.WindowWidth / 5 + 2;

        public void DrawDashBoardBoundary()
        {
            string horline = new string('\u2500', Console.WindowWidth - 3);
            Console.SetCursorPosition(1, 1);
            Console.Write($"\u250C{horline}\u2510");
            for (int i = 2; i < Console.WindowHeight - 1; i++)
            {
                Console.SetCursorPosition(1, i);
                Console.Write("\u2502");
                Console.SetCursorPosition(Console.WindowWidth / 5, i);
                Console.Write("\u2502");
                Console.SetCursorPosition(Console.WindowWidth - 1, i);
                Console.Write("\u2502");
            }
            Console.SetCursorPosition(1, Console.WindowHeight - 1);
            Console.Write($"\u2514{horline}\u2518");
        }
    }
}