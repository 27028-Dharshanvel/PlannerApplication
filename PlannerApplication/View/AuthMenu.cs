using PlannerApplication.Services;

using System.Reflection.Metadata.Ecma335;

namespace PlannerApplication.View
{
    public class AuthMenu
    {
        AuthService authService;
        TaskMenu taskMenu;
        public AuthMenu(AuthService authService, TaskMenu taskmenu)
        {
            this.authService = authService;
            this.taskMenu = taskmenu;
        }


        public void DisplayTaskMenu()
        {
            Console.Title = "Planner Application";
            bool isAppRunning = true;
            //DrawBoundary();
            string[] menu = { "Login", "Signup" };
            int select = 0;

            Console.CursorVisible = false;
            //            while (isAppRunning)
            //            {
            //                if (!Console.IsOutputRedirected)
            //                {
            //                    try { Console.Clear(); } catch { }
            //                }
            //                AnsiConsole.Write(
            //                   new FigletText("Planner Application")
            //                       .Centered()
            //                       .Color(Color.Red)
            //                );
            //DrawUsernameBox();
            //DrawPasswordBox();
            //                //Console.WriteLine($"width : {Console.WindowWidth} height : { Console.WindowHeight}");
            ////                Console.WriteLine(@"----------Planner Application----------

            ////1.Sign up 
            ////2.Login
            ////3.Exit");
            //                string? choice = Console.ReadLine();
            //                switch (choice)
            //                {
            //                    case "1":
            //                        RegisterUser();
            //                        if (!Console.IsInputRedirected) Console.ReadKey();
            //                        break;

            //                    case "2":
            //                        if (!LoginUser())
            //                        {
            //                            if (!Console.IsInputRedirected) Console.ReadKey();
            //                        }
            //                        break;

            //                    case "3":
            //                        isAppRunning = false;
            //                        break;

            //                    default:
            //                        Console.WriteLine("Invalid option. Press any key to continue...");
            //                        if (!Console.IsInputRedirected) Console.ReadKey();
            //                        break;
            //                }
            //            }
            while (isAppRunning)
            {
                DrawBoundary();
                DrawUsernameBox();
                DrawPasswordBox();
                Console.SetCursorPosition(Console.WindowWidth / 2 - 9, Console.WindowHeight / 2 + 8);
                Console.Write("                                  ");
                Console.SetCursorPosition(Console.WindowWidth / 2 - 5, Console.WindowHeight / 2 - 5);
                Console.Write("                             ");
                Console.SetCursorPosition(Console.WindowWidth / 2 - 5, Console.WindowHeight / 2 - 5);
                for (int i = 0; i < menu.Length; i++)
                {
                    if (i == select)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write($" {menu[i]} ");
                        Console.ResetColor();
                    }
                    else Console.Write($" {menu[i]} ");
                }

                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.LeftArrow)
                {
                    select = select == 0 ? menu.Length - 1 : select - 1;
                }
                else if (key == ConsoleKey.RightArrow)
                {
                    select = select == menu.Length - 1 ? 0 : select + 1;
                }

                else if (key == ConsoleKey.Enter)
                {
                    Console.CursorVisible = true;
                    if (select == 0)
                    {
                        if (!LoginUser())
                        {
                            if (!Console.IsInputRedirected) Console.ReadKey();
                        }
                        continue;
                    }

                    if (select == 1)
                    {
                        RegisterUser();
                        if (!Console.IsInputRedirected) Console.ReadKey();
                        continue;
                    }
                }
            }
        }

        public void RegisterUser()
        {
            //Console.Write("Enter UserName : ");
            Console.SetCursorPosition(Console.WindowWidth / 2 - 9, Console.WindowHeight / 2 - 2);
            string? username = Console.ReadLine();

            //Console.Write("Enter Password : ");
            Console.SetCursorPosition(Console.WindowWidth / 2 - 9, Console.WindowHeight / 2 + 2);
            string? password = Console.ReadLine();
            Result result = authService.AddUser(username ?? string.Empty, password ?? string.Empty);
            if (result.IsSuccess)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 - 9, Console.WindowHeight / 2 + 8);
                Console.Write("Register successful");
                return;
            }
            else
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 - 9, Console.WindowHeight / 2 + 8);
                Console.Write(result.ErrorMessage);
            }
        }

        public bool LoginUser()
        {
            Console.SetCursorPosition(Console.WindowWidth / 2 - 9, Console.WindowHeight / 2 - 2);
            string? username = Console.ReadLine();
            Console.SetCursorPosition(Console.WindowWidth / 2 - 9, Console.WindowHeight / 2 + 2);
            string? password = Console.ReadLine();
            GuidResult result = authService.LoginUser(username ?? string.Empty, password ?? string.Empty);
            if (result.IsSuccess)
            {
                //Console.WriteLine("Login successful");
                Console.Clear();
                taskMenu.DisplayTaskMenu(result.Id);
                return true;
            }
            else
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 - 9, Console.WindowHeight / 2 + 8);
                Console.Write(result.ErrorMessage);
                return false;
            }
        }


        public void DrawUsernameBox()
        {
            Console.SetCursorPosition(Console.WindowWidth / 2 - 20, Console.WindowHeight / 2 - 2);
            Console.Write("Username : ");
            Console.SetCursorPosition(Console.WindowWidth / 2 - 10, Console.WindowHeight / 2 - 3);
            DrawFieldBox(30);
            Console.SetCursorPosition(0, 0);
        }

        public void DrawPasswordBox()
        {
            Console.SetCursorPosition(Console.WindowWidth / 2 - 20, Console.WindowHeight / 2 + 2);
            Console.Write("Password : ");
            Console.SetCursorPosition(Console.WindowWidth / 2 - 10, Console.WindowHeight / 2 + 1);
            DrawFieldBox(30);
            Console.SetCursorPosition(0, 0);
        }

        public void DrawFieldBox(int length)
        {
            string line = new string('\u2500', length);
            string blankline = new string(' ', length);
            var curPosition = Console.GetCursorPosition();
            Console.Write($"\u250C{line}\u2510");
            Console.SetCursorPosition(curPosition.Item1, curPosition.Item2 + 1);
            Console.Write($"\u2502{blankline}\u2502");
            Console.SetCursorPosition(curPosition.Item1, curPosition.Item2 + 2);
            Console.Write($"\u2514{line}\u2518");
        }

        public void DrawBoundary()
        {
            string horline = new string('\u2500', Console.WindowWidth - 3);
            Console.SetCursorPosition(1, 1);
            Console.Write($"\u250C{horline}\u2510");
            for (int i = 2; i < Console.WindowHeight - 1; i++)
            {
                Console.SetCursorPosition(1, i);
                Console.Write("\u2502");
                Console.SetCursorPosition(Console.WindowWidth - 1, i);
                Console.Write("\u2502");
            }
            Console.SetCursorPosition(1, Console.WindowHeight - 1);
            Console.Write($"\u2514{horline}\u2518");
        }
    }
}