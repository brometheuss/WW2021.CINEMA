using System;
using System.Collections.Generic;
using System.Text;

namespace WinterWorkShop.Cinema.Domain.Models
{
    public class FilterProjectionDomainModel
    {
        public Guid Id { get; set; }
        public Guid MovieId { get; set; }
        public DateTime ProjectionTime { get; set; }
        public string AuditoriumName { get; set; }
        public string MovieTitle { get; set; }
        public double MovieRating { get; set; }
        public int MovieYear { get; set; }

    }
}
