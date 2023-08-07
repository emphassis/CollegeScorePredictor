namespace CollegeScorePredictor.AIModels.OverPrediction.Database
{
    public class ThirdDownPredictionDbo
    {
        public long ThirdDownPredictionId { get; set; }
        public long TeamOneId { get; set; } //the current "selected" team
        public double TeamOneThirdDownCompletions { get; set; } //their gamely readings
        public double TeamOneThirdDownAttempts { get; set; } //their gamely readings
        public double TeamOnePastThirdDownCompletions { get; set; } //their gamely readings
        public double TeamOnePastThirdDownAttempts { get; set; } //their gamely readings
        public long TeamTwoId { get; set; } //the opponent
        public double TeamTwoThirdDownsCompletedAllowed { get; set; } // the third downs of all the opponents of the opponent 
        public double TeamTwoThirdDownsAttemptedAllowed { get; set; } // the third downs of all the opponents of the opponent 
        public int Week { get; set; }
        public int Year { get; set; }
    }
}
