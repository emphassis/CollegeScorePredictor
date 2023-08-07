namespace CollegeScorePredictor.Models.Record
{
    public class GetGameDrivesModel
    {
        public int HomeTeamPositivePlays { get; set; }
        public int HomeTeamNegativePlays { get; set; }
        public int HomeTeamNeutralPlays { get; set; }
        public int AwayTeamPositivePlays { get; set; }
        public int AwayTeamNegativePlays { get; set; }
        public int AwayTeamNeutralPlays { get; set; }

        public int HomeTeamDefensiveTacklesForLoss { get; set; }
        public int HomeTeamDefensiveSacks { get; set; }
        public int HomeTeamFieldGoals { get; set; }
        public int HomeTeamDefensiveSafety { get; set; }
        public int HomeTeamSpecialTeamsPoints { get; set; }
        public int HomeTeamPassingPlays { get; set; }
        public int HomeTeamRushingPlays { get; set; }
        public int AwayTeamDefensiveTacklesForLoss { get; set; }
        public int AwayTeamDefensiveSacks { get; set; }
        public int AwayTeamFieldGoals { get; set; }
        public int AwayTeamDefensiveSafety { get; set; }
        public int AwayTeamSpecialTeamsPoints { get; set; }
        public int AwayTeamPassingPlays { get; set; }
        public int AwayTeamRushingPlays { get; set; }
    }
}
