using CollegeScorePredictor.Pages.Bet;

namespace CollegeScorePredictor.Models.Record
{
    public class GetGameLeadersModel
    {
        public long HomePassingPlayer { get; set; }
        public int HomePassingYards { get; set; }
        public int HomePassingAttempts { get; set; }
        public int HomePassingCompletions { get; set; }
        public int HomePassingTouchdowns { get; set; }
        public int HomePassingInterceptions { get; set; }

        public long HomeRushingPlayer { get; set; }
        public int HomeRushingYards { get; set; }
        public int HomeRushingAttempts { get; set; }
        public int HomeRushingTouchdowns { get; set; }
        public bool HomeRushingIsQuarterback { get; set; }

        public long HomeReceivingPlayer { get; set; }
        public int HomeReceivingYards { get; set; }
        public int HomeReceivingAttempts { get; set; }
        public int HomeReceivingTouchdowns { get; set; }
        public bool HomeReceivingIsTightEnd { get; set; }

        public long AwayPassingPlayer { get; set; }
        public int AwayPassingYards { get; set; }
        public int AwayPassingAttempts { get; set; }
        public int AwayPassingCompletions { get; set; }
        public int AwayPassingTouchdowns { get; set; }
        public int AwayPassingInterceptions { get; set; }

        public long AwayRushingPlayer { get; set; }
        public int AwayRushingYards { get; set; }
        public int AwayRushingAttempts { get; set; }
        public int AwayRushingTouchdowns { get; set; }
        public bool AwayRushingIsQuarterback { get; set; }

        public long AwayReceivingPlayer { get; set; }
        public int AwayReceivingYards { get; set; }
        public int AwayReceivingAttempts { get; set; }
        public int AwayReceivingTouchdowns { get; set; }
        public bool AwayReceivingIsTightEnd { get; set; }
    }
}
