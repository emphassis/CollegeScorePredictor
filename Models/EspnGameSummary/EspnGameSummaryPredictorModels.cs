namespace CollegeScorePredictor.Models.EspnGameSummary
{
#pragma warning disable IDE1006 // Naming Styles
    public class Predictor
    {
        public AwayTeam awayTeam { get; set; } = new AwayTeam();
        public string? header { get; set; }
        public HomeTeam homeTeam { get; set; } = new HomeTeam();
    }

    public class AwayTeam
    {
        public string? gameProjection { get; set; } //chance to win
        public string? id { get; set; }
        public string? teamChanceLoss { get; set; } //chance to lose
    }
    public class HomeTeam
    {
        public string? gameProjection { get; set; } //chance to win
        public string? id { get; set; }
        public string? teamChanceLoss { get; set; } //chance to lose
    }
}
