using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class Reservation
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public required DateTimeOffset CheckInDate { get; set; }
        public required DateTimeOffset CheckOutDate { get; set; }
        public required string HotelId { get; set; }
        public required string RoomId { get; set; }
        public required string UserEmail { get; set; }

        [ForeignKey("HotelId")] 
        public Hotel Hotel { get; set; }
    }
}