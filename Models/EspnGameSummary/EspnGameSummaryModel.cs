namespace CollegeScorePredictor.Models.EspnGameSummary
{
#pragma warning disable IDE1006 // Naming Styles
    public class EspnGameSummaryModel
    {
        public object? againstTheSpread { get; set; }
        public object? article { get; set; }
        public Boxscore boxscore { get; set; } = new Boxscore();
        public object? broadcasts { get; set; }
        public Drives drives { get; set; } = new Drives();
        public object? format { get; set; }
        public object? gameInfo { get; set; }
        public Header header { get; set; } = new Header();
        public object? leaders { get; set; }
        public object? news { get; set; }
        public object? odds { get; set; }
        public object? pickcenter { get; set; }
        public Predictor predictor { get; set; } = new Predictor();
        public List<ScoringPlays> scoringPlays { get; set; } = new List<ScoringPlays>();
        public object? standings { get; set; }
        public object? videos { get; set; }
        public List<WinProbability> winprobability { get; set; } = new List<WinProbability>();
    }
}
