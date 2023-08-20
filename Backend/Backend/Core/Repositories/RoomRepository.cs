using Backend.Core.IRepositories;
using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Core.Repositories;

public class RoomRepository : GenericRepository<Room>, IRoomRepository
{
    private readonly ApplicationDbContext _context;
    
    public RoomRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Room>> GetAvailableRoomsById(string id)
    {
        // var reservedRoomIds = _context.Reservations
        // .Where(r => r.HotelId == id &&
        // r.CheckInDate <= DateTime.Now &&
        // r.CheckOutDate >= DateTime.Now);

        // var reservedRoomIds = _context.Reservations
        //     .Where(r => r.HotelId == id &&
        //                 r.CheckInDate <= DateTime.Now &&
        //                 r.CheckOutDate >= DateTime.Now).ToList();

        // var xd = reservedRoomIds.Select(r => r.RoomId);
        // .Select(r => r.RoomId);
        var rooms = await dbSet.ToListAsync();
        // .Where(r => !reservedRoomIds.Contains(r.Id) &&
        // r.HotelId == id);
        return rooms;
    }

    public override async Task<IEnumerable<Room>> GetAll()
    {
        return await _context.Rooms
        .OrderByDescending(o => o.Id)
        .ToListAsync();
    }
}