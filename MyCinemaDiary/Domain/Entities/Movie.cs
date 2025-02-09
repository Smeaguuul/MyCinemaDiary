using System.Text.Json;

namespace MyCinemaDiary.Domain.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string Director { get; set; }
        public string ExtendedTitle { get; set; }
        public string Name { get; set; }
        public DateTime FirstAirTime { get; set; }
        public string Overview { get; set; }
        public string PrimaryLanguage { get; set; }
        public string PrimaryType { get; set; }
        public string Status { get; set; }
        public int Year { get; set; }
        public string Slug { get; set; }
        public string ImageUrl { get; set; }
        public string Thumbnail { get; set; }
        public string TvdbId { get; set; }
        public string ImdbId { get; set; }
        public string TmdbId { get; set; }
        public List<string> MovieGenres { get; set; }

        public static Movie Parse(JsonElement movieElement)
        {
            var movie = new Movie
            {
                Country = movieElement.GetProperty("country").GetString(),
                Director = movieElement.GetProperty("director").GetString(),
                ExtendedTitle = movieElement.GetProperty("extended_title").GetString(),
                Name = movieElement.GetProperty("name").GetString(),
                FirstAirTime = movieElement.GetProperty("first_air_time").GetDateTime().ToUniversalTime(),
                Overview = movieElement.GetProperty("overview").GetString(),
                PrimaryLanguage = movieElement.GetProperty("primary_language").GetString(),
                PrimaryType = movieElement.GetProperty("primary_type").GetString(),
                Status = movieElement.GetProperty("status").GetString(),
                Year = int.Parse(movieElement.GetProperty("year").GetString()),
                Slug = movieElement.GetProperty("slug").GetString(),
                ImageUrl = movieElement.GetProperty("image_url").GetString(),
                Thumbnail = movieElement.GetProperty("thumbnail").GetString(),
                TvdbId = movieElement.GetProperty("tvdb_id").GetString(),
                ImdbId = movieElement.GetProperty("remote_ids").EnumerateArray().Where(element => element.GetProperty("sourceName").ToString().Equals("IMDB")).First().GetProperty("id").GetString(),
                TmdbId = movieElement.GetProperty("remote_ids").EnumerateArray().Where(element => element.GetProperty("sourceName").ToString().Equals("TheMovieDB.com")).First().GetProperty("id").GetString(),
                MovieGenres = movieElement.GetProperty("genres").EnumerateArray().Select(element => element.ToString()).ToList(),
            };

            return movie;
        }
    }
}
