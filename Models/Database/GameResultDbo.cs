namespace CollegeScorePredictor.Models.Database
{
    public class GameResultDbo
    {
        public long GameResultId { get; set; }
        public long EventId { get; set; }
        public bool IsConference { get; set; }
        public bool IsNeutralSite { get; set; }
        public int Week { get; set; }
        public int Year { get; set; }


        public long HomeTeamId { get; set; }
        public string HomeTeamName { get; set; } = string.Empty;
        public int HomeTeamConference { get; set; }
        public double HomeTeamScore { get; set; }
        public double? HomeTeamChanceToWin { get; set; }
        public bool HomeTeamWon { get; set; }
        public double HomeTeamFirstDowns { get; set; } //predict based on opponent allowed first downs
        public double HomeTeamThirdDownCompletions { get; set; } //predict based on opponent allowed third downs
        public double HomeTeamThirdDownAttempts { get; set; }//
        public double HomeTeamFourthDownCompletions { get; set; } //predict based on opponent allowed fourth downs
        public double HomeTeamFourthDownAttempts { get; set; }//
        public double HomeTeamTotalYards { get; set; }//predict based on predicted values
        public double HomeTeamNetPassingYards { get; set; }//predict based on prior attempts/completions and opponents attempts/completions allowed
        public double HomeTeamPassingAttempts { get; set; }//averaged
        public double HomeTeamPassingCompletions { get; set; }//averaged
        public double HomeTeamYardsPerPass { get; set; }//predict based on passing model
        public double HomeTeamRushingYards { get; set; }//predict based on teams prior rushing attempts and yards per rush attempts and opponents allowed for those
        public double HomeTeamRushingAttempts { get; set; } //averaged
        public double HomeTeamYardsPerRushAttempt { get; set; } //averaged
        public double HomeTeamTotalPenalties { get; set; }//averaged
        public double HomeTeamTotalPenaltyYards { get; set; }//averaged
        public double HomeTeamFumblesLost { get; set; }//predict base on teams fumbles lost and opponents forced fumbles 
        public double HomeTeamInterceptions { get; set; }//predict based on teams interceptions and opponents force dinterceptions
        public double HomeTeamPossessionTime { get; set; }//average
        public double? HomeTeamWinProbabilityBeginningOfGame { get; set; }
        public double? HomeTeamWinProbabilityEndOfGame { get; set; }
        public double HomeTeamPositivePlays { get; set; }
        public double HomeTeamNegativePlays { get; set; }
        public double HomeTeamNeutralPlays { get; set; }
        public double HomeTeamDefensiveTacklesForLoss { get; set; }
        public double HomeTeamDefensiveSacks { get; set; }
        public double HomeTeamFieldGoals { get; set; }
        public double HomeTeamDefensiveSafety { get; set; }
        public double HomeTeamSpecialTeamsPoints { get; set; }


        public long AwayTeamId { get; set; }//boxscore/teams[0]/team/id
        public string AwayTeamName { get; set; } = string.Empty;//boxscore/teams[0]/team/displayName
        public int AwayTeamConference { get; set; }
        public double AwayTeamScore { get; set; } //scoringplays[last]/awayScore
        public double? AwayTeamChanceToWin { get; set; }
        public bool AwayTeamWon { get; set; }
        public double AwayTeamFirstDowns { get; set; }//boxscore/teams[0]/statistics[0]/displayValue
        public double AwayTeamThirdDownCompletions { get; set; }
        public double AwayTeamThirdDownAttempts { get; set; }
        public double AwayTeamFourthDownCompletions { get; set; }
        public double AwayTeamFourthDownAttempts { get; set; }
        public double AwayTeamTotalYards { get; set; }
        public double AwayTeamNetPassingYards { get; set; }
        public double AwayTeamPassingAttempts { get; set; }
        public double AwayTeamPassingCompletions { get; set; }
        public double AwayTeamYardsPerPass { get; set; }
        public double AwayTeamRushingYards { get; set; }
        public double AwayTeamRushingAttempts { get; set; }
        public double AwayTeamYardsPerRushAttempt { get; set; }
        public double AwayTeamTotalPenalties { get; set; }
        public double AwayTeamTotalPenaltyYards { get; set; }
        public double AwayTeamFumblesLost { get; set; }
        public double AwayTeamInterceptions { get; set; }
        public double AwayTeamPossessionTime { get; set; }
        public double? AwayTeamWinProbabilityBeginningOfGame { get; set; }
        public double? AwayTeamWinProbabilityEndOfGame { get; set; }
        public double AwayTeamPositivePlays { get; set; }
        public double AwayTeamNegativePlays { get; set; }
        public double AwayTeamNeutralPlays { get; set; }
        public double AwayTeamDefensiveTacklesForLoss { get; set; }
        public double AwayTeamDefensiveSacks { get; set; }
        public double AwayTeamFieldGoals { get; set; }
        public double AwayTeamDefensiveSafety { get; set; }
        public double AwayTeamSpecialTeamsPoints { get; set; }
    }
}
