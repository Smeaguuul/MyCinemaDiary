﻿using Microsoft.AspNetCore.Authorization;
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
            //var movies = await _movies.SearchMovie(title, resultLimit);

            return movies;
        }

        [HttpPost (Name = "Save Movie")]
        public async Task<IActionResult> Post(Movie movie)
        {
            // Save the movie to the database
            movie.FirstAirTime = movie.FirstAirTime.ToUniversalTime();
            await _movies.SaveMovie(movie);
            return Created();
        }
    }
}
