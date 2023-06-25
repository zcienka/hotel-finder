namespace Backend.Models
{
    public class CommentDto
    {
        public required string Description { get; set; }
        public required string UserEmail { get; set; }
        public required int HotelId { get; set; }
    }
}