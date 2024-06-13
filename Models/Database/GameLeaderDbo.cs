namespace CollegeScorePredictor.Models.Database
{
    public class GameLeaderDbo
    {
        public long GameLeaderId { get; set; }
        public long EventId { get; set; }
        public long TeamId { get; set; }
        public int Week { get; set; }
        public int Year { get; set; }
        
        public long PassingPlayer { get; set; }
        public int PassingYards { get; set; }
        public int PassingAttempts { get; set; }
        public int PassingCompletions { get; set; }
        public int PassingTouchdowns { get; set; }
        public int PassingInterceptions { get; set; }
        
        public long RushingPlayer { get; set; }
        public int RushingYards { get; set; }
        public int RushingAttempts { get; set; }
        public int RushingTouchdowns { get; set; }
        public bool RushingIsQuarterback { get; set; }
        
        public long ReceivingPlayer { get; set; }
        public int ReceivingYards { get; set; }
        public int ReceivingAttempts { get; set; }
        public int ReceivingTouchdowns { get; set; }
        public bool ReceivingIsTightEnd { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
