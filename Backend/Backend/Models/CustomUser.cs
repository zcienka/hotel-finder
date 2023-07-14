using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class CustomUser
    {
        [Key]
        public required string Email { get; set; }

        public List<string> LikedHotels { get; set; }
    }
}