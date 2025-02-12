namespace MyCinemaDiary.API.Models
{
    public class DiaryEntryModel
    {
        public int Rating { get; set; }
        public string Review { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public int MovieId { get; set; }
    }

}
