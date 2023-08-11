namespace Backend.Models
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}