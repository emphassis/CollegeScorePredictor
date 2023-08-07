namespace CollegeScorePredictor.AIModels.OverPrediction.Database
{
    public class FumblesPredictionDbo
    {
        public long FumblesPredictionId { get; set; }
        public long TeamOneId { get; set; }
        public double TeamOneFumblesLost { get; set; }
        public double TeamOnePastFumblesLost { get; set; }
        public long TeamTwoId { get; set; }
        public double TeamTwoOpponentFumblesLost { get; set; }
        public int Week { get; set; }
        public int Year { get; set; }
    }
}
