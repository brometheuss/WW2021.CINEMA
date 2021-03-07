using System;
using System.Collections.Generic;
using System.Text;

namespace WinterWorkShop.Cinema.Domain.Queries
{
    public class MovieQuery
    {
        public string ActorName { get; set; }
        public string Title { get; set; }
        public int YearBiggerThan { get; set; }
        public int YearLowerThan { get; set; }
        public double RatingBiggerThan { get; set; }
        public double RatingLowerThan { get; set; }
        public bool HasOscar { get; set; }
    }
}
