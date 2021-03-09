using System;
using System.Collections.Generic;
using System.Text;

namespace WinterWorkShop.Cinema.Domain.Models
{
    public class CityDomainModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CinemaDomainModel> CinemasList { get; set; } 
    }
}
