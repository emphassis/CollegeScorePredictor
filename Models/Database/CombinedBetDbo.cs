namespace CollegeScorePredictor.Models.Database
{
    public class CombinedBetDbo
    {
        public long CombinedBetId { get; set; }
        public long GamePredictionId { get; set; }
        public long HomeTeamId { get; set; }
        public long AwayTeamId { get; set; }
        public bool Bet { get; set; }
        public double Odd { get; set; }
        public bool PublicBetOver { get; set; }
        public bool PublicBetUnder { get; set; }
        public bool PublicBetHomeMoneyline { get; set; }
        public bool PublicBetAwayMoneyline { get; set; }
        public bool PublicBetHomeSpread { get; set; }
        public bool PublicBetAwaySpread { get; set; }
        public int BetType { get; set; }
        public int Variance { get; set; }
        public int Week { get; set; }
        public int Year { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
