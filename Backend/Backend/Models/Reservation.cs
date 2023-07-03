using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class Reservation
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public required DateTime CheckInDate { get; set; }
        public required DateTime CheckOutDate { get; set; }
        public required string HotelId { get; set; }
        public required string RoomId { get; set; }
        public required string UserEmail { get; set; }

    }
}