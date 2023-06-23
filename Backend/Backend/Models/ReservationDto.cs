namespace Backend.Models
{
    public class ReservationDto
    {
        public required DateTime CheckInDate { get; set; }
        public required DateTime CheckOutDate { get; set; }
        public required int HotelId { get; set; }
        public required int RoomsNumber { get; set; }
    }
}