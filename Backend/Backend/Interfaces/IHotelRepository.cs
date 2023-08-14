using Backend.Models;

namespace Backend.Interfaces
{
    public interface IHotelRepository
    {
        IQueryable<Hotel> GetAll();
        IQueryable<Hotel> GetSearchResults(string? name,
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
        public List<Comment> GetComments(string hotelId);
    }
}