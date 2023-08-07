namespace CollegeScorePredictor.Models.EspnGameSummary
{
#pragma warning disable IDE1006 // Naming Styles
    public class WinProbability
    {
        public decimal? homeWinPercentage { get; set; }
        public string? playId { get; set; }
        public int secondsLeft { get; set; }
        public decimal? tiePercentage { get; set; }
    }
}
