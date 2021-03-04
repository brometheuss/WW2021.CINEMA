﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WinterWorkShop.Cinema.Data.Entities;

namespace WinterWorkShop.Cinema.Data
{
    [Table("seat")]
    public class Seat
    {
        public Guid Id { get; set; }

        [Column("auditorium_id")]
        public int AuditoriumId { get; set; }

        public int Row { get; set; }

        public int Number { get; set; }

        public virtual Auditorium Auditorium { get; set; }
        public virtual ICollection<ReservationSeat> ReservationSeats { get; set; }
    }
}
