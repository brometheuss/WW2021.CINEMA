using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WinterWorkShop.Cinema.Data.Entities
{
    [Table("reservation")]
    public class Reservation
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public int ProjectionId { get; set; }
        public User User { get; set; }
        public Projection Projection { get; set; }
        public virtual ICollection<ReservationSeat> ReservationSeats { get; set; }
    }
}
