namespace MyCinemaDiary.Domain.Entities
{
    public class DiaryEntry
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Review { get; set; }
        public int Rating { get; set; }
        public DateTime Date { get; set; }
        public Movie? Movie { get; set; }
        public User? User { get; set; }
    }
}
