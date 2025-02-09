using MyCinemaDiary.Domain.Entities;
using MyCinemaDiary.Infrastructure.Data;
using MyCinemaDiary.Infrastructure.ExternalApiClients;
using MyCinemaDiary.Infrastructure.Repositories;

namespace MyCinemaDiary.Application
{
    public class Movies
    {
        private IMoviesRepository moviesRepository;
        private TheTvDbAPI theTvDbAPI;

        public Movies(IMoviesRepository moviesRepository)
        {
            this.moviesRepository = moviesRepository;
            this.theTvDbAPI = new TheTvDbAPI();
            this.theTvDbAPI.initialize();
        }

        public async Task<List<Movie>> SearchMovie(string title, int amount)
        {
            var searchResults = await theTvDbAPI.Search(title, amount);
            var movies = new List<Movie>();
            foreach (var result in searchResults.RootElement.GetProperty("data").EnumerateArray())
            {
                var movie = Movie.Parse(result);
                movies.Add(movie);
            }
            return movies;
        }

        public async Task SaveMovie(Movie movie)
        {
            await moviesRepository.AddMovie(movie);
        }
    }
}
