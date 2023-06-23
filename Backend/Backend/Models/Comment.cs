namespace Backend.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public required string Description { get; set; }
        public required string UserEmail { get; set; }
        public required int HotelId { get; set; }
    }
}
