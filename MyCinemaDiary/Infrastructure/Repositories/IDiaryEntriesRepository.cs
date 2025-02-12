using MyCinemaDiary.Domain.Entities;

namespace MyCinemaDiary.Infrastructure.Repositories
{
    public interface IDiaryEntriesRepository
    {
        Task AddDiaryEntry(DiaryEntry diaryEntry);
        Task<IEnumerable<DiaryEntry?>> GetByMovieId(int id);
        Task<IEnumerable<DiaryEntry?>> GetByUserId(int id);
        Task<IEnumerable<DiaryEntry?>> GetByMovieAndUserId(int movieId, int userId);
        Task RemoveDiaryEntry(DiaryEntry diaryEntry);
    }
}