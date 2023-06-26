using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public required DateTime CheckInDate { get; set; }
        public required DateTime CheckOutDate { get; set; }
        public required int HotelId { get; set; }
        public required int RoomId { get; set; }
        public required int UserId { get; set; }

    }
}