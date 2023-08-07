namespace CollegeScorePredictor.Models.Database
{
    public class BetHistoryDbo
    {
        public long BetId { get; set; }
        public bool Won { get; set; }
        public int Week { get; set; }
        public int Year { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
