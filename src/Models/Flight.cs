namespace AirlineReservationSystem.Models
{
    public class Flight
    {
        public required string FlightNumber {
            get;
            set;
        }
        public required string Source {
            get;
            set;
        }
        public required string Destination {
            get;
            set;
        }
        public required DateTime Date {
            get;
            set;
        }
        public required TimeSpan Time {
            get;
            set;
        }
        public required int Stops {
            get;
            set;
        }
        public required int BookedTickets
        {
            get;
            set;
        } = 0;
        public required decimal Price {
            get;
            set;
        }
    }
}