using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WinterWorkShop.Cinema.Data.Entities
{
    [Table("city")]
    public class City
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Cinema> Cinemas { get; set; }
    }
}
