using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class User
    {
        public required string Name { get; set; }
        public required string LastName { get; set; }

        [Key]
        public required string Email { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}