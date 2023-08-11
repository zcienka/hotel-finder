using Backend.Models;

namespace Backend.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();
        Task<bool> Add(User user);
        Task<bool> Delete(User user);
        Task<User> GetByIdAsync(string id);
        Task<bool> Save();
        Task<bool> Update(User user);
        bool Exists(string id);
        List<Reservation> GetReservations(string userId);
        List<Comment> GetComments(string userId);
    }
}
