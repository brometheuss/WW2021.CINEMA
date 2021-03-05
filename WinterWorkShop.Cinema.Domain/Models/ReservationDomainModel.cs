using System;
using System.Collections.Generic;
using System.Text;

namespace WinterWorkShop.Cinema.Domain.Models
{
    public class ReservationDomainModel
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public int ProjectionId { get; set; }
    }
}
