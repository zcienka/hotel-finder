using Backend.Data;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository
{
    public class RoomRepository : IRoomRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IReservationRepository _reservationRepository;

        public RoomRepository(ApplicationDbContext context, IReservationRepository reservationRepository)
        {
            _context = context;
            _reservationRepository = reservationRepository;
        }

        public async Task<IEnumerable<Room>> GetAll()
        {
            return await _context.Rooms
                .OrderByDescending(o => o.Id)
                .ToListAsync();
        }
        public bool HotelExists(string id)
        {
            return (_context.Hotels?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public List<Room> GetAvailableRoomsById(string id)
        {
            var reservedRoomIds = _reservationRepository.GetReservationsForHotel(id)
                .Select(r => r.RoomId)
                .ToList();

            var rooms = _context.Rooms
                .Where(r => !reservedRoomIds.Contains(r.Id) &&
                            r.HotelId == id)
                .ToList();
            return rooms;
        }

        public Task<bool> Add(Room room)
        {
            _context.Add(room);
            return Save();
        }

        public Task<bool> Delete(Room room)
        {
            _context.Remove(room);
            return Save();
        }

        public async Task<Room> GetByIdAsync(string id)
        {
            return await _context.Rooms.FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public Task<bool> Update(Room room)
        {
            _context.Update(room);
            return Save();
        }

        public bool Exists(string id)
        {
            return (_context.Rooms?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
