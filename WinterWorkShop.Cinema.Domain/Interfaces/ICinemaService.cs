﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.Domain.Interfaces
{
    public interface ICinemaService
    {
        Task<IEnumerable<CinemaDomainModel>> GetAllAsync();
        Task<CinemaDomainModel> CreateCinemaAsync(CinemaDomainModel cinemaDomainModel, int numOfSeats, int numbOfRows);
        Task<CinemaDomainModel> DeleteCinemaAsync(int id);
    }
}
