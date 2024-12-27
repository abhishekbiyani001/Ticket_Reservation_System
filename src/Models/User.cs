using System;
using System.Collections.Generic;
using AirlineReservationSystem.Models;

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
    public List<Flight> BookedFlights { get; set; } = new();
}