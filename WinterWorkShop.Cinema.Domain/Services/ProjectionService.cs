using System;
using System.Collections.Generic;
using System.Text;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Domain.Services
{
    public class ProjectionService : IProjectionService
    {
        private readonly IProjectionsRepository _projectionsRepository;
        
        public ProjectionService(IProjectionsRepository projectionsRepository)
        {
            _projectionsRepository = projectionsRepository;
        }

        public async System.Threading.Tasks.Task<IEnumerable<ProjectionDomainModel>> GetAllAsync()
        {
            var data = await _projectionsRepository.GetAll();

            List<ProjectionDomainModel> result = new List<ProjectionDomainModel>();
            ProjectionDomainModel model;
            foreach (var item in data)
            {
                model = new ProjectionDomainModel
                {
                    Id = item.Id,
                    MovieId = item.MovieId,
                    SalaId = item.SalaId,
                    ProjectionTime = item.DateTime
                };
                result.Add(model);
            }

            return result;
        }
    }
}
