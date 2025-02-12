﻿using Microsoft.AspNetCore.Mvc;
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

        [HttpGet(Name = "SearchEntries")]
        public async Task<IEnumerable<DiaryEntry>> Get()
        {
            var movieIdQuery = HttpContext.Request.Query["movieId"].ToString();
            var userIdQuery = HttpContext.Request.Query["userId"].ToString();

            var searchByMovie = int.TryParse(movieIdQuery, out var movieId);
            var searchByUser = int.TryParse(userIdQuery, out var userId);
            IEnumerable<DiaryEntry> entries = [];
            if (searchByMovie && searchByUser)
            {
                entries = await _diaryEntries.GetByMovieAndUserId(movieId, userId);
            }
            else if (searchByUser)
            {
                entries = await _diaryEntries.GetByUserId(userId);
            }
            else if (searchByMovie)
            {
                entries = await _diaryEntries.GetByMovieId(movieId);
            }

            return entries;
        }

        [HttpPost(Name = "Save Diaryentry")]
        public async Task<IActionResult> Post([FromBody] DiaryEntryModel diaryEntryModel)
        {
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

            await _diaryEntries.AddDiaryEntry(diaryEntry, diaryEntryModel.MovieId, diaryEntryModel.UserId);
            return CreatedAtAction(nameof(Post), new { id = diaryEntry.Id }, diaryEntry);
        }
    }
}
