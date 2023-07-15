namespace Backend.Requests
{
    public class ReservationRequest
    {
        public required DateTimeOffset CheckInDate { get; set; }
        public required DateTimeOffset CheckOutDate { get; set; }
        public required string HotelId { get; set; }
        public required string RoomId { get; set; }
        public required string UserEmail { get; set; }
    }
}
