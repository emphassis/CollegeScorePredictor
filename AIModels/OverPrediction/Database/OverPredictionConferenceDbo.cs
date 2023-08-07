namespace CollegeScorePredictor.AIModels.OverPrediction.Database
{
    public class OverPredictionConferenceDbo
    {
        public long OverPredictionId { get; set; }
        public long EventId { get; set; }
        public bool IsConference { get; set; }
        public bool IsNeutralSite { get; set; }
        public int Week { get; set; }
        public int Year { get; set; }


        public long TeamId { get; set; }
        public double TeamScore { get; set; }
        public bool TeamWon { get; set; }
        public int TeamWins { get; set; }
        public int TeamLosses { get; set; }
        public int TeamConference { get; set; }
        public double TeamFirstDowns { get; set; } //predict
        public double TeamThirdDownConversions { get; set; } //predict
        public double TeamFourthDownConversions { get; set; } //predict
        public double TeamTotalYards { get; set; }//predict
        public double TeamNetPassingYards { get; set; }//predict
        public double TeamRushingYards { get; set; }//predict
        public double TeamTotalPenalties { get; set; }//averaged
        public double TeamTotalPenaltyYards { get; set; }//averaged
        public double TeamFumblesLost { get; set; }//predict
        public double TeamInterceptions { get; set; }//predict
        public double TeamPossessionTime { get; set; }//average
        public double TeamPositivePlays { get; set; }//average
        public double TeamNegativePlays { get; set; }//average


        public long OpponentId { get; set; }
        public int OpponentWins { get; set; }
        public int OpponentLosses { get; set; }
        public int OpponentConference { get; set; }
        public double OpponentAverageFirstDownsAllowed { get; set; }
        public double OpponentAverageThirdDownConversionsAllowed { get; set; }
        public double OpponentAverageFourthDownConversionsAllowed { get; set; }
        public double OpponentAverageTotalYardsAllowed { get; set; }
        public double OpponentAveragePassingYardsAllowed { get; set; }
        public double OpponentAverageRushingYardsAllowed { get; set; }
        public double OpponentAverageTotalPenalties { get; set; }
        public double OpponentAverageTotalPenaltyYards { get; set; }
        public double OpponentAverageFumblesForced { get; set; }
        public double OpponentAverageInterceptionsForced { get; set; }
        public double OpponentAveragePossessionTime { get; set; }
        public double OpponentAveragePositivePlays { get; set; }
        public double OpponentAverageNegativePlays { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
