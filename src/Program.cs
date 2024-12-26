using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace AirlineReservationSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Welcome to the Airline Ticket Reservation System!");
                Console.WriteLine("1. Admin Login");
                Console.WriteLine("2. User Login");
                Console.WriteLine("3. Exit");

                Console.Write("\nEnter your choice (1-3): ");
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AdminLogin();
                        break;

                    case "2":
                        UserLogin();
                        break;

                    case "3":
                        Console.WriteLine("Thank you for using the Airline Ticket Reservation System. Goodbye!");
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please enter 1, 2, or 3.");
                        break;
                }
            }
        }

        private static void AdminLogin()
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

            if (AuthenticateAdmin(username, password))
            {
                Console.WriteLine("\nLogin successful! Welcome, Admin.");
                AdminMenu();
            }
            else
            {
                Console.WriteLine("\nInvalid username or password. Please try again.");
            }
        }

        private static void UserLogin()
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

            if (AuthenticateUser(username, password))
            {
                Console.WriteLine("\nLogin successful! Welcome, User.");
                UserMenu();
            }
            else
            {
                Console.WriteLine("\nInvalid username or password. Please try again.");
            }
        }

        private static bool AuthenticateAdmin(string username, string password)
        {
            var admins = LoadDataFromFile<Admin>("data/admins.json");
            foreach (var admin in admins)
            {
                if (admin.Username == username && admin.Password == password)
                    return true;
            }
            return false;
        }

        private static bool AuthenticateUser(string username, string password)
        {
            var users = LoadDataFromFile<User>("data/users.json");
            foreach (var user in users)
            {
                if (user.Username == username && user.Password == password)
                    return true;
            }
            return false;
        }

        private static void AdminMenu()
        {
            bool logout = false;

            while (!logout)
            {
                Console.Clear();
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

        private static void UserMenu()
        {
            bool logout = false;

            while (!logout)
            {
                Console.Clear();
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

        private static List<T> LoadDataFromFile<T>(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    string jsonData = File.ReadAllText(filePath);
                    return JsonConvert.DeserializeObject<List<T>>(jsonData) ?? new List<T>();
                }
                else
                {
                    Console.WriteLine($"Error: File '{filePath}' not found.");
                    return new List<T>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data from file: {ex.Message}");
                return new List<T>();
            }
        }
    }

    public class Admin
    {
        public required string Username {
            get;
            set;
        }
        public required string Password {
            get;
            set;
        }
    }

    public class User
    {
        public required string Username {
            get;
            set;
        }
        public required string Password {
            get;
            set;
        }
    }
}