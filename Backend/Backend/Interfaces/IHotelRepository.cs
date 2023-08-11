using Backend.Models;

namespace Backend.Interfaces
{
    public interface IHotelRepository
    {
        Task<IEnumerable<Hotel>> GetAll();

        IEnumerable<Hotel> GetSearchResults(string? name,
            string? city,
            DateTimeOffset? checkInDate,
            DateTimeOffset? checkOutDate,
            int? roomCount,
            string? category);

        Task<bool> Add(Hotel hotel);
        Task<bool> Delete(Hotel hotel);
        Task<Hotel> GetByIdAsync(string id);
        Task<bool> Save();
        Task<bool> Update(Hotel hotel);
        bool Exists(string id);
    }
}