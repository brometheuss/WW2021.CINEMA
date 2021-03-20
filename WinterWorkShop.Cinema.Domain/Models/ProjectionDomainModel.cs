﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WinterWorkShop.Cinema.Domain.Models
{
    public class ProjectionDomainModel
    {
        public Guid Id { get; set; }

        public Guid MovieId { get; set; }

        public string MovieTitle { get; set; }

        public int AuditoriumId { get; set; }

        public string AuditoriumName { get; set; }

        public DateTime ProjectionTime { get; set; }
    }
}
