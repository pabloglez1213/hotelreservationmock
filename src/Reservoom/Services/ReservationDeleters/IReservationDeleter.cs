using Reservoom.Models;
using System.Threading.Tasks;

namespace Reservoom.Services.ReservationDeleters
{
    public interface IReservationDeleter
    {
        Task DeleteReservation(Reservation reservation);
    }
}
