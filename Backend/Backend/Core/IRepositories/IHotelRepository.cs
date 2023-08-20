using Backend.Models;

namespace Backend.Core.IRepositories;

public interface IHotelRepository : IGenericRepository<Hotel>
{
    IEnumerable<Hotel> GetSearchResults(string? name,
        string? city,
        DateTimeOffset? checkInDate,
        DateTimeOffset? checkOutDate,
        int? roomCount,
        string? category);
}