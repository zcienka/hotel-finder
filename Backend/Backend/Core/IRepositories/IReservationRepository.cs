using Backend.Models;

namespace Backend.Core.IRepositories;

public interface IReservationRepository : IGenericRepository<Reservation>
{
    bool IsRoomInHotel(string hotelId, string roomId);
    IEnumerable<Reservation> GetReservationsForHotel(string hotelId);
}