using System;
using System.Collections.Generic;
using System.Text;

namespace WinterWorkShop.Cinema.Data.Entities
{
    public class Reservation
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProjectionId { get; set; }
        public User User { get; set; }
        public Projection Projection { get; set; }
        public virtual ICollection<ReservationSeat> ReservationSeats { get; set; }
    }
}
