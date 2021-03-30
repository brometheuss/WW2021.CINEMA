using System;
using System.Collections.Generic;
using System.Text;

namespace WinterWorkShop.Cinema.Domain.Models
{
    public class UserReservationDomainModel
    {
        public Guid ProjectionId { get; set; }
        public string ProjectionTime { get; set; }
        public string AuditoriumName { get; set; }
        public string MovieTitle { get; set; }

    }
}
