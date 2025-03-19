using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace TicketToCode.Core.Models
{
    public class Booking
    {
        public int Id { get; set; }

        //  Koppling till eventet
        public int EventId { get; set; }
        public Event Event { get; set; }

        //  Koppling till användaren
        public int UserId { get; set; }
        public User User { get; set; }

        //  Antal biljetter
        public int TicketCount { get; set; }

        //  Tidpunkt för bokning
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;
    }
}

