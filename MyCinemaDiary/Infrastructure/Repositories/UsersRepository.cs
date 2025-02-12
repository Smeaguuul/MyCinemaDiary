using Microsoft.EntityFrameworkCore;
using MyCinemaDiary.Domain.Entities;
using MyCinemaDiary.Infrastructure.Data;

namespace MyCinemaDiary.Infrastructure.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly AppDBContext _dbContext;

        public UsersRepository(AppDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task AddUser(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveUser(User user)
        {
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }
    }
}
