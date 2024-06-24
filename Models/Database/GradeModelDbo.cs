namespace CollegeScorePredictor.AIModels.OffensivePrediction.Models
{
    public class GradeModelDbo
    {
        public long GradeModelId { get; set; }
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
        public float TeamChanceToWin { get; set; }
        public float TeamFirstDowns { get; set; } //
        public float TeamTotalYards { get; set; }//
        public float TeamNetPassingYards { get; set; }//
        public float TeamYardsPerPass { get; set; }
        public float TeamPassingAttempts { get; set; }//
        public float TeamRushingYards { get; set; }//
        public float TeamRushingAttempts { get; set; }//
        public float TeamYardsPerRushAttempt { get; set; }
        public float TeamTotalPenalties { get; set; }//
        public float TeamTotalPenaltyYards { get; set; }//
        public float TeamFumblesLost { get; set; }//
        public float TeamInterceptions { get; set; }//
        public float TeamPossessionTime { get; set; }//
        public float TeamPositivePlays { get; set; }//
        public float TeamNegativePlays { get; set; }//
        public float TeamDefensiveTacklesForLoss { get; set; }
        public float TeamDefensiveSacks { get; set; }
        public float TeamFieldGoals { get; set; }
        public float TeamDefensiveSafety { get; set; }
        public float TeamSpecialTeamsPoints { get; set; }
        public float TeamPointsPerPlay { get; set; }
        public float TeamTurnoverMargin { get; set; }


        public long OpponentTeamId { get; set; }
        public int OpponentWins { get; set; }
        public int OpponentLosses { get; set; }
        public int OpponentConference { get; set; }
        public double OpponentScore { get; set; }
        public double OpponentFirstDownsAllowed { get; set; }
        public double OpponentTotalYardsAllowed { get; set; }
        public double OpponentPassingYardsAllowed { get; set; }
        public double OpponentRushingYardsAllowed { get; set; }
        public double OpponentYardsPerPassAllowed { get; set; }
        public double OpponentYardsPerRushAttemptAllowed { get; set; }
        public double OpponentTotalPenalties { get; set; }
        public double OpponentTotalPenaltyYards { get; set; }
        public double OpponentFumblesForced { get; set; }
        public double OpponentInterceptionsForced { get; set; }
        public double OpponentPossessionTime { get; set; }
        public double OpponentPositivePlays { get; set; }
        public double OpponentNegativePlays { get; set; }
        public double OpponentDefensiveTacklesForLoss { get; set; }
        public double OpponentDefensiveSacks { get; set; }
        public double OpponentFieldGoalsAllowed { get; set; }
        public double OpponentDefensiveSafety { get; set; }
        public double OpponentSpecialTeamsPointsAllowed { get; set; }
        public double OpponentPointsPerPlay { get; set; }
        public double OpponentTurnoverMargin { get; set; }


        public double? Grade { get; set; }
        public double? WeightedGrade { get; set; }
        public bool TeamOffenseUpswing { get; set; }
        public bool TeamDefenseUpswing { get; set; }
        public bool TeamTurnoverUpswing { get; set; }
        public bool TeamGradeUpswing { get; set; }
        public double TeamPasserRating { get; set; }
        public double TeamRusherRating { get; set; }
        public double TeamReceiverRating { get; set; }

        public double? OpponentGrade { get; set; }
        public double? OpponentWeightedGrade { get; set; }
        public bool OpponentTeamOffenseUpswing { get; set; }
        public bool OpponentTeamDefenseUpswing { get; set; }
        public bool OpponentTeamTurnoverUpswing { get; set; }
        public bool OpponentTeamGradeUpswing { get; set; }
        public double OpponentTeamPasserRating { get; set; }
        public double OpponentTeamRusherRating { get; set; }
        public double OpponentTeamReceiverRating { get; set; }



        public DateTime CreatedDate { get; set; }
    }
}
