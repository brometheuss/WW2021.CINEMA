using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.Domain.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDomainModel>> GetAllAsync();
        Task<RoleDomainModel> GetByIdAsync(int id);
    }
}
