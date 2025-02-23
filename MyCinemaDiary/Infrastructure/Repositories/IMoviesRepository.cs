using MyCinemaDiary.Domain.Entities;

namespace MyCinemaDiary.Infrastructure.Repositories
{
    public interface IMoviesRepository
    {
        Task AddMovie(Movie movie);
        Task<IEnumerable<Movie>> GetAllAsync();
        Task<Movie?> GetByIdAsync(int id);
        Task RemoveMovie(Movie movie);
        Task<IEnumerable<Movie>> GetLatestMovies(int amount);
    }
}