namespace Backend.Models
{
    public class HotelRequest
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }
        public required string PhoneNumber { get; set; }
        public required List<string> Image { get; set; }
        public required string Category { get; set; }
    }
}