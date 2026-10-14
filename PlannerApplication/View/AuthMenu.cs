using PlannerApplication.Services;

namespace PlannerApplication.View
{
    public class AuthMenu
    {
        AuthService authService;
        TaskMenu taskMenu;
        public AuthMenu(AuthService authService,TaskMenu taskmenu)
        {
            this.authService = authService;
            this.taskMenu = taskmenu;
        }

        public void DisplayTaskMenu()
        {
            Console.Title = "Planner Application";
            bool isAppRunning = true;
            while (isAppRunning)
            {
                Console.Clear();
                Console.WriteLine(@"----------Planner Application----------

1.Sign up
2.Login");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        RegisterUser();
                        break;

                    case "2":
                        LoginUser();
                        break;

                    case "3":
                        isAppRunning = false;
                        break;
                }
                Console.ReadKey();
            }
        }

        public void RegisterUser()
        {
            Console.Write("Enter UserName : ");
            string username = Console.ReadLine();
            Console.Write("\nEnter Password : ");
            string password = Console.ReadLine();
            Result result = authService.AddUser(username, password);
            if (result.IsSuccess)
            {
                Console.WriteLine("Register successful");
            }
            else
            {
                Console.WriteLine(result.ErrorMessage);
            }
        }

        public bool LoginUser()
        {
            Console.Write("Enter UserName : ");
            string username = Console.ReadLine();
            Console.Write("\nEnter Password : ");
            string password = Console.ReadLine();
            GuidResult result = authService.LoginUser(username, password);
            if (result.IsSuccess)
            {
                Console.WriteLine("Login successfull");
                taskMenu.DisplayTaskMenu(result.Id);
                return true;
            }
            else
            {
                Console.WriteLine(result.ErrorMessage);
                return false;
            }
        }
    }
}
