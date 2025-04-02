using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCinemaDiary.API.Models;
using MyCinemaDiary.Application;
using MyCinemaDiary.Domain.Entities;

namespace MyCinemaDiary.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DiaryEntriesController : ControllerBase
    {
        private readonly DiaryEntries _diaryEntries;


        public DiaryEntriesController(DiaryEntries diaryEntries)
        {
            _diaryEntries = diaryEntries;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var movieId = HttpContext.Request.Query["movieId"].ToString();
            var userId = HttpContext.Request.Query["userId"].ToString();

            IEnumerable<DiaryEntry?> diaryEntries;
            // TODO Move logic lower
            // Either you get the user from the token or from the query.
            // If userId is not provided in the query, the user just gets their own diary entries.
            if (movieId != "" && userId != "")
            {
                if (!int.TryParse(movieId, out int movieIdInt)) return BadRequest("MovieId is not a number");
                if (!int.TryParse(userId, out int userIdInt)) return BadRequest("UserId is not a number");
                diaryEntries = await _diaryEntries.GetByMovieAndUserId(movieIdInt, userIdInt);
            }
            else if (userId != "")
            {
                if (!int.TryParse(userId, out int userIdInt)) return BadRequest("UserId is not a number");
                diaryEntries = await _diaryEntries.GetByUserId(userIdInt);
            }
            else if (movieId != "")
            {
                if (!int.TryParse(movieId, out int movieIdInt)) return BadRequest("MovieId is not a number");
                diaryEntries = await _diaryEntries.GetByMovieId(movieIdInt);
            }
            else
            {
                if (!int.TryParse(User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value, out int currentUserId)) return BadRequest("UserId is not valid.");

                diaryEntries = await _diaryEntries.GetByUserId(currentUserId);
            }

            return Ok(diaryEntries); //Can return empty list
        }

        [Authorize]
        [HttpPost("Save")]
        public async Task<IActionResult> Post([FromBody] DiaryEntryModel diaryEntryModel)
        {
            var userId = int.Parse(User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value);
            if (diaryEntryModel == null)
            {
                return BadRequest("Diary entry model is null.");
            }

            var diaryEntry = new DiaryEntry
            {
                Rating = diaryEntryModel.Rating,
                Review = diaryEntryModel.Review,
                Title = diaryEntryModel.Title,
                Date = diaryEntryModel.Date.ToUniversalTime(),
            };

            await _diaryEntries.AddDiaryEntry(diaryEntry, diaryEntryModel.MovieId, userId);
            return CreatedAtAction(nameof(Post), new { id = diaryEntry.Id }, diaryEntry);
        }
    }
}
