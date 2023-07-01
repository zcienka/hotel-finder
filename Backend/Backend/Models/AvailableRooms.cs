namespace Backend.Models
{
    public class AvailableRooms
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string Name { get; set; }
        public string HotelId { get; set; }
        public int Rooms { get; set; }
    }
}
