using Backend.Models;

namespace Backend.Core.IRepositories;

public interface IRoomRepository : IGenericRepository<Room>
{
    Task<IEnumerable<Room>> GetAvailableRoomsById(string id);
}