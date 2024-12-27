using System;
using System.Linq;

namespace AirlineReservationSystem
{
    public static class AdminService
    {
        public static void Login()
        {
            Console.Clear();
            Console.Write("\nEnter Admin Username: ");
            string? username = Console.ReadLine();
            Console.Write("Enter Admin Password: ");
            string? password = Console.ReadLine();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Console.WriteLine("\nUsername or password cannot be empty. Please try again.");
                return;
            }

            var admins = FileHandler<Admin>.LoadData("data/admins.json");
            var admin = admins.FirstOrDefault(a => a.Username == username);

            if (admin == null)
            {
                Console.WriteLine("\nUsername not found. Please check your credentials.");
            }
            else if (admin.Password != password)
            {
                Console.WriteLine("\nIncorrect password. Please try again.");
            }
            else
            {
                Console.WriteLine("\nLogin successful! Welcome, Admin.");
                AdminMenu();
            }
        }

        private static void AdminMenu()
        {
            bool logout = false;

            while (!logout)
            {
                Console.Clear();
                Console.WriteLine("You have successfully logged in.");
                Console.WriteLine("\n--- Admin Menu ---");
                Console.WriteLine("1. Modify Flight Details");
                Console.WriteLine("2. Send Notifications");
                Console.WriteLine("3. Logout");

                Console.Write("\nEnter your choice (1-3): ");
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Modify Flight Details - Feature coming soon.");
                        break;

                    case "2":
                        Console.WriteLine("Send Notifications - Feature coming soon.");
                        break;

                    case "3":
                        logout = true;
                        Console.WriteLine("Logged out successfully!");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please enter 1, 2, or 3.");
                        break;
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }
    }
}
