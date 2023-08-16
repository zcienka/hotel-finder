namespace Backend.Requests
{
    public class HotelRequest
    {
        public string? Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Category { get; set; }
    }
}