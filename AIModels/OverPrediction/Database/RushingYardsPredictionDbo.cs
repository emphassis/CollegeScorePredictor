namespace CollegeScorePredictor.AIModels.OverPrediction.Database
{
    public class RushingYardsPredictionDbo
    {
        public long RushingYardsPredictionId { get; set; }
        public long TeamOneId { get; set; }
        public double TeamOneRushingYards { get; set; }
        public double TeamOneRushingAttempts { get; set; }
        public double TeamOneYardsPerRushAttempt { get; set; }
        public long TeamTwoId { get; set; }
        public double TeamTwoOpponentRushingAttempts { get; set; }
        public double TeamTwoOpponentYardsPerRushAttempt { get; set; }
        public int Week { get; set; }
        public int Year { get; set; }
    }
}
