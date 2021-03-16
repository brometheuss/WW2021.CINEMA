using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;
using System.Linq;

namespace WinterWorkShop.Cinema.Domain.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRolesRepository _rolesRepository;

        public RoleService(IRolesRepository rolesRepository)
        {
            _rolesRepository = rolesRepository;
        }

        public async Task<IEnumerable<RoleDomainModel>> GetAllAsync()
        {
            var roles = await _rolesRepository.GetAll();
            
            if(roles.Count() == 0)
            {
                return null;
            }

            List<RoleDomainModel> lista = roles.Select(role => new RoleDomainModel
            {
                Id = role.Id,
                Name = role.Name,
                Users = role.Users.Select(user => new UserDomainModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsAdmin = user.IsAdmin,
                    Points = user.Points,
                    RoleId = user.RoleId,
                    UserName = user.UserName
                })
                .ToList()
            })
                .ToList();

            return lista; 
        }

        public async Task<RoleDomainModel> GetByIdAsync(int id)
        {
            var role = await _rolesRepository.GetByIdAsync(id);

            if(role == null)
            {
                return null;
            }

            RoleDomainModel roleModel = new RoleDomainModel
            {
                Id = role.Id,
                Name = role.Name,
                Users = role.Users.Select(user => new UserDomainModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    IsAdmin = user.IsAdmin,
                    LastName = user.LastName,
                    Points = user.Points,
                    RoleId = user.RoleId,
                    UserName = user.UserName
                }).ToList() 
            };

            return roleModel;
        }
    }
}

