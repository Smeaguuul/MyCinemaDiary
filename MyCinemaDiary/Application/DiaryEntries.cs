using MyCinemaDiary.Domain.Entities;
using MyCinemaDiary.Infrastructure.Repositories;

namespace MyCinemaDiary.Application
{
    public class DiaryEntries
    {
        private readonly IDiaryEntriesRepository _diaryEntriesRepository;
        private readonly IMoviesRepository _moviesRepository;
        private readonly IUsersRepository _usersRepository;

        public DiaryEntries(IDiaryEntriesRepository diaryEntriesRepository, IMoviesRepository moviesRepository, IUsersRepository usersRepository)
        {
            _diaryEntriesRepository = diaryEntriesRepository;
            _moviesRepository = moviesRepository;
            _usersRepository = usersRepository;
        }

        public async Task AddDiaryEntry(DiaryEntry diaryEntry, int movieId, int userId)
        {
            // TODO diaryentry class should change to just include id's of movie and user
            var movie = await _moviesRepository.GetByIdAsync(movieId);
            var user = await _usersRepository.GetByIdAsync(userId);
            if (movie == null) throw new Exception("Movie not found");
            if (user == null) throw new Exception("User not found");
            diaryEntry.Movie = movie;
            diaryEntry.User = user;
            await _diaryEntriesRepository.AddDiaryEntry(diaryEntry);
        }

        public async Task RemoveDiaryEntry(DiaryEntry diaryEntry)
        {
            await _diaryEntriesRepository.RemoveDiaryEntry(diaryEntry);
        }

        public async Task<IEnumerable<DiaryEntry?>> GetByUserId(int id)
        {
            return await _diaryEntriesRepository.GetByUserId(id);
        }

        public async Task<IEnumerable<DiaryEntry?>> GetByMovieId(int id)
        {
            return await _diaryEntriesRepository.GetByMovieId(id);
        }

        public async Task<IEnumerable<DiaryEntry?>> GetByMovieAndUserId(int movieId, int userId)
        {
            return await _diaryEntriesRepository.GetByMovieAndUserId(movieId, userId);
        }
    }
}
