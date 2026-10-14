using PlannerApplication.Repositories;
using PlannerApplication.Services;
using PlannerApplication.View;

namespace PlannerApplication
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TaskRepository taskRepository = new TaskRepository();
            TaskService taskService = new TaskService(taskRepository);
            AuthService authService = new AuthService();
            TaskMenu taskMenu = new TaskMenu(taskService);
            AuthMenu consoleView = new AuthMenu(authService, taskMenu);
            consoleView.DisplayTaskMenu();
        }
    }
}
