using Backend.Core.IRepositories;
using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Core.Repositories;

public class ReservationRepository : GenericRepository<Reservation>, IReservationRepository
{
    private readonly ApplicationDbContext _context;

    public ReservationRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public bool IsRoomInHotel(string hotelId, string roomId)
    {
        return dbSet.Any(r =>
            r.HotelId == hotelId && r.Id == roomId);
    }


    public async Task<IEnumerable<Reservation>> GetReservationsForHotel(string hotelId)
    {
        return await dbSet
            .Where(r => r.HotelId == hotelId &&
                        r.CheckInDate <= DateTime.Now &&
                        r.CheckOutDate >= DateTime.Now).ToListAsync();
    }

    public override async Task<IEnumerable<Reservation>> GetAll()
    {
        return await _context.Reservations
            .OrderByDescending(o => o.Id)
            .ToListAsync();
    }
}