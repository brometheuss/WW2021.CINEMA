using System;
using System.Collections.Generic;
using System.Text;

namespace WinterWorkShop.Cinema.Data
{
    public class Cinema
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Auditorium> Auditoriums { get; set; }
    }
}
