using Microsoft.EntityFrameworkCore;
using MyCinemaDiary.Domain.Entities;
using MyCinemaDiary.Infrastructure.Data;

namespace MyCinemaDiary.Infrastructure.Repositories
{
    public class MoviesRepository : IMoviesRepository
    {
        private readonly AppDBContext _dbContext;

        public MoviesRepository(AppDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task AddMovie(Movie movie)
        {
            _dbContext.Movies.Add(movie);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveMovie(Movie movie)
        {
            _dbContext.Movies.Remove(movie);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Movie?> GetByIdAsync(int id)
        {
            return await _dbContext.Movies.FindAsync(id);
        }

        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
            return await _dbContext.Movies.ToListAsync();
        }

        public async Task<IEnumerable<Movie>> GetLatestMovies(int amount)
        {
            return _dbContext.Movies.OrderByDescending(x => x.Id).Take(amount).ToList();
        }
    }
}
