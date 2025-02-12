using Microsoft.EntityFrameworkCore;
using MyCinemaDiary.Domain.Entities;
using MyCinemaDiary.Infrastructure.Data;

namespace MyCinemaDiary.Infrastructure.Repositories
{
    public class DiaryEntriesRepository : IDiaryEntriesRepository
    {
        private readonly AppDBContext _dbContext;

        public DiaryEntriesRepository(AppDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task AddDiaryEntry(DiaryEntry diaryEntry)
        {
            _dbContext.DiaryEntries.Add(diaryEntry);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveDiaryEntry(DiaryEntry diaryEntry)
        {
            _dbContext.DiaryEntries.Remove(diaryEntry);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<DiaryEntry?>> GetByUserId(int id)
        {
            return await _dbContext.DiaryEntries.Where((entry) => entry.User.Id == id).ToListAsync();
        }

        public async Task<IEnumerable<DiaryEntry?>> GetByMovieId(int id)
        {
            return await _dbContext.DiaryEntries.Where((entry) => entry.Movie.Id == id).ToListAsync();
        }

        public async Task<IEnumerable<DiaryEntry?>> GetByMovieAndUserId(int movieId, int userId)
        {
            return await _dbContext.DiaryEntries.Where((entry) => entry.Movie.Id == movieId && entry.User.Id == userId).ToListAsync();

        }


    }
}
