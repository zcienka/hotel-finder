using System.Xml.Linq;
using Backend.Data;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository
{
    public class HotelRepository : IHotelRepository
    {
        private readonly ApplicationDbContext _context;

        public HotelRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Hotel> GetAll()
        {
            return _context.Hotels
                .OrderByDescending(o => o.Id);
        }

        public IQueryable<Hotel> GetSearchResults(string? name, 
            string? city, 
            DateTimeOffset? checkInDate,
            DateTimeOffset? checkOutDate,
            int? roomCount,
            string? category)
        {
            var hotels = _context.Hotels.ToList().AsQueryable();

            if (name != null)
            {
                hotels = hotels.Where(hotel => hotel.Name.ToLower().Contains(name.ToLower()));
            }

            if (category != null)
            {
                hotels = hotels.Where(hotel => hotel.Category.ToLower().Contains(category.ToLower()));
            }

            if (roomCount != null)
            {
                hotels = from hotel in hotels
                    join room in _context.Rooms on hotel.Id equals room.HotelId into hotelRooms
                    where hotelRooms.Count() >= roomCount
                    select hotel;
            }

            if (city != null)
            {
                hotels = hotels.Where(hotel => hotel.City.ToLower().Contains(city.ToLower()));
            }

            if (checkInDate != null && checkOutDate != null)
            {
                var hotelsWithFreeRooms = (from hotel in hotels
                        join room in _context.Rooms on hotel.Id equals room.HotelId
                        where !_context.Reservations.Any(reservation =>
                            reservation.HotelId == hotel.Id &&
                            reservation.RoomId == room.Id &&
                            (
                                (reservation.CheckInDate <= checkInDate && reservation.CheckOutDate >= checkInDate) ||
                                (reservation.CheckInDate <= checkOutDate && reservation.CheckOutDate >= checkOutDate) ||
                                (reservation.CheckInDate >= checkInDate && reservation.CheckOutDate <= checkOutDate)
                            ))
                        select hotel)
                    .Distinct();

                hotels = hotelsWithFreeRooms;
            }

            return hotels;
        }

        public Task<bool> Add(Hotel hotel)
        {
            _context.Add(hotel);
            return Save();
        }

        public Task<bool> Delete(Hotel hotel)
        {
            _context.Remove(hotel);
            return Save();
        }

        public async Task<Hotel> GetByIdAsync(string id)
        {
            return await _context.Hotels.FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public Task<bool> Update(Hotel hotel)
        {
            _context.Update(hotel);
            return Save();
        }

        public bool Exists(string id)
        {
            return (_context.Hotels?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public List<Comment> GetComments(string hotelId)
        {
            return _context.Comments.Where(q => q.HotelId == hotelId).ToList();
        }
    }
}
