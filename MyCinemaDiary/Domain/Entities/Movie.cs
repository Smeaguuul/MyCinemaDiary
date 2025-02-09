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
    }
}
