namespace CollegeScorePredictor.Models.Win
{
    public class YearlyWinResponseModel
    {
        public int PredictedLessThanOne { get; set; }
        public int PredictedLessThanTwo { get; set; }
        public int PredictedLessThanThree { get; set; }
        public List<TeamWinsModel> TeamWins { get; set; } = new List<TeamWinsModel>();
    }
    public class TeamWinsModel
    {
        public long TeamId { get; set; }
        public string TeamName { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int PredictedWins { get; set; }
        public int PredictedLosses { get; set; }
        public int PredictedTossups { get; set; }
    }
}
