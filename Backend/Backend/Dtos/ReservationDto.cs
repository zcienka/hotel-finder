using Backend.Models;

namespace Backend.Dtos
{
    public class ReservationDto
    {
        public string? Id { get; set; }
        public required DateTimeOffset CheckInDate { get; set; }
        public required DateTimeOffset CheckOutDate { get; set; }
        public required string HotelId { get; set; }
        public required string RoomId { get; set; }
        public required string UserEmail { get; set; }
        public required string Image { get; set;}
    }
}