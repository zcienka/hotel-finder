using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public required string Description { get; set; }
        public required int UserId { get; set; }
        public required int HotelId { get; set; }
    }
}
