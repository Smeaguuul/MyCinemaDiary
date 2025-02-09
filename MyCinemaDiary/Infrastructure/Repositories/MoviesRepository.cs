using MyCinemaDiary.Domain.Entities;

namespace MyCinemaDiary.Infrastructure.Repositories
{
    public interface MoviesRepository
    {
        public List<Movie> GetMovies();
        public Movie GetMovie(int id);
        public void AddMovie(Movie movie);
        public void UpdateMovie(Movie movie);
        public void DeleteMovie(int id);
    }
}
