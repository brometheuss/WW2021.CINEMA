using System;
using System.Collections.Generic;
using System.Text;

namespace WinterWorkShop.Cinema.Domain.Models
{
    public class MovieProjectionDomainModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public double Rating { get; set; }
        public int Year { get; set; }
        public List<ProjectionDomainModel> Projections { get; set; }

    }
}
