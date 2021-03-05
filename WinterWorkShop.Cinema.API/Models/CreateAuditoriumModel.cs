﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Common;

namespace WinterWorkShop.Cinema.API.Models
{
    public class CreateAuditoriumModel
    {
        [Required]        
        public int cinemaId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = Messages.AUDITORIUM_PROPERTY_NAME_NOT_VALID)]
        public string auditName { get; set; }

        [Required]
        [Range(1,20, ErrorMessage = Messages.AUDITORIUM_PROPERTY_SEATROWSNUMBER_NOT_VALID)]
        public int seatRows { get; set; }

        [Required]
        [Range(1, 20, ErrorMessage = Messages.AUDITORIUM_PROPERTY_SEATNUMBER_NOT_VALID)]
        public int numberOfSeats { get; set; }

    }
}
