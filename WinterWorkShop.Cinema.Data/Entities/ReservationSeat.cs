using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WinterWorkShop.Cinema.Data.Entities
{
    [Table("reservationSeat")]
    public class ReservationSeat
    {
        public Guid ReservationId { get; set; }
        public Guid SeatId { get; set; }
        public Reservation  Reservation { get; set; }
        public Seat Seat { get; set; }
    }
}
