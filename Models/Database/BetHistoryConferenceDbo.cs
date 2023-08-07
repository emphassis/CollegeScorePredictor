namespace CollegeScorePredictor.Models.Database
{
    public class BetHistoryConferenceDbo
    {
        public long BetConferenceId { get; set; }
        public bool Won { get; set; }
        public int Week { get; set; }
        public int Year { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
