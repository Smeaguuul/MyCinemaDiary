using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCinemaDiary.Application;
using MyCinemaDiary.Domain.Entities;

namespace MyCinemaDiary.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieSearchController : ControllerBase
    {
        private readonly Movies _movies;


        public MovieSearchController(Movies movies)
        {
            _movies = movies;
        }

        [Authorize] // Authorize so unauthorized user can't abuse the API call
        [HttpGet(Name = "SearchMovies")]
        public async Task<IEnumerable<Movie>> Get()
        {
            var title = HttpContext.Request.Query["query"].ToString();
            var limit = HttpContext.Request.Query["limit"].ToString();

            int resultLimit;
            if (!int.TryParse(limit, out resultLimit))
            {
                resultLimit = 2;
            }

            var movies = await _movies.SearchMovie(title, resultLimit);

            return movies;
        }
    }
}
