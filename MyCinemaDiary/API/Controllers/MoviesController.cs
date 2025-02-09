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
            var movies = await _movies.SearchMovie("Lord of the rings", 2);

            return movies;

            //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            //    TemperatureC = Random.Shared.Next(-20, 55),
            //    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            //})
            //.ToArray();
        }

    }
}
