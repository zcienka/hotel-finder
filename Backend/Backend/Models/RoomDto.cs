namespace Backend.Models
{
    public class RoomDto
    {
        public required int Capacity { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required float Price { get; set; }
        public required int HotelId { get; set; }
    }
}