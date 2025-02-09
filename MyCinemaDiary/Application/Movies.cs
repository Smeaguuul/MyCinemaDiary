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
                var movie = new Movie
                {
                    Country = result.GetProperty("country").GetString(),
                    Director = result.GetProperty("director").GetString(),
                    ExtendedTitle = result.GetProperty("extended_title").GetString(),
                    Name = result.GetProperty("name").GetString(),
                    FirstAirTime = result.GetProperty("first_air_time").GetDateTime(),
                    Overview = result.GetProperty("overview").GetString(),
                    PrimaryLanguage = result.GetProperty("primary_language").GetString(),
                    PrimaryType = result.GetProperty("primary_type").GetString(),
                    Status = result.GetProperty("status").GetString(),
                    Year = int.Parse(result.GetProperty("year").GetString()),
                    Slug = result.GetProperty("slug").GetString(),
                    ImageUrl = result.GetProperty("image_url").GetString(),
                    Thumbnail = result.GetProperty("thumbnail").GetString(),
                    TvdbId = result.GetProperty("tvdb_id").GetString(),
                    ImdbId = result.GetProperty("remote_ids").EnumerateArray().Where(element => element.GetProperty("sourceName").ToString().Equals("IMDB")).First().GetProperty("id").GetString(),
                    TmdbId = result.GetProperty("remote_ids").EnumerateArray().Where(element => element.GetProperty("sourceName").ToString().Equals("TheMovieDB.com")).First().GetProperty("id").GetString(),
                    MovieGenres = result.GetProperty("genres").EnumerateArray().Select(element => element.ToString()).ToList(),
                };
                movies.Add(movie);
            }
            return movies;
        }
    }
}
