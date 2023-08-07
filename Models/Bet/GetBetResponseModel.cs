namespace CollegeScorePredictor.Models.Bet
{
    public class GetBetResponseModel
    {
        public long BetId { get; set; }
        public bool Bet { get; set; }
        public string HomeTeamName { get; set; } = string.Empty;
        public string AwayTeamName { get; set; } = string.Empty;
        public string BetTypeName { get; set; } = string.Empty;
        public double Odd { get; set; }
        public int Variance { get; set; }
    }
}
