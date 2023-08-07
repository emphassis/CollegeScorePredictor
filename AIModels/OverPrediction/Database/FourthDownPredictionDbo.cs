namespace CollegeScorePredictor.AIModels.OverPrediction.Database
{
    public class FourthDownPredictionDbo
    {
        public long FourthDownPredictionId { get; set; }
        public long TeamOneId { get; set; } //the current "selected" team
        public double TeamOneFourthDownCompletions { get; set; } //their gamely readings
        public double TeamOneFourthDownAttempts { get; set; } //their gamely readings
        public double TeamOnePastFourthDownCompletions { get; set; } //their gamely readings
        public double TeamOnePastFourthDownAttempts { get; set; } //their gamely readings
        public long TeamTwoId { get; set; } //the opponent
        public double TeamTwoFourthDownsCompletedAllowed { get; set; } // the fourth downs of all the opponents of the opponent 
        public double TeamTwoFourthDownsAttemptedAllowed { get; set; } // the fourth downs of all the opponents of the opponent
        public int Week { get; set; }
        public int Year { get; set; }
    }
}
