namespace CollegeScorePredictor.AIModels.OverPrediction
{
    public class PredictGameRequestModel
    {
        public long HomeTeamId { get; set; }
        public long AwayTeamId { get; set; }
        public int Week { get; set; }
        public int Year { get; set; }
        public bool IsConference { get; set; }
        public bool IsNeutralSite { get; set; }
    }
}
