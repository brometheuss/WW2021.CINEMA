using System;
using System.Collections.Generic;
using System.Text;

namespace WinterWorkShop.Cinema.Domain.Queries
{
    public class FilterProjectionsQuery
    {
        public int CinemaId { get; set; }
        public int AuditoriumId { get; set; }
        public Guid MovieId { get; set; }
        public DateTime? DateTime { get; set; }
    }
}
