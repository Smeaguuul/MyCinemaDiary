using Microsoft.AspNetCore.Mvc;
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

        public async Task Register(User user)
        {
            try
            {
                _dbContext.Users.Add(user);
            }
            catch (Exception e)
            {
                throw new Exception("User cant be added!");
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            return user;
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
