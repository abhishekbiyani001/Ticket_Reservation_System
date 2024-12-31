using System;
using AirlineReservationSystem.Services;

namespace AirlineReservationSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            bool exit = false;
            NotificationService notificationService = new NotificationService();
            AdminService adminService = new AdminService(notificationService);
            UserService userService = new UserService(notificationService);

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
                        adminService.AdminLogin();
                        break;

                    case "2":
                        userService.UserLogin();
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
    }
}