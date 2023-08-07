namespace CollegeScorePredictor.Models.Database
{
    public class ScorePredictionDbo
    {
        public long ScorePredictionId { get; set; }
        public long GamePredictionId { get; set; }
        public string ModelName { get; set; } = string.Empty;
        public int HomeTeamScore { get; set; }
        public int AwayTeamScore { get; set; }
        public double AwayTeamWins { get; set; }
        public double HomeTeamWins { get; set; }
    }
}
