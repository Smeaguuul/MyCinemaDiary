using MyCinemaDiary.Domain.Entities;
using MyCinemaDiary.Infrastructure.ExternalApiClients;
using MyCinemaDiary.Infrastructure.Repositories;

namespace MyCinemaDiary.Application
{
    public class Users
    {
        private IUsersRepository usersRepository;

        public Users(IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public async Task<User> GetUser(int id)
        {
            return await usersRepository.GetByIdAsync(id);
        }

        public async Task AddUser(User user)
        {
            await usersRepository.AddUser(user);
        }
    }
}
