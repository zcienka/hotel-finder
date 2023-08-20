using Backend.Core.IRepositories;
using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Core.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public List<Reservation> GetReservations(string userEmail)
    {
        return dbSet
            .Where(u => u.Email == userEmail)
            .SelectMany(u => u.Reservations)
            .ToList();
    }

    public List<Comment> GetComments(string userEmail)
    {
        return dbSet
            .Where(u => u.Email == userEmail)
            .SelectMany(r => r.Comments)
            .ToList();
    }

    public override async Task<bool> Exists(string email)
    {
        var entity = await dbSet.FindAsync(email);
        return entity != null;
    }

    public override async Task<IEnumerable<User>> GetAll()
    {
        return await dbSet
            .OrderByDescending(o => o.Email)
            .ToListAsync();
    }

 
}