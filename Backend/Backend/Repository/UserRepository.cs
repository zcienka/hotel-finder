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
                .OrderByDescending(o => o.Id)
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

        public async Task<User> GetByIdAsync(string id)
        {
            return await _context.Users.FirstOrDefaultAsync(o => o.Id == id);
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

        public bool Exists(string id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public  List<Reservation> GetUserReservations()
        {
            return  _context.Users
                .SelectMany(r => r.Reservations)
                .ToList();
        }
    }
}