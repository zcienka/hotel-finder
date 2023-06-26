namespace Backend.Models
{
    public class ReservationDto
    {
        public required DateTime CheckInDate { get; set; }
        public required DateTime CheckOutDate { get; set; }
        public required int HotelId { get; set; }
        public required int RoomId { get; set; }
        public required int UserId { get; set; }
    }
}