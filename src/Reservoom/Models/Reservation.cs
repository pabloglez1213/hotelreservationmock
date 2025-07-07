using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservoom.Models
{
    public class Reservation
    {
        public RoomID RoomID { get; }
        public string Username { get; }
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }

        public TimeSpan Length => EndTime.Subtract(StartTime);

        public Reservation(RoomID roomID, string username, DateTime startTime, DateTime endTime)
        {
            if (roomID == null)
            {
                throw new ArgumentNullException(nameof(roomID), "RoomID cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Username cannot be null or empty.", nameof(username));
            }

            if (startTime >= endTime)
            {
                throw new ArgumentException("Start time must be before end time.", nameof(startTime));
            }

            RoomID = roomID;
            Username = username;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
