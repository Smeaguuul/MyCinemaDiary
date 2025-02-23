using Microsoft.AspNetCore.Authorization;
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


        public MoviesController(Movies movies)
        {
            _movies = movies;
        }

        [HttpGet(Name = "GetMovies")]
        public async Task<IEnumerable<Movie>> Get()
        {
            var title = HttpContext.Request.Query["query"].ToString();
            var limit = HttpContext.Request.Query["limit"].ToString();

            int resultLimit;
            if (!int.TryParse(limit, out resultLimit))
            {
                resultLimit = 2;
            }

            var movies = await _movies.GetMovies(title, resultLimit);

            return movies;
        }

        [HttpGet("movie")]
        public async Task<IActionResult> GetById()
        {
            var id = HttpContext.Request.Query["id"].ToString();
            if (!int.TryParse(id, out int movieId))
            {
                return BadRequest("MovieId not provided.");
            }

            var movie = await _movies.GetMovie(movieId);
            return Ok(movie);
        }

        [HttpPost(Name = "Save Movie")]
        public async Task<IActionResult> Post(Movie movie)
        {
            // Save the movie to the database
            movie.FirstAirTime = movie.FirstAirTime.ToUniversalTime();
            await _movies.SaveMovie(movie);
            return Created();
        }

        [HttpGet("latest")]
        public async Task<IEnumerable<Movie>> GetLatestMovies()
        {
            var count = HttpContext.Request.Query["count"].ToString();
            int movieCount;
            if (!int.TryParse(count, out movieCount))
            {
                movieCount = 2;
            }
            var movies = await _movies.GetLatestMovies(movieCount);
            return movies;
        }
    }
}
