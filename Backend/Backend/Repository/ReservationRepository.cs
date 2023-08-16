using Backend.Data;
using Backend.Interfaces;
using Backend.Models;
using Backend.Requests;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly ApplicationDbContext _context;

        public ReservationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reservation>> GetAll()
        {
            return await _context.Reservations
                .OrderByDescending(o => o.Id)
                .ToListAsync();
        }

        public Task<bool> Add(Reservation reservation)
        {
            _context.Add(reservation);
            return Save();
        }

        public Task<bool> Delete(Reservation reservation)
        {
            _context.Remove(reservation);
            return Save();
        }

        public async Task<Reservation> GetByIdAsync(string id)
        {
            return await _context.Reservations.FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public Task<bool> Update(Reservation reservation)
        {
            _context.Update(reservation);
            return Save();
        }

        public bool Exists(string id)
        {
            return (_context.Reservations?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public bool HotelExists(string id)
        {
            return (_context.Reservations?.Any(e => e.HotelId == id)).GetValueOrDefault();
        }

        public bool IsRoomInHotel(string hotelId, string roomId)
        {
            return _context.Rooms.Any(r =>
                r.HotelId == hotelId && r.Id == roomId);
        }

        public List<Reservation> GetReservationsForHotel(string hotelId)
        {
            return _context.Reservations
                .Where(r => r.HotelId == hotelId &&
                            r.CheckInDate <= DateTime.Now &&
                            r.CheckOutDate >= DateTime.Now).ToList();
        }

        public bool UserExists(string email)
        {
            return (_context.Users?.Any(e => e.Email == email)).GetValueOrDefault();
        }
    }
}
