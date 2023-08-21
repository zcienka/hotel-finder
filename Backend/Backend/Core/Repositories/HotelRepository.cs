using Backend.Core.IRepositories;
using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Core.Repositories;

public class HotelRepository : GenericRepository<Hotel>, IHotelRepository
{
    private readonly ApplicationDbContext _context;

    public HotelRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public IEnumerable<Hotel> GetSearchResults(string? name,
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

    public override async Task<IEnumerable<Hotel>> GetAll()
    {
        return await dbSet
            .OrderByDescending(o => o.Id).ToListAsync();
    }
}