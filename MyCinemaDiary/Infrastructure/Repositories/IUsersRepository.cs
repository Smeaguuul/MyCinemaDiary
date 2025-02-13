using MyCinemaDiary.Domain.Entities;

namespace MyCinemaDiary.Infrastructure.Repositories
{
    public interface IUsersRepository
    {
        Task Register(User user);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task RemoveUser(User user);
        Task<User?> GetByUsernameAsync(string username);
    }
}