﻿using MyCinemaDiary.Domain.Entities;

namespace MyCinemaDiary.Infrastructure.Repositories
{
    public interface IUsersRepository
    {
        Task AddEntry(User user, DiaryEntry diaryEntry);
        Task AddUser(User user);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task RemoveUser(User user);
    }
}