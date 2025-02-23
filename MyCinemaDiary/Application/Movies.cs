using MyCinemaDiary.Domain.Entities;
using MyCinemaDiary.Infrastructure.Data;
using MyCinemaDiary.Infrastructure.ExternalApiClients;
using MyCinemaDiary.Infrastructure.Repositories;
using MyCinemaDiary.Infrastructure.Services;

namespace MyCinemaDiary.Application
{
    public class Movies
    {
        private IMoviesRepository _moviesRepository;
        private TheTvDbAPI _theTvDbAPI;
        private HttpClientService _httpClientService;

        public Movies(IMoviesRepository moviesRepository, TheTvDbAPI theTvDbAPI, HttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _moviesRepository = moviesRepository;
            _theTvDbAPI = theTvDbAPI;
            _theTvDbAPI.initialize();
        }

        public async Task<List<Movie>> SearchMovie(string title, int amount)
        {
            var searchResults = await _theTvDbAPI.Search(title, amount);
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
            // Download the thumbnail and image
            var movieThumbnail = movie.Thumbnail;
            var movieImage = movie.ImageUrl;
            byte[] thumbnailBytes = await _httpClientService.GetByteArrayAsync(movieThumbnail);
            byte[] imageBytes = await _httpClientService.GetByteArrayAsync(movieImage);

            // Save the thumbnail and image to the wwwroot/images folder
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img");
            var thumbnailName = movie.TvdbId + "Thumbnail.jpg";
            var imageName = movie.TvdbId + ".jpg";
            await File.WriteAllBytesAsync(Path.Combine(path, thumbnailName), thumbnailBytes);
            await File.WriteAllBytesAsync(Path.Combine(path, imageName), imageBytes);

            // Save the movie to the database with the thumbnail and image paths
            movie.Thumbnail = thumbnailName;
            movie.ImageUrl = imageName;

            await _moviesRepository.AddMovie(movie);
        }

        public async Task<Movie?> GetMovie(int id)
        {
            return await _moviesRepository.GetByIdAsync(id);
        }

        public async Task<List<Movie>> GetMovies(string title, int amount)
        {
            var movies = await _moviesRepository.GetAllAsync();
            return movies.ToList();
        }
    }
}
