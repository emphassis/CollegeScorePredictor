namespace CollegeScorePredictor.Models.Grade
{
    public class GetGradeModel
    {
        public double TeamTotalYards { get; set; }
        public double OpponentTotalYardsAllowed { get; set; }
        public double TotalWins { get; set; }
        public double OpponentTotalWins { get; set; }
        public int TeamConference { get; set; }
        public int OpponentTeamConference { get; set; }

        //TODO:
        public double TeamPoints { get; set; }
        public double TeamTurnoverMargin { get; set; }
        public double TeamPenalties { get; set; }
    }
}


/*
 * 
        [TeamConference] INT NOT NULL,
        [TeamChanceToWin] FLOAT NULL,
        [TeamFirstDowns] FLOAT NOT NULL,
        [TeamTotalYards] FLOAT NOT NULL,
        [TeamNetPassingYards] FLOAT NOT NULL,
        [TeamYardsPerPass] FLOAT NOT NULL,
        [TeamPassingAttempts] FLOAT NOT NULL,
        [TeamRushingYards] FLOAT NOT NULL,
        [TeamRushingAttempts] FLOAT NOT NULL,
        [TeamYardsPerRushAttempt] FLOAT NOT NULL,
        [TeamTotalPenalties] FLOAT NOT NULL,
        [TeamTotalPenaltyYards] FLOAT NOT NULL,
        [TeamFumblesLost] FLOAT NOT NULL,
        [TeamInterceptions] FLOAT NOT NULL,
        [TeamPossessionTime] FLOAT NOT NULL,
        [TeamPositivePlays] FLOAT NOT NULL,
        [TeamNegativePlays] FLOAT NOT NULL,
        [TeamDefensiveTacklesForLoss] FLOAT NOT NULL,
        [TeamDefensiveSacks] FLOAT NOT NULL,
        [TeamFieldGoals] FLOAT NOT NULL,
        [TeamDefensiveSafety] FLOAT NOT NULL,
        [TeamSpecialTeamsPoints] FLOAT NOT NULL,
        [TeamPointsPerPlay] FLOAT NOT NULL,
        [TeamTurnoverMargin] FLOAT NOT NULL,
        
        [OpponentConference] INT NOT NULL,
        [OpponentScore] FLOAT NOT NULL,
        [OpponentFirstDowns] FLOAT NOT NULL,
        [OpponentTotalYardsAllowed] FLOAT NOT NULL,
        [OpponentPassingYardsAllowed] FLOAT NOT NULL,
        [OpponentRushingYardsAllowed] FLOAT NOT NULL,
        [OpponentYardsPerPassAllowed] FLOAT NOT NULL,
        [OpponentYardsPerRushAttemptAllowed] FLOAT NOT NULL,
        [OpponentTotalPenalties] FLOAT NOT NULL,
        [OpponentTotalPenaltyYards] FLOAT NOT NULL,
        [OpponentFumblesForced] FLOAT NOT NULL,
        [OpponentInterceptionsForced] FLOAT NOT NULL,
        [OpponentPossessionTime] FLOAT NOT NULL,
        [OpponentPositivePlays] FLOAT NOT NULL,
        [OpponentNegativePlays] FLOAT NOT NULL,
        [OpponentDefensiveTacklesForLoss] FLOAT NOT NULL,
        [OpponentDefensiveSacks] FLOAT NOT NULL,
        [OpponentFieldGoalsAllowed] FLOAT NOT NULL,
        [OpponentDefensiveSafety] FLOAT NOT NULL,
        [OpponentSpecialTeamsPointsAllowed] FLOAT NOT NULL,
        [OpponentPointsPerPlay] FLOAT NOT NULL,
        [OpponentTurnoverMargin] FLOAT NOT NULL,

        --#region GradeModel Specific
        [Grade] FLOAT NULL,
        [WeightedGrade] FLOAT NULL,
        [TeamOffenseUpswing] BIT NOT NULL,
        [TeamDefenseUpswing] BIT NOT NULL,
        [TeamTurnoverUpswing] BIT NOT NULL,
        [TeamGradeUpswing] BIT NOT NULL,
        [TeamPasserRating] FLOAT NULL,
        [TeamRusherRating] FLOAT NULL,
        [TeamReceiverRating] FLOAT NULL,
        
        [OpponentGrade] FLOAT NULL,
        [OpponentWeightedGrade] FLOAT NULL,
        [OpponentOffenseUpswing] BIT NOT NULL,
        [OpponentDefenseUpswing] BIT NOT NULL,
        [OpponentTurnoverUpswing] BIT NOT NULL,
        [OpponentGradeUpswing] BIT NOT NULL,
        [OpponentPasserRating] FLOAT NULL,
        [OpponentRusherRating] FLOAT NULL,
        [OpponentReceiverRating] FLOAT NULL,
        --#endregion
	
 */