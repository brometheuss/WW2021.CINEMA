using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Common;

namespace WinterWorkShop.Cinema.API.Models
{
    public class MovieModel
    {
        [Required]
        [StringLength(50, ErrorMessage = Messages.MOVIE_PROPERTY_TITLE_NOT_VALID)]
        public string Title { get; set; }
        
        [Required]
        [Range(1895, 2100, ErrorMessage = Messages.MOVIE_PROPERTY_YEAR_NOT_VALID)]
        public int Year{ get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = Messages.MOVIE_PROPERTY_RATING_NOT_VALID)]
        public double Rating { get; set; }

        [Required]
        public bool Current { get; set; }

        [Required(ErrorMessage = Messages.MOVIE_CREATION_ERROR_HASOSCAR_REQUIRED)]
        public bool HasOscar { get; set; }
    }
}
