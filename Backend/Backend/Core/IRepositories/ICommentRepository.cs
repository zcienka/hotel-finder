using Backend.Models;

namespace Backend.Core.IRepositories;

public interface ICommentRepository : IGenericRepository<Comment>
{
    Task<List<Comment>> GetAllByHotel(string hotelId);
}