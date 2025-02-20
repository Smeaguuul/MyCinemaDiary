using BCrypt.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyCinemaDiary.Application.DTO_s;
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

        public async Task Register(string name, string username, string password)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                Name = name,
                Username = username,
                PasswordHash = passwordHash
            };
            try
            {
                await usersRepository.Register(user);
            }
            catch (Exception)
            {
                throw new Exception("Username needs to be unique!");
            }
        }

        public async Task<string> Login(string username, string password)
        {
            var user = await usersRepository.GetByUsernameAsync(username);

            if (user == null)
            {
                throw new Exception("User doesn't exist");
            }
            
            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                throw new Exception("Invalid password");
            }

            // Generate a JWT token
            var token = TokenHandler.GenerateJwtToken(user);


            return token;
        }
    }
}
