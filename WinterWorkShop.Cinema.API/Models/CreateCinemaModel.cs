using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Common;

namespace WinterWorkShop.Cinema.API.Models
{
    public class CreateCinemaModel
    {
        [Required]
        public int CityId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = Messages.CINEMA_NAME_NOT_VALID)]
        public string Name { get; set; }

        [Required]
        [Range(1, 20, ErrorMessage = Messages.AUDITORIUM_PROPERTY_SEATROWSNUMBER_NOT_VALID)]
        public int SeatRows { get; set; }

        [Required]
        [Range(1, 20, ErrorMessage = Messages.AUDITORIUM_PROPERTY_SEATNUMBER_NOT_VALID)]
        public int NumberOfSeats { get; set; }
        /*[Required]
        public string AuditoriumName*/


    }
}
