namespace CollegeScorePredictor.AIModels.OverPrediction
{
    public class PredictGameResponseModel
    {
        public long HomeTeamId { get; set; }
        public long AwayTeamId { get; set; }
        public string HomeTeamName { get; set; } = string.Empty;
        public string AwayTeamName { get; set; } = string.Empty;
        public List<ScorePrediction> ScorePredictions { get; set; } = new List<ScorePrediction>();
        public int Week { get; set; }
        public int Year { get; set; }
    }

    public class ScorePrediction
    {
        public string ModelName { get; set; } = string.Empty;
        public double HomeTeamScore { get; set; }
        public double AwayTeamScore { get; set; }
        public double OverUnderScore { get; set; }
        public bool AwayTeamWins { get; set; }
        public bool HomeTeamWins { get; set; }
    }
}
