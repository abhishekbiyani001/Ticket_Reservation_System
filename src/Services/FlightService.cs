using System;
using System.Collections.Generic;
using AirlineReservationSystem.Models;
using AirlineReservationSystem.FileHandler;

namespace AirlineReservationSystem.Services
{
    public class FlightService
    {
        private const string FlightDataPath = "data/flights.json";

        public List<Flight> GetAllFlights()
        {
            return FileHandler<Flight>.LoadData(FlightDataPath);
        }

        public void AddFlight(Flight flight)
        {
            var flights = GetAllFlights();
            flights.Add(flight);
            FileHandler<Flight>.SaveData(FlightDataPath, flights);
        }

        public void UpdateFlight(string flightNumber, Flight updatedFlight)
        {
            var flights = GetAllFlights();
            var index = flights.FindIndex(f => f.FlightNumber == flightNumber);

            if (index != -1)
            {
                flights[index] = updatedFlight;
                FileHandler<Flight>.SaveData(FlightDataPath, flights);
            }
            else
            {
                Console.WriteLine("Flight not found.");
            }
        }

        public void DeleteFlight(string flightNumber)
        {
            var flights = GetAllFlights();
            flights.RemoveAll(f => f.FlightNumber == flightNumber);
            FileHandler<Flight>.SaveData(FlightDataPath, flights);
        }
    }
}