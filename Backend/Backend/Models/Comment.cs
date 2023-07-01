using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Comment
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public required string Description { get; set; }
        public required string UserId { get; set; }
        public required string HotelId { get; set; }
    }
}
