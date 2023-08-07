namespace CollegeScorePredictor.Models.Database
{
    public class GamePredictionConferenceDbo
    {
        public long GamePredictionConferenceId { get; set; }
        public long HomeTeamId { get; set; }
        public long AwayTeamId { get; set; }
        public string HomeTeamName { get; set; } = string.Empty;
        public string AwayTeamName { get; set; } = string.Empty;
        public int Week { get; set; }
        public int Year { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
