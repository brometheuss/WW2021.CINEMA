using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Data.Entities;

namespace WinterWorkShop.Cinema.Repositories
{
    public interface IReservationsRepository : IRepository<Reservation> 
    {
        IEnumerable<Reservation> GetReservationByProjectionId(Guid projectionId);
        Task<IEnumerable<Reservation>> GetReservationsByUserId(Guid userId);
    }
    public class ReservationsRepository : IReservationsRepository
    {
        private CinemaContext _cinemaContext;

        public ReservationsRepository(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
        }

        public Reservation Delete(object id)
        {
            Reservation existing = _cinemaContext.Reservations.Find(id);
            var result = _cinemaContext.Reservations.Remove(existing);

            return result.Entity;
        }

        public async Task<IEnumerable<Reservation>> GetAll()
        {
            var data = await _cinemaContext.Reservations
                .Include(rs => rs.ReservationSeats)
                .ThenInclude(s => s.Seat)
                .ThenInclude(a => a.Auditorium)
                .ToListAsync();

            return data;
        }

        public async Task<Reservation> GetByIdAsync(object id)
        {
            var data = await _cinemaContext.Reservations.FindAsync(id);

            return data;
        }

        public IEnumerable<Reservation> GetReservationByProjectionId(Guid projectionId)
        {
            var reservation = _cinemaContext.Reservations
                .Include(rs => rs.ReservationSeats)
                .ThenInclude(s => s.Seat)
                .ThenInclude(a => a.Auditorium)
                .Where(reservation => reservation.ProjectionId == projectionId);

            return reservation;
        }

        public Reservation Insert(Reservation obj)
        {
            return _cinemaContext.Reservations.Add(obj).Entity;
        }

        public void Save()
        {
            _cinemaContext.SaveChanges();
        }

        public Reservation Update(Reservation obj)
        {
            var updatedEntry = _cinemaContext.Reservations.Attach(obj);
            _cinemaContext.Entry(obj).State = EntityState.Modified;

            return updatedEntry.Entity;
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByUserId(Guid userId)
        {
            var data = await _cinemaContext.Reservations
                .Include(projection => projection.Projection)
                .ThenInclude(movie => movie.Movie)
                .Include(projection => projection.Projection)
                .ThenInclude(auditorium => auditorium.Auditorium)
                .Where(reservation => reservation.UserId == userId)
                .ToListAsync();

            return data;
        }
    }
}
