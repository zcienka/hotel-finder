using Backend.Models;

namespace Backend.Core.IRepositories;

public interface IUserRepository : IGenericRepository<User>
{
    List<Reservation> GetReservations(string userEmail);
    List<Comment> GetComments(string userEmail);
}