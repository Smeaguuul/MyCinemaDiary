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
        public async Task<IEnumerable<DiaryEntry>?> Get()
        {
            var userId = int.Parse(User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value);

            if (userId == 0)
            {
                return null;
            }

            return await _diaryEntries.GetByUserId(userId);
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
