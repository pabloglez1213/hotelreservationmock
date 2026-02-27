using System;
using Reservoom.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservoom.Models
{
    public class Verbo
    {
        private readonly ReservationBook _reservationBook;

        public string Name { get; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string PropertyType { get; set; }

        public int NumberOfBedrooms { get; set; }

        public int NumberOfBathrooms { get; set; }

        public int MaxGuests { get; set; }

        public decimal PricePerNight { get; set; }

        public string HostName { get; set; }

        public string Description { get; set; }

        public bool InstantBookAvailable { get; set; }

        public int MinimumNights { get; set; }

        public string CancellationPolicy { get; set; }

        public double AverageRating { get; set; }

        public int TotalReviews { get; set; }

        public double CleanlinessRating { get; set; }

        public double LocationRating { get; set; }

        public double ValueRating { get; set; }

        public double CommunicationRating { get; set; }

        public double CheckInRating { get; set; }

        public double AccuracyRating { get; set; }

        public Verbo(string name, ReservationBook reservationBook)
        {
            Name = name;
            _reservationBook = reservationBook;
        }

        /// <summary>
        /// Get all reservations.
        /// </summary>
        /// <returns>All reservations in the verbo reservation book.</returns>
        public async Task<IEnumerable<Reservation>> GetAllReservations()
        {
            return await _reservationBook.GetAllReservations();
        }

        /// <summary>
        /// Make a reservation.
        /// </summary>
        /// <param name="reservation">The incoming reservation.</param>
        /// <exception cref="InvalidReservationTimeRangeException">Thrown if reservation start time is after end time.</exception>
        /// <exception cref="ReservationConflictException">Thrown if incoming reservation conflicts with existing reservation.</exception>
        public async Task MakeReservation(Reservation reservation)
        {
            await _reservationBook.AddReservation(reservation);
        }

        /// <summary>
        /// Get all reservations for a specific user.
        /// </summary>
        /// <param name="username">The username to filter reservations by.</param>
        /// <returns>All reservations for the specified user.</returns>
        public async Task<IEnumerable<Reservation>> GetReservationsForUser(string username)
        {
            IEnumerable<Reservation> allReservations = await _reservationBook.GetAllReservations();
            return allReservations.Where(r => r.Username?.Equals(username, StringComparison.OrdinalIgnoreCase) == true);
        }

        /// <summary>
        /// Get all reservations within a specified date range.
        /// </summary>
        /// <param name="startDate">The start date of the range.</param>
        /// <param name="endDate">The end date of the range.</param>
        /// <returns>All reservations that overlap with the specified date range.</returns>
        public async Task<IEnumerable<Reservation>> GetReservationsByDateRange(DateTime startDate, DateTime endDate)
        {
            IEnumerable<Reservation> allReservations = await _reservationBook.GetAllReservations();
            return allReservations.Where(r => r.StartTime < endDate && r.EndTime > startDate);
        }

        /// <summary>
        /// Check if a reservation would conflict with existing reservations.
        /// </summary>
        /// <param name="reservation">The reservation to check for conflicts.</param>
        /// <returns>True if there is a conflicting reservation, false otherwise.</returns>
        public async Task<bool> HasConflictingReservation(Reservation reservation)
        {
            IEnumerable<Reservation> allReservations = await _reservationBook.GetAllReservations();
            return allReservations.Any(r => 
                r.RoomID == reservation.RoomID && 
                r.StartTime < reservation.EndTime && 
                r.EndTime > reservation.StartTime);
        }
    }
}
