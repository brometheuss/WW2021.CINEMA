using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUsersRepository _usersRepository;

        public UserService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<IEnumerable<UserDomainModel>> GetAllAsync()
        {
            var data = await _usersRepository.GetAll();

            if (data == null)
            {
                return null;
            }

            List<UserDomainModel> result = new List<UserDomainModel>();
            UserDomainModel model;
            foreach (var item in data)
            {
                model = new UserDomainModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Surname = item.Surname,
                    password = item.password,
                    IsAdmin = item.IsAdmin,
                };
                result.Add(model);
            }

            return result;
        }
    }
}
