using System;
using System.Collections.Generic;
using System.Text;

namespace WinterWorkShop.Cinema.Domain.Models
{
    public class CreateAuditoriumResultModel
    {
        //public AuditoriumDomainModel Auditorium { get; set; }

        public int Id { get; set; }

        public string AuditoriumName { get; set; }

        public int CinemaId { get; set; }

        public string CinemaName { get; set; }

        public bool IsSuccessful { get; set; }

        public string ErrorMessage { get; set; }

        public List<SeatDomainModel> SeatsList { get; set; }
    }
}
