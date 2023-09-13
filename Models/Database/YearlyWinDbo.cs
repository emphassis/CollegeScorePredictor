namespace CollegeScorePredictor.Models.Database
{
    public class YearlyWinDbo
    {
        public long YearlyWinId { get; set; }
        public long AwayTeamId { get; set; }
        public long HomeTeamId { get; set; }
        public float AwayTeamWin { get; set; }
        public float HomeTeamWin { get; set; }
        public int Year { get; set; }
    }
}
