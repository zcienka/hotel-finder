namespace Backend.Models
{
    public class ReservationDto
    {
        public required DateTime CheckInDate { get; set; }
        public required DateTime CheckOutDate { get; set; }
        public required string HotelId { get; set; }
        public required string RoomId { get; set; }
        public required string UserId { get; set; }
    }
}