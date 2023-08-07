namespace CollegeScorePredictor.Models.Bet
{
    public class GetBetHistoryResponseModel
    {
        public int BetsWon { get; set; }
        public int BetsLost { get; set; }
        public double Profit { get; set; }
        public List<BetHistoryModel> BetHistory { get; set; } = new List<BetHistoryModel>();
    }

    public class BetHistoryModel
    {
        public long BetId { get; set; }
        public bool Won { get; set; }
        public string AwayTeamName { get; set; } = string.Empty;
        public string HomeTeamName { get; set; } = string.Empty;
        public string BetTypeName { get; set; } = string.Empty;
        public int Variance { get; set; }
        public double Odd { get; set; }
    }
}
