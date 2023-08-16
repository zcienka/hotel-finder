using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class Comment
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public required string Description { get; set; }
        public required string UserEmail { get; set; }
        public required string HotelId { get; set; }

        [ForeignKey("UserEmail")]
        public virtual User User { get; set; }
        [ForeignKey("HotelId")]
        public virtual Hotel Hotel { get; set; }
    }
}
