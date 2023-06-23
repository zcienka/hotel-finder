namespace Backend.Models
{
    public class AvailableRooms
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int HotelId { get; set; }
        public int Rooms { get; set; }
    }
}
