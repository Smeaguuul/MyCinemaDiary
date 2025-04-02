using Microsoft.EntityFrameworkCore;
using MyCinemaDiary.Domain.Entities;
using MyCinemaDiary.Infrastructure.Repositories;

namespace MyCinemaDiary.Application
{
    public class DiaryEntries
    {
        private readonly IRepository<DiaryEntry> _diaryEntriesRepository;
        private readonly IRepository<Movie> _moviesRepository;
        private readonly IRepository<User> _usersRepository;

        public DiaryEntries(IRepository<DiaryEntry> diaryEntriesRepository, IRepository<Movie> moviesRepository, IRepository<User> usersRepository)
        {
            _diaryEntriesRepository = diaryEntriesRepository;
            _moviesRepository = moviesRepository;
            _usersRepository = usersRepository;
        }

        public async Task AddDiaryEntry(DiaryEntry diaryEntry, int movieId, int userId)
        {
            // TODO diaryentry class should change to just include id's of movie and user
            var movie = await _moviesRepository.FirstOrDefaultAsync(movie => movie.Id == movieId);
            var user = await _usersRepository.FirstOrDefaultAsync(movie => movie.Id == userId);
            if (movie == null) throw new Exception("Movie not found");
            if (user == null) throw new Exception("User not found");
            diaryEntry.Movie = movie;
            diaryEntry.User = user;
            await _diaryEntriesRepository.AddAsync(diaryEntry);
        }

        public async Task RemoveDiaryEntry(DiaryEntry diaryEntry)
        {
            await _diaryEntriesRepository.RemoveAsync(diaryEntry);
        }

        public async Task<IEnumerable<DiaryEntry?>> GetByUserId(int id)
        {
            return await _diaryEntriesRepository.GetAllAsync(
                predicate: entry => entry.User.Id == id, 
                include: query => query.Include(entry => entry.Movie)); // Uses eager loading aswell
        }

        public async Task<IEnumerable<DiaryEntry?>> GetByMovieId(int id)
        {
            return await _diaryEntriesRepository.GetAllAsync(
                predicate: entry => entry.Movie.Id == id, 
                include: query => query.Include(entry => entry.User));
        }

        public async Task<IEnumerable<DiaryEntry?>> GetByMovieAndUserId(int movieId, int userId)
        {
            return await _diaryEntriesRepository.GetAllAsync(
                predicate: entry => entry.Movie.Id == movieId && entry.User.Id == userId);
        }
    }
}
