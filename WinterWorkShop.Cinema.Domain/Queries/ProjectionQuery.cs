using System;
using System.Collections.Generic;
using System.Text;

namespace WinterWorkShop.Cinema.Domain.Queries
{
    public class ProjectionQuery
    {
        public int AuditoriumId { get; set; }
        public int CinemaId { get; set; }
        public int MovieId { get; set; }
        public DateTime? DateLaterThan { get; set; }
        public DateTime? DateEarlierThan { get; set; }
    }
}
