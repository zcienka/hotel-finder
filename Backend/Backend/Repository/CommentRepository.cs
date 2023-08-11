using Backend.Data;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetAll()
        {
            return await _context.Comments
                .OrderByDescending(o => o.Id)
                .ToListAsync();
        }

        public Task<bool> Add(Comment comment)
        {
            _context.Add(comment);
            return Save();
        }

        public Task<bool> Delete(Comment comment)
        {
            _context.Remove(comment);
            return Save();
        }

        public async Task<Comment> GetByIdAsync(string id)
        {
            return await _context.Comments.FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public Task<bool> Update(Comment comment)
        {
            _context.Update(comment);
            return Save();
        }

        public bool Exists(string id)
        {
            return (_context.Comments?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public bool HotelExists(string id)
        {
            return (_context.Comments?.Any(e => e.HotelId == id)).GetValueOrDefault();
        }

    }
}
