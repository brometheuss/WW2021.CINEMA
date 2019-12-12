using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WinterWorkShop.Cinema.API.Models
{
    public class MovieModel
    {
        [Required]
        public string Title { get; set; }
        
        [Required]
        [Range(0, 3000)]
        public int Year{ get; set; }

        [Required]
        [Range(1, 10)]
        public double Rating { get; set; }

        public bool Current { get; set; }
    }
}
