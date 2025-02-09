using Microsoft.AspNetCore.Mvc;
using MyCinemaDiary.Application;
using MyCinemaDiary.Domain.Entities;

namespace MyCinemaDiary.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly Movies _movies;

        private readonly ILogger<MoviesController> _logger;

        public MoviesController(ILogger<MoviesController> logger, Movies movies)
        {
            _logger = logger;
            _movies = movies;
        }

        [HttpGet(Name = "GetMovies")]
        public async Task<IEnumerable<Movie>> Get()
        {
            // Accessing query strings directly
            var title = HttpContext.Request.Query["query"].ToString();
            var limit = HttpContext.Request.Query["limit"].ToString();

            // You can convert limit to an integer if needed
            int resultLimit;
            if (!int.TryParse(limit, out resultLimit))
            {
                resultLimit = 2; // Default value if parsing fails
            }

            var movies = await _movies.SearchMovie(title, resultLimit);

            return movies;
        }

    }
}
