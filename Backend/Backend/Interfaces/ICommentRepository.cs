using Backend.Models;

namespace Backend.Interfaces
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetAll();
        Task<bool> Add(Comment comment);
        Task<bool> Delete(Comment comment);
        Task<Comment> GetByIdAsync(string id);
        Task<bool> Save();
        Task<bool> Update(Comment comment);
        bool Exists(string id);
        bool HotelExists(string id);
        bool UserExists(string id);
    }
}
