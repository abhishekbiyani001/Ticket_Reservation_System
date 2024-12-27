using System;
using System.Linq;
using AirlineReservationSystem.FileHandler;

namespace AirlineReservationSystem
{
    public static class UserService
    {
        public static void Login()
        {
            Console.Clear();
            Console.Write("\nEnter User Username: ");
            string? username = Console.ReadLine();
            Console.Write("Enter User Password: ");
            string? password = Console.ReadLine();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Console.WriteLine("\nUsername or password cannot be empty. Please try again.");
                return;
            }

            var users = FileHandler<User>.LoadData("data/users.json");
            var user = users.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                Console.WriteLine("\nUsername not found. Please check your credentials.");
            }
            else if (user.Password != password)
            {
                Console.WriteLine("\nIncorrect password. Please try again.");
            }
            else
            {
                Console.WriteLine("\nLogin successful! Welcome, User.");
                UserMenu();
            }
        }

        private static void UserMenu()
        {
            bool logout = false;

            while (!logout)
            {
                Console.Clear();
                Console.WriteLine("You have successfully logged in.");
                Console.WriteLine("\n--- User Menu ---");
                Console.WriteLine("1. View Tickets");
                Console.WriteLine("2. Book Tickets");
                Console.WriteLine("3. Cancel Tickets");
                Console.WriteLine("4. Logout");

                Console.Write("\nEnter your choice (1-4): ");
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("View Tickets - Feature coming soon.");
                        break;

                    case "2":
                        Console.WriteLine("Book Tickets - Feature coming soon.");
                        break;

                    case "3":
                        Console.WriteLine("Cancel Tickets - Feature coming soon.");
                        break;

                    case "4":
                        logout = true;
                        Console.WriteLine("Logged out successfully!");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please enter 1, 2, 3, or 4.");
                        break;
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }
    }
}