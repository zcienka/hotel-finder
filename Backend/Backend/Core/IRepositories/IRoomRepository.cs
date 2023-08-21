using Backend.Models;

namespace Backend.Core.IRepositories;

public interface IRoomRepository : IGenericRepository<Room>
{
    IEnumerable<Room> GetAvailableRoomsById(string id);
}