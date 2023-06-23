namespace Backend.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public required DateTime CheckInDate { get; set; }
        public required DateTime CheckOutDate { get; set; }
        public required int HotelId { get; set; }
        public required List<Room> RoomsList { get; set; }
        public required int UserEmail { get; set; }
    }
}