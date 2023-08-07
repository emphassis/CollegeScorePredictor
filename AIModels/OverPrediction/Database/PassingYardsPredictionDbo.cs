namespace CollegeScorePredictor.AIModels.OverPrediction.Database
{
    public class PassingYardsPredictionDbo
    {
        public long PassingYardsPredictionId { get; set; }
        public long TeamOneId { get; set; }
        public double TeamOneNetPassingYards { get; set; }
        public double TeamOnePassingAttempts { get; set; }
        public double TeamOnePassingCompletions { get; set; }
        public long TeamTwoId { get; set; }
        public double TeamTwoOpponentPassingAttempts { get; set; }
        public double TeamTwoOpponentPassingCompletions { get; set; }
        public int Week { get; set; }
        public int Year { get; set; }
    }
}
