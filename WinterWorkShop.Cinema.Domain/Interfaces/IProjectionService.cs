using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Domain.Queries;

namespace WinterWorkShop.Cinema.Domain.Interfaces
{
    public interface IProjectionService
    {
        //Task<IEnumerable<ProjectionDomainModel>> GetAllAsync();
        Task<CreateProjectionResultModel> CreateProjection(ProjectionDomainModel domainModel);
        Task<IEnumerable<ProjectionDomainModel>> GetAllAsync(ProjectionQuery query);
        Task<ProjectionDomainModel> GetProjectionByIdAsync(Guid id);
        Task<List<FilterProjectionDomainModel>> GetProjectionsWithMovieAndAuditorium(FilterProjectionsQuery query);

    }
}
