using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.Domain.Interfaces
{
    public interface ICityService
    {
        Task<IEnumerable<CityDomainModel>> GetAllAsync();
        Task<CityDomainModel> GetByIdAsync(int id);
        Task<CityDomainModel> GetByCityNameAsync(string cityName);
    }
}
