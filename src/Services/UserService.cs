using System;
using System.Collections.Generic;
using System.Linq;
using AirlineReservationSystem.Models;
using AirlineReservationSystem.Services;
using AirlineReservationSystem.FileHandler;

namespace AirlineReservationSystem.Services
{
    public class UserService
    {
        private readonly FlightService _flightService = new();
        private User? _currentUser;

        public void UserLogin()
        {
            Console.Clear();
            Console.WriteLine("\n--- User Login ---");
            Console.Write("Enter User Username: ");
            string username = Console.ReadLine()!;
            Console.Write("Enter User Password: ");
            string password = Console.ReadLine()!;

            if (AuthenticateUser(username, password))
            {
                Console.WriteLine("\nLogin successful! Welcome, User.");
                CheckAndUpdateBookings();
                UserMenu();
            }
            else
            {
                Console.WriteLine("\nInvalid username or password. Please try again.");
            }
        }

        private bool AuthenticateUser(string username, string password)
        {
            var users = FileHandler<User>.LoadData("data/users.json");
            var user = users.FirstOrDefault(u => u.Username == username);

            if (user == null || user.Password != password)
            {
                return false;
            }

            _currentUser = user;
            return true;
        }

        private void CheckAndUpdateBookings()
        {
            var flights = _flightService.GetAllFlights();
            var updatedBookings = new List<Flight>();

            foreach (var bookedFlight in _currentUser!.BookedFlights)
            {
                var flight = flights.FirstOrDefault(f => f.FlightNumber == bookedFlight.FlightNumber);
                if (flight != null && flight.Source == bookedFlight.Source && flight.Destination == bookedFlight.Destination)
                {
                    updatedBookings.Add(bookedFlight);
                }
            }

            if (updatedBookings.Count != _currentUser.BookedFlights.Count)
            {
                _currentUser.BookedFlights = updatedBookings;
                FileHandler<User>.SaveData("data/users.json", new List<User> { _currentUser });
                Console.WriteLine("\nSome of your bookings have been removed due to flight details modification.");
            }
        }

        public void UserMenu()
        {
            bool logout = false;

            while (!logout)
            {
                Console.Clear();
                Console.WriteLine("You have successfully logged in.");
                Console.WriteLine("\n--- User Menu ---");
                Console.WriteLine("1. View Flights");
                Console.WriteLine("2. Book Flight");
                Console.WriteLine("3. Cancel Booking");
                Console.WriteLine("4. View My Bookings");
                Console.WriteLine("5. Logout");

                Console.Write("\nEnter your choice (1-5): ");
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewFlights();
                        break;
                    case "2":
                        BookFlight();
                        break;
                    case "3":
                        CancelBooking();
                        break;
                    case "4":
                        ViewMyBookings();
                        break;
                    case "5":
                        logout = true;
                        Console.WriteLine("Logged out successfully!");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please enter 1-5.");
                        break;
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        private void ViewFlights()
        {
            Console.Clear();
            Console.WriteLine("\n--- View Flights ---");
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
                Console.WriteLine("----------------------------");
            }
        }

        private void BookFlight()
        {
            Console.Clear();
            Console.WriteLine("\n--- Book Flight ---");

            Console.Write("Enter Source: ");
            string source = Console.ReadLine()!;
            Console.Write("Enter Destination: ");
            string destination = Console.ReadLine()!;

            var flights = from flight in _flightService.GetAllFlights()
                          where flight.Source.Equals(source, StringComparison.OrdinalIgnoreCase) &&
                                flight.Destination.Equals(destination, StringComparison.OrdinalIgnoreCase)
                          select flight;

            if (!flights.Any())
            {
                Console.WriteLine("No matching flights available.");
                return;
            }

            Console.WriteLine("\nAvailable Flights:");
            foreach (var flight in flights)
            {
                Console.WriteLine($"Flight Number: {flight.FlightNumber}");
                Console.WriteLine($"Date: {flight.Date:dd-MM-yyyy}");
                Console.WriteLine($"Time: {flight.Time:hh\\:mm}");
                Console.WriteLine($"Stops: {flight.Stops}");
                Console.WriteLine($"Price: {flight.Price:C}");
                Console.WriteLine("----------------------------");
            }

            Console.Write("\nEnter Flight Number to Book: ");
            string flightNumber = Console.ReadLine()!;
            var selectedFlight = flights.FirstOrDefault(f => f.FlightNumber == flightNumber);

            if (selectedFlight == null)
            {
                Console.WriteLine("Invalid Flight Number.");
                return;
            }

            _currentUser!.BookedFlights.Add(selectedFlight);
            FileHandler<User>.SaveData("data/users.json", new List<User> { _currentUser });

            var ticket = new Ticket
            {
                TicketId = new Random().Next(1, 1000),
                UserId = _currentUser.Username,
                FlightNumber = selectedFlight.FlightNumber,
                Source = selectedFlight.Source,
                Destination = selectedFlight.Destination,
                Date = selectedFlight.Date.ToString("dd-MM-yyyy"),
                Time = selectedFlight.Time.ToString("hh\\:mm"),
                Price = selectedFlight.Price,
                BookingStatus = "Booked",
                BookingDate = DateTime.Now.ToString("dd-MM-yyyy")
            };

            var tickets = FileHandler<Ticket>.LoadData("data/tickets.json");
            tickets.Add(ticket);
            FileHandler<Ticket>.SaveData("data/tickets.json", tickets);

            Console.WriteLine("\nFlight booked successfully!");
            UpdateTicketDetails(selectedFlight);
        }

        private void UpdateTicketDetails(Flight updatedFlight)
        {
            var tickets = FileHandler<Ticket>.LoadData("data/tickets.json");
            bool isUpdated = false;

            foreach (var ticket in tickets)
            {
                if (ticket.FlightNumber == updatedFlight.FlightNumber)
                {
                    if (ticket.Source != updatedFlight.Source ||
                        ticket.Destination != updatedFlight.Destination ||
                        ticket.Date != updatedFlight.Date.ToString("dd-mm-yyyy") ||
                        ticket.Time != updatedFlight.Time.ToString("hh\\:mm") ||
                        ticket.Price != updatedFlight.Price)
                    {
                        ticket.Source = updatedFlight.Source;
                        ticket.Destination = updatedFlight.Destination;
                        ticket.Date = updatedFlight.Date.ToString("dd-mm-yyyy");
                        ticket.Time = updatedFlight.Time.ToString("hh\\:mm");
                        ticket.Price = updatedFlight.Price;
                        isUpdated = true;
                    }
                }
            }

            if (isUpdated)
            {
                FileHandler<Ticket>.SaveData("data/tickets.json", tickets);
                Console.WriteLine("\nTicket details updated successfully!");
            }
        }

        private void CancelBooking()
        {
            Console.Clear();
            Console.WriteLine("\n--- Cancel Booking ---");
            Console.Write("Enter Flight Number to Cancel Booking: ");
            string flightNumber = Console.ReadLine()!;
            var flight = _currentUser!.BookedFlights.FirstOrDefault(f => f.FlightNumber == flightNumber);

            if (flight == null)
            {
                Console.WriteLine("Flight not found in your bookings!");
                return;
            }

            _currentUser.BookedFlights.Remove(flight);
            FileHandler<User>.SaveData("data/users.json", new List<User> { _currentUser });

            var tickets = FileHandler<Ticket>.LoadData("data/tickets.json");
            var ticket = tickets.FirstOrDefault(t => t.FlightNumber == flightNumber && t.UserId == _currentUser.Username);

            if (ticket != null)
            {
                ticket.BookingStatus = "Canceled";
                FileHandler<Ticket>.SaveData("data/tickets.json", tickets);
            }

            Console.WriteLine("\nBooking canceled successfully!");
        }

        private void ViewMyBookings()
        {
            Console.Clear();
            Console.WriteLine("\n--- My Bookings ---");

            if (_currentUser!.BookedFlights.Count == 0)
            {
                Console.WriteLine("You have no bookings.");
                return;
            }

            foreach (var flight in _currentUser.BookedFlights)
            {
                Console.WriteLine($"Flight Number: {flight.FlightNumber}");
                Console.WriteLine($"Source: {flight.Source}");
                Console.WriteLine($"Destination: {flight.Destination}");
                Console.WriteLine($"Date: {flight.Date:dd-MM-yyyy}");
                Console.WriteLine($"Time: {flight.Time:hh\\:mm}");
                Console.WriteLine($"Stops: {flight.Stops}");
                Console.WriteLine($"Price: {flight.Price:C}");
                Console.WriteLine("----------------------------");
            }
        }
    }
}