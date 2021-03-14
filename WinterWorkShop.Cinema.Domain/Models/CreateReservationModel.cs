using System;
using System.Collections.Generic;
using System.Text;

namespace WinterWorkShop.Cinema.Domain.Models
{
    public class CreateReservationModel
    {
        //public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid ProjectionId { get; set; }

        public IEnumerable<SeatDomainModel> SeatsRequested { get; set; }
    }
}
