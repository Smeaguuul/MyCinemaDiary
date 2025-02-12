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
            Movie movie = new Movie
            {
                Country = GetStringProperty(movieElement, "country"),
                Director = GetStringProperty(movieElement, "director"),
                ExtendedTitle = GetStringProperty(movieElement, "extended_title"),
                Name = GetStringProperty(movieElement, "name"),
                FirstAirTime = GetDateTimeProperty(movieElement, "first_air_time"),
                Overview = GetStringProperty(movieElement, "overview"),
                PrimaryLanguage = GetStringProperty(movieElement, "primary_language"),
                PrimaryType = GetStringProperty(movieElement, "primary_type"),
                Status = GetStringProperty(movieElement, "status"),
                Year = GetYearProperty(movieElement, "year"),
                Slug = GetStringProperty(movieElement, "slug"),
                ImageUrl = GetStringProperty(movieElement, "image_url"),
                Thumbnail = GetStringProperty(movieElement, "thumbnail"),
                TvdbId = GetStringProperty(movieElement, "tvdb_id"),
                ImdbId = GetRemoteId(movieElement, "IMDB"),
                TmdbId = GetRemoteId(movieElement, "TheMovieDB.com"),
                MovieGenres = GetGenres(movieElement)
            };

            return movie;
        }

        private static string GetStringProperty(JsonElement element, string propertyName)
        {
            return element.TryGetProperty(propertyName, out JsonElement propertyElement)
                ? propertyElement.GetString() ?? "Not found"
                : "Not found";
        }

        private static DateTime GetDateTimeProperty(JsonElement element, string propertyName)
        {
            return element.TryGetProperty(propertyName, out JsonElement propertyElement)
                ? propertyElement.GetDateTime().ToUniversalTime()
                : DateTime.MinValue;
        }

        private static int GetYearProperty(JsonElement element, string propertyName)
        {
            return element.TryGetProperty(propertyName, out JsonElement propertyElement) && propertyElement.GetString() != null
                ? int.Parse(propertyElement.GetString())
                : 0;
        }

        private static string GetRemoteId(JsonElement element, string sourceName)
        {
            string remoteId;
            try { 
            remoteId = element.TryGetProperty("remote_ids", out JsonElement remoteIdsElement)
                ? remoteIdsElement.EnumerateArray()
                    .FirstOrDefault(e => e.TryGetProperty("sourceName", out JsonElement sourceNameElement) && sourceNameElement.GetString() == sourceName)
                    .TryGetProperty("id", out JsonElement idElement)
                        ? idElement.GetString() ?? "Not found"
                        : "Not found"
                : "Not found";
            } catch (Exception e)
            {
                remoteId = "Could not find " + sourceName;
            }
            return remoteId;
        }

        private static List<string> GetGenres(JsonElement element)
        {
            return element.TryGetProperty("genres", out JsonElement genresElement)
                ? genresElement.EnumerateArray().Select(e => e.ToString()).ToList()
                : new List<string>();
        }
    }
}
