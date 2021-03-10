﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.Domain.Interfaces
{
    public interface IActorService
    {
        Task<IEnumerable<ActorDomainModel>> GetAllAsync();
        Task<ActorDomainModel> GetByIdAsync(Guid id);
    }
}
