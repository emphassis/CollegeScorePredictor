namespace CollegeScorePredictor.Models.Database
{
    public class ScorePredictionConferenceDbo
    {
        public long ScorePredictionConferenceId { get; set; }
        public long GamePredictionConferenceId { get; set; }
        public string ModelName { get; set; } = string.Empty;
        public int HomeTeamScore { get; set; }
        public int AwayTeamScore { get; set; }
    }
}
