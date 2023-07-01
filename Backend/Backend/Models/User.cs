using Microsoft.AspNetCore.Identity;

namespace Backend.Models
{
    public class User : IdentityUser
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}