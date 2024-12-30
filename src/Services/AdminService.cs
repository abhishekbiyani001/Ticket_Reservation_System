using System;
using System.Collections.Generic;
using System.Linq;
using AirlineReservationSystem.Models;
using AirlineReservationSystem.Services;
using AirlineReservationSystem.FileHandler;

namespace AirlineReservationSystem.Services
{
    public class AdminService
    {
        private readonly FlightService _flightService = new();

        public void AdminLogin()
        {
            Console.Clear();
            Console.WriteLine("\n--- Admin Login ---");
            Console.Write("Enter Admin Username: ");
            string username = Console.ReadLine()!;
            Console.Write("Enter Admin Password: ");
            string password = Console.ReadLine()!;

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

        private bool AuthenticateAdmin(string username, string password)
        {
            var admins = FileHandler<Admin>.LoadData("data/admins.json");
            var admin = admins.FirstOrDefault(a => a.Username == username);

            if (admin == null || admin.Password != password)
            {
                return false;
            }

            return true;
        }

        public void AdminMenu()
        {
            bool logout = false;

            while (!logout)
            {
                Console.Clear();
                Console.WriteLine("You have successfully logged in.");
                Console.WriteLine("\n--- Admin Menu ---");
                Console.WriteLine("1. Add New Flight");
                Console.WriteLine("2. Modify Flight Details");
                Console.WriteLine("3. Delete Flight");
                Console.WriteLine("4. View All Flights");
                Console.WriteLine("5. Add New Admin");
                Console.WriteLine("6. Add New User");
                Console.WriteLine("7. Logout");

                Console.Write("\nEnter your choice (1-7): ");
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddFlight();
                        break;
                    case "2":
                        ModifyFlight();
                        break;
                    case "3":
                        DeleteFlight();
                        break;
                    case "4":
                        ViewAllFlights();
                        break;
                    case "5":
                        AddNewAdmin();
                        break;
                    case "6":
                        AddNewUser();
                        break;
                    case "7":
                        logout = true;
                        Console.WriteLine("Logged out successfully!");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please enter 1-7.");
                        break;
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        private void AddNewUser()
        {
            Console.Clear();
            Console.WriteLine("\n--- Add New User ---");

            Console.Write("Enter New User Username: ");
            string username = Console.ReadLine()!;
            Console.Write("Enter New User Password: ");
            string password = Console.ReadLine()!;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("\nUsername and password cannot be empty.");
                return;
            }

            var users = FileHandler<User>.LoadData("data/users.json");

            if (users.Any(u => u.Username == username))
            {
                Console.WriteLine("\nUser with this username already exists.");
                return;
            }

            var newUser = new User
            {
                Username = username,
                Password = password,
                BookedFlights = new List<Flight>()
            };

            users.Add(newUser);
            FileHandler<User>.SaveData("data/users.json", users);

            Console.WriteLine("\nNew user added successfully!");
        }

        private void AddFlight()
        {
            Console.Clear();
            Console.WriteLine("\n--- Add New Flight ---");

            string flightNumber;
            while (true)
            {
                Console.Write("Flight Number: ");
                flightNumber = Console.ReadLine()!;
                if (!string.IsNullOrWhiteSpace(flightNumber) && int.TryParse(flightNumber, out int flightNum) && flightNum > 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Flight Number must be a positive integer.");
                }
            }

            var existingFlight = _flightService.GetAllFlights().FirstOrDefault(f => f.FlightNumber == flightNumber);
            if (existingFlight != null)
            {
                Console.WriteLine("A flight with this flight number already exists.");
                return;
            }

            string source;
            while (true)
            {
                Console.Write("Source: ");
                source = Console.ReadLine()!;
                if (!string.IsNullOrWhiteSpace(source) && !int.TryParse(source, out _))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Source must be a valid string.");
                }
            }

            string destination;
            while (true)
            {
                Console.Write("Destination: ");
                destination = Console.ReadLine()!;
                if (!string.IsNullOrWhiteSpace(destination) && !int.TryParse(destination, out _))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Destination must be a valid string.");
                }
            }

            DateTime date;
            while (true)
            {
                Console.Write("Date (dd-MM-yyyy): ");
                if (DateTime.TryParseExact(Console.ReadLine(), "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out date))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Date not entered in proper format.");
                }
            }

            TimeSpan time;
            while (true)
            {
                Console.Write("Time (HH:mm): ");
                if (TimeSpan.TryParseExact(Console.ReadLine(), "hh\\:mm", null, out time))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Time not entered in proper format.");
                }
            }

            Console.Write("Number of Stops: ");
            int stops = int.Parse(Console.ReadLine()!);
            if (stops < 0)
            {
                stops = 0;
            }

            decimal price;
            while (true)
            {
                Console.Write("Price: ");
                string priceInput = Console.ReadLine()!;
                if (!string.IsNullOrWhiteSpace(priceInput) && decimal.TryParse(priceInput, out price) && price >= 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Price must be a non-negative decimal.");
                }
            }

            var flight = new Flight
            {
                FlightNumber = flightNumber,
                Source = source,
                Destination = destination,
                Date = date,
                Time = time,
                Stops = stops,
                Price = price,
                BookedTickets = 0
            };

            _flightService.AddFlight(flight);
            Console.WriteLine("\nFlight added successfully!");
        }

        private void ModifyFlight()
        {
            Console.Clear();
            Console.WriteLine("\n--- Modify Flight ---");
            Console.Write("Enter Flight Number to Modify: ");
            string flightNumber = Console.ReadLine()!;

            //var flights = _flightService.GetAllFlights();
            //var flight = flights.Find(f => f.FlightNumber == flightNumber);
            var flight = _flightService.GetAllFlights().FirstOrDefault(f => f.FlightNumber == flightNumber);

            if (flight == null)
            {
                Console.WriteLine("Flight not found!");
                return;
            }

            Console.Write("New Source (Leave blank to keep current): ");
            string source = Console.ReadLine()!;
            if (!string.IsNullOrWhiteSpace(source) && int.TryParse(source, out _))
            {
                Console.WriteLine("Source must be a valid string.");
                return;
            }

            Console.Write("New Destination (Leave blank to keep current): ");
            string destination = Console.ReadLine()!;
            if (!string.IsNullOrWhiteSpace(destination) && int.TryParse(destination, out _))
            {
                Console.WriteLine("Destination must be a valid string.");
                return;
            }

            Console.Write("New Date (dd-MM-yyyy, Leave blank to keep current): ");
            string dateInput = Console.ReadLine()!;
            if (!string.IsNullOrWhiteSpace(dateInput) && !DateTime.TryParseExact(dateInput, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out _))
            {
                Console.WriteLine("Date not entered in proper format.");
                return;
            }

            Console.Write("New Time (HH:mm, Leave blank to keep current): ");
            string timeInput = Console.ReadLine()!;
            if (!string.IsNullOrWhiteSpace(timeInput) && !TimeSpan.TryParseExact(timeInput, "hh\\:mm", null, out _))
            {
                Console.WriteLine("Time not entered in proper format.");
                return;
            }

            Console.Write("New Number of Stops (Leave blank to keep current): ");
            string stopsInput = Console.ReadLine()!;
            if (!string.IsNullOrWhiteSpace(stopsInput) && (!int.TryParse(stopsInput, out int stops) || stops < 0))
            {
                Console.WriteLine("Number of Stops must be a non-negative integer.");
                return;
            }

            Console.Write("New Price (Leave blank to keep current): ");
            string priceInput = Console.ReadLine()!;
            if (!string.IsNullOrWhiteSpace(priceInput) && (!decimal.TryParse(priceInput, out decimal price) || price < 0))
            {
                Console.WriteLine("Price must be a non-negative decimal.");
                return;
            }

            flight.Source = string.IsNullOrEmpty(source) ? flight.Source : source;
            flight.Destination = string.IsNullOrEmpty(destination) ? flight.Destination : destination;
            flight.Date = string.IsNullOrEmpty(dateInput) ? flight.Date : DateTime.ParseExact(dateInput, "dd-MM-yyyy", null);
            flight.Time = string.IsNullOrEmpty(timeInput) ? flight.Time : TimeSpan.ParseExact(timeInput, "hh\\:mm", null);
            flight.Stops = string.IsNullOrEmpty(stopsInput) ? flight.Stops : int.Parse(stopsInput);
            flight.Price = string.IsNullOrEmpty(priceInput) ? flight.Price : decimal.Parse(priceInput);

            _flightService.UpdateFlight(flightNumber, flight);
            Console.WriteLine("\nFlight modified successfully!");
        }

        private void AddNewAdmin()
        {
            Console.Clear();
            Console.WriteLine("\n--- Add New Admin ---");

            Console.Write("Enter New Admin Username: ");
            string username = Console.ReadLine()!;

            Console.Write("Enter New Admin Password: ");
            string password = Console.ReadLine()!;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("\nUsername and password cannot be empty.");
                return;
            }

            var admins = FileHandler<Admin>.LoadData("data/admins.json");
            if (admins.Any(a => a.Username == username))
            {
                Console.WriteLine("\nAdmin with this username already exists.");
                return;
            }

            var newAdmin = new Admin
            {
                Username = username,
                Password = password
            };

            admins.Add(newAdmin);
            FileHandler<Admin>.SaveData("data/admins.json", admins);

            Console.WriteLine("\nNew admin added successfully!");
        }

        private void DeleteFlight()
        {
            Console.Clear();
            Console.WriteLine("\n--- Delete Flight ---");
            Console.Write("Enter Flight Number to Delete: ");
            string flightNumber = Console.ReadLine()!;
            _flightService.DeleteFlight(flightNumber);
            Console.WriteLine("\nFlight deleted successfully!");
        }

        private void ViewAllFlights()
        {
            Console.Clear();
            Console.WriteLine("\n--- All Flights ---");
            var flights = _flightService.GetAllFlights();

            if (flights.Count == 0)
            {
                Console.WriteLine("No flights available.");
                return;
            }

            foreach (var flight in flights)
            {
                Console.WriteLine($"Flight Number: {flight.FlightNumber}");
                Console.WriteLine($"Source: {flight.Source}");
                Console.WriteLine($"Destination: {flight.Destination}");
                Console.WriteLine($"Date: {flight.Date:dd-MM-yyyy}");
                Console.WriteLine($"Time: {flight.Time:hh\\:mm}");
                Console.WriteLine($"Stops: {flight.Stops}");
                Console.WriteLine($"Price: {flight.Price:C}");
                Console.WriteLine($"Tickets Booked: {flight.BookedTickets} / 500");
                Console.WriteLine("----------------------------");
            }
        }
    }
}