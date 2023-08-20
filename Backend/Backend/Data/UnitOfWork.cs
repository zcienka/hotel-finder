using Backend.Core.IConfiguration;
using Backend.Core.IRepositories;
using Backend.Core.Repositories;

namespace Backend.Data;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    protected readonly ApplicationDbContext _context;

    public ICommentRepository Comments { get; private set; }
    public IHotelRepository Hotels { get; private set; }
    public IReservationRepository Reservations { get; private set; }
    public IRoomRepository Rooms { get; private set; }
    public IUserRepository Users { get; private set; }


    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Comments = new CommentRepository(_context);
        Hotels = new HotelRepository(_context);
        Reservations = new ReservationRepository(_context);
        Rooms = new RoomRepository(_context);
        Users = new UserRepository(_context);
    }

    public async Task CompleteAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}