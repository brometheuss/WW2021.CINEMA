using System;
using System.Collections.Generic;
using System.Text;

namespace WinterWorkShop.Cinema.Data.Entities
{
    public class ReservationSeat
    {
        public int ReservationId { get; set; }
        public int SeatId { get; set; }
        public Reservation  Reservation { get; set; }
        public Seat Seat { get; set; }
    }
}
