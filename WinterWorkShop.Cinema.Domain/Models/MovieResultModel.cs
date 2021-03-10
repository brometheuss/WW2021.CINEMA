﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WinterWorkShop.Cinema.Domain.Models
{
    public class MovieResultModel
    {
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public MovieDomainModel Movie { get; set; }
    }
}
