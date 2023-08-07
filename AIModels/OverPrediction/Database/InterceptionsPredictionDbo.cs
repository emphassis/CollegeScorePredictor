namespace CollegeScorePredictor.AIModels.OverPrediction.Database
{
    public class InterceptionsPredictionDbo
    {
        public long InterceptionsPredictionId { get; set; }
        public long TeamOneId { get; set; }
        public double TeamOneInterceptions { get; set; }
        public double TeamOnePastInterceptions { get; set; }
        public long TeamTwoId { get; set; }
        public double TeamTwoOpponentInterceptions { get; set; }
        public int Week { get; set; }
        public int Year { get; set; }
    }
}
