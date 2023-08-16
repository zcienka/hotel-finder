using Backend.Data;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users
                .OrderByDescending(o => o.Email)
                .ToListAsync();
        }

        public Task<bool> Add(User user)
        {
            _context.Add(user);
            return Save();
        }

        public Task<bool> Delete(User user)
        {
            _context.Remove(user);
            return Save();
        }

        public async Task<User> GetByIdAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(o => o.Email == email);
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public Task<bool> Update(User user)
        {
            _context.Update(user);
            return Save();
        }

        public bool Exists(string email)
        {
            return (_context.Users?.Any(e => e.Email == email)).GetValueOrDefault();
        }

        public  List<Reservation> GetReservations(string userEmail)
        {
            return _context.Users
                .Where(u => u.Email == userEmail)
                .SelectMany(u => u.Reservations)
                .ToList();
        }

        public List<Comment> GetComments(string userEmail)
        {
            return _context.Users
                .Where(u => u.Email == userEmail)
                .SelectMany(r => r.Comments)
                .ToList();
        }
    }
}