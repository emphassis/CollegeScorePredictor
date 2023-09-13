using static CollegeScorePredictor.Constants.Enums;

namespace CollegeScorePredictor.Models.Bet
{
    public class BetSlip
    {
        public string Bet { get; set; } = string.Empty;
        public string BetName { get; set; } = string.Empty;
        public string AwayTeamName { get; set; } = string.Empty;
        public string HomeTeamName { get; set; } = string.Empty;
    }
}
