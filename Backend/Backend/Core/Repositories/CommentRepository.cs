using Backend.Core.IRepositories;
using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Core.Repositories;

public class CommentRepository : GenericRepository<Comment>, ICommentRepository
{
    private readonly ApplicationDbContext _context;

    public CommentRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<List<Comment>> GetAllByHotel(string hotelId)
    {
        return await dbSet.Where(q => q.HotelId == hotelId).ToListAsync();
    }
}