namespace Backend.Models
{
    public class User
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required List<Reservation> Reservations { get; set; }
    }
}