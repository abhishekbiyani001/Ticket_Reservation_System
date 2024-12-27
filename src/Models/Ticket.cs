namespace AirlineReservationSystem.Models
{
    public class Ticket
    {
        public required int TicketId {
            get;
            set;
        }
        public required string UserId {
            get;
            set;
        }
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
        public required string Date {
            get;
            set;
        }
        public required string Time {
            get;
            set;
        }
        public required decimal Price {
            get;
            set;
        }
        public required string BookingStatus {
            get;
            set;
        }
        public required string BookingDate {
            get;
            set;
        }
    }
}