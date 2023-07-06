namespace Backend.Dtos
{
    public class ReservationDto
    {
        public required DateTimeOffset CheckInDate { get; set; }
        public required DateTimeOffset CheckOutDate { get; set; }
        public required string HotelId { get; set; }
        public required string RoomId { get; set; }
        public required string UserEmail { get; set; }
    }
}