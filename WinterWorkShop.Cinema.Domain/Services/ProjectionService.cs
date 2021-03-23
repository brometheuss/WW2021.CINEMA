using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Domain.Queries;
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

        public async Task<CreateProjectionResultModel> CreateProjection(ProjectionDomainModel domainModel)
        {
            int projectionTime = 3;

            var projectionsAtSameTime = _projectionsRepository.GetByAuditoriumId(domainModel.AuditoriumId)
                .Where(x => x.DateTime < domainModel.ProjectionTime.AddHours(projectionTime) && x.DateTime > domainModel.ProjectionTime.AddHours(-projectionTime))
                .ToList();

            if (projectionsAtSameTime != null && projectionsAtSameTime.Count > 0)
            {
                return new CreateProjectionResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.PROJECTIONS_AT_SAME_TIME
                };
            }

            var newProjection = new Data.Projection
            {
                MovieId = domainModel.MovieId,
                AuditoriumId = domainModel.AuditoriumId,
                DateTime = domainModel.ProjectionTime
            };

            var insertedProjection = _projectionsRepository.Insert(newProjection);

            if (insertedProjection == null)
            {
                return new CreateProjectionResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.PROJECTION_CREATION_ERROR
                };
            }

            _projectionsRepository.Save();
            var projekcija = await _projectionsRepository.GetByIdAsync(insertedProjection.Id);
            CreateProjectionResultModel result = new CreateProjectionResultModel
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Projection = new ProjectionDomainModel
                {
                    Id = insertedProjection.Id,
                    AuditoriumId = insertedProjection.AuditoriumId,
                    MovieId = insertedProjection.MovieId,
                    ProjectionTime = insertedProjection.DateTime,
                    AuditoriumName = projekcija.Auditorium.AuditName,
                    MovieTitle = projekcija.Movie.Title
                }
            };

            return result;
        }

        //Projection filter by: Auditorium, Cinema, Movie, TimeSpan
        public async Task<IEnumerable<ProjectionDomainModel>> GetAllAsync(ProjectionQuery query)
        {
            var data = await _projectionsRepository.GetAll();

            if (query.AuditoriumId > 0)
                data = data.Where(x => x.AuditoriumId == query.AuditoriumId);

            if (query.CinemaId > 0)
                data = data.Where(x => x.Auditorium.Cinema.Id == query.CinemaId);

            if (query.MovieId != Guid.Empty)
                data = data.Where(x => x.MovieId == query.MovieId);

            if (query.DateLaterThan.HasValue)
                data = data.Where(x => x.DateTime > query.DateLaterThan);

            if (query.DateEarlierThan.HasValue)
                data = data.Where(x => x.DateTime < query.DateEarlierThan);

            List<ProjectionDomainModel> result = new List<ProjectionDomainModel>();
            ProjectionDomainModel model;
            foreach (var item in data)
            {
                model = new ProjectionDomainModel
                {
                    Id = item.Id,
                    MovieId = item.MovieId,
                    AuditoriumId = item.AuditoriumId,
                    ProjectionTime = item.DateTime,
                    MovieTitle = item.Movie.Title,
                    AuditoriumName = item.Auditorium.AuditName
                };
                result.Add(model);
            }

            return result;
        }

        public async Task<ProjectionDomainModel> GetProjectionByIdAsync(Guid id)
        {
            var projection = await _projectionsRepository.GetByIdAsync(id);

            if (projection == null)
            {
                return null;
            }

            ProjectionDomainModel projectionModel = new ProjectionDomainModel
            {
                Id = projection.Id,
                MovieId = projection.MovieId,
                AuditoriumName = projection.Auditorium.AuditName,
                AuditoriumId = projection.AuditoriumId,
                MovieTitle = projection.Movie.Title,
                ProjectionTime = projection.DateTime
            };

            return projectionModel;
        }
        public async Task<List<FilterProjectionDomainModel>> GetProjectionsWithMovieAndAuditorium(FilterProjectionsQuery query)
        {
            var projections = await _projectionsRepository.GetAllCurrentProjections();


            if (projections.Count() == 0)
            {
                return null;
            }
            if (query.CinemaId > 0)
            {
                projections = projections.Where(projection => projection.Auditorium.CinemaId == query.CinemaId);
            }

            if (query.AuditoriumId > 0)
            {
                projections = projections.Where(projection => projection.AuditoriumId == query.AuditoriumId);
            }

            if (query.MovieId != Guid.Empty)
            {
                projections = projections.Where(projection => projection.MovieId == query.MovieId);
            }

            if (query.DateTime != null)
                projections = projections.Where(projection => projection.DateTime.Date == query.DateTime);

            List<FilterProjectionDomainModel> filterProjectionDomainModels = projections.Select(projection => new FilterProjectionDomainModel
            {
                Id = projection.Id,
                AuditoriumName = projection.Auditorium.AuditName,
                MovieId = projection.MovieId,
                MovieRating = projection.Movie.Rating ?? 0,
                MovieTitle = projection.Movie.Title,
                MovieYear = projection.Movie.Year,
                ProjectionTime = projection.DateTime
            }).ToList();

            return filterProjectionDomainModels;

        }
    }
}
