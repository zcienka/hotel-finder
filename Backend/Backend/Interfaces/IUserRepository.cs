using Backend.Models;

namespace Backend.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();
        Task<bool> Add(User user);
        Task<bool> Delete(User user);
        Task<User> GetByIdAsync(string email);
        Task<bool> Save();
        Task<bool> Update(User user);
        bool Exists(string email);
        List<Reservation> GetReservations(string userEmail);
        List<Comment> GetComments(string userEmail);
    }
}
