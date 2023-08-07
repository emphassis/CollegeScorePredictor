namespace CollegeScorePredictor.AIModels.OverPrediction.Database
{
    public class FirstDownPredictionDbo
    {
        public long FirstDownPredictionId { get; set; }
        public long TeamOneId { get; set; } //the current "selected" team
        public double TeamOneFirstDowns { get; set; } //their gamely readings
        public double TeamOnePastFirstDowns { get; set; } //their gamely readings
        public long TeamTwoId { get; set; } //the opponent
        public double TeamTwoFirstDownsAllowed { get; set; } // the first downs of all the opponents of the opponent 
        public int Week { get; set; }
        public int Year { get; set; }
    }
}
