using BCrypt.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyCinemaDiary.Domain.Entities;
using MyCinemaDiary.Infrastructure.ExternalApiClients;
using MyCinemaDiary.Infrastructure.Repositories;

namespace MyCinemaDiary.Application
{
    public class Users
    {
        private readonly IUsersRepository usersRepository;

        public Users(IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public async Task<User> GetUser(int id)
        {
            return await usersRepository.GetByIdAsync(id);
        }

        public async Task Register(string name, string username, string password)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                Name = name,
                Username = username,
                PasswordHash = passwordHash
            };

            await usersRepository.Register(user);
        }

        public async Task<User> Login(string username, string password)
        {
            var user = await usersRepository.GetByUsernameAsync(username);
            
            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                throw new Exception("Invalid password");
            }

            return user;
        }
    }
}
