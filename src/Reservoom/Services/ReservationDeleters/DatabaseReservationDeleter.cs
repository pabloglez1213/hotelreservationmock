using Microsoft.EntityFrameworkCore;
using Reservoom.DbContexts;
using Reservoom.DTOs;
using Reservoom.Models;
using System.Threading.Tasks;

namespace Reservoom.Services.ReservationDeleters
{
    public class DatabaseReservationDeleter : IReservationDeleter
    {
        private readonly IReservoomDbContextFactory _dbContextFactory;

        public DatabaseReservationDeleter(IReservoomDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task DeleteReservation(Reservation reservation)
        {
            using (ReservoomDbContext context = _dbContextFactory.CreateDbContext())
            {
                ReservationDTO reservationDTO = await context.Reservations.FirstOrDefaultAsync(r =>
                    r.FloorNumber == reservation.RoomID.FloorNumber &&
                    r.RoomNumber == reservation.RoomID.RoomNumber &&
                    r.Username == reservation.Username &&
                    r.StartTime == reservation.StartTime);

                if (reservationDTO != null)
                {
                    context.Reservations.Remove(reservationDTO);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
