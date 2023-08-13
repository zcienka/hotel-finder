using Backend.Models;

namespace Backend.Interfaces
{
    public interface IReservationRepository
    {
        Task<IEnumerable<Reservation>> GetAll();
        Task<bool> Add(Reservation reservation);
        Task<bool> Delete(Reservation reservation);
        Task<Reservation> GetByIdAsync(string id);
        Task<bool> Save();
        Task<bool> Update(Reservation reservation);
        bool Exists(string id);
        bool HotelExists(string id);
        bool IsRoomInHotel(string hotelId, string roomId);
        List<Reservation> GetReservationsForHotel(string hotelId);
    }
}
