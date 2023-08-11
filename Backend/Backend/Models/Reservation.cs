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
        public required string UserId { get; set; }

        [ForeignKey("HotelId")]
        public virtual Hotel Hotel { get; set; }
        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}