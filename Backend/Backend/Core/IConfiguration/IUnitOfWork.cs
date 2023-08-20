using Backend.Core.IRepositories;

namespace Backend.Core.IConfiguration;

public interface IUnitOfWork
{
    ICommentRepository Comments { get; }
    IHotelRepository Hotels { get; }
    IReservationRepository Reservations { get; }
    IRoomRepository Rooms { get; }
    IUserRepository Users { get; }
    Task CompleteAsync();
}