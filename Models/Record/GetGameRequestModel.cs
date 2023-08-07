namespace CollegeScorePredictor.Models.Record
{
    public class GetGameRequestModel
    {
        public string EventId { get; set; } = string.Empty;
        public int Week { get; set; }
        public int Year { get; set; }
        public long HomeTeamId { get; set; }
        public long AwayTeamId { get; set; }
        public bool PlayByPlayAvailable { get; set; }
    }
}
