using Backend.Models;

namespace Backend.Interfaces
{
    public interface IRoomRepository
    {
        Task<IEnumerable<Room>> GetAll();
        bool HotelExists(string id);
        List<Room> GetAvailableRoomsById(string id);
        Task<bool> Add(Room room);
        Task<bool> Delete(Room room);
        Task<Room> GetByIdAsync(string id);
        Task<bool> Save();
        Task<bool> Update(Room room);
        bool Exists(string id);
    }
}
