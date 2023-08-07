namespace CollegeScorePredictor.Models.BettingData
{
    public class BettingDataOddsModel
    {
        public List<ScoresModel> Scores { get; set; } = new List<ScoresModel>();
        public object? SportsBooks { get; set; }
    }

    public class ScoresModel
    {
        public int SeasonType { get; set; }
        public long GameID { get; set; }
        public int Season { get; set; }
        public string? Type { get; set; }
        public int Week { get; set; }
        public string? Day { get; set; }
        public string? DateTime { get; set; }
        public string? DayString { get; set; }
        public string? TimeString { get; set; }
        public string? DayOfWeek { get; set; }
        public string? DateTimeString { get; set; }
        public string? LastUpdated { get; set; }
        public bool ShouldHaveStarted { get; set; }
        public string? Status { get; set; }
        public int AwayTeamID { get; set; }
        public string? AwayTeam { get; set; }
        public int AwayTeamScore { get; set; }
        public bool AwayTeamHasWon { get; set; }
        public AwayTeamDetails AwayTeamDetails { get; set; } = new AwayTeamDetails();
        public int HomeTeamID { get; set; }
        public string? HomeTeam { get; set; }
        public int HomeTeamScore { get; set; }
        public bool HomeTeamHasWon { get; set; }
        public HomeTeamDetails HomeTeamDetails { get; set; } = new HomeTeamDetails();
        public object? Periods { get; set; }
        public string? PeriodNames { get; set; }
        public string? HomeTeamPeriods { get; set; }
        public string? AwayTeamPeriods { get; set; }
        public double? OverUnder { get; set; }
        public double? PointSpread { get; set; }
        public double? HomeTeamMoneyLine { get; set; }
        public double? AwayTeamMoneyLine { get; set; }
        public double? PointSpreadHomeTeamMoneyLine { get; set; }
        public double? PointSpreadAwayTeamMoneyLine { get; set; }
        public object? GameOddWebs { get; set; }
        public int Scope { get; set; }
        public object? Consensus { get; set; }
        public object? Opener { get; set; }
        public List<GameOddsBySportsBookModel> GameOddsBySportsBook { get; set; } = new List<GameOddsBySportsBookModel>();
    }

    public class AwayTeamDetails
    {
        public int TeamID { get; set; }
        public string? Name { get; set; }
        public string? FullName { get; set; }
        public string? Key { get; set; }
        public string? WikipediaLogoUrl { get; set; }
        public string? PrimaryColor { get; set; }
        public string? City { get; set; }
    }

    public class HomeTeamDetails
    {
        public int TeamID { get; set; }
        public string? Name { get; set; }
        public string? FullName { get; set; }
        public string? Key { get; set; }
        public string? WikipediaLogoUrl { get; set; }
        public string? PrimaryColor { get; set; }
        public string? City { get; set; }
    }

    public class GameOddsBySportsBookModel
    {
        public int Key { get; set; }
        public SportsBookModel Value { get; set; } = new SportsBookModel();
    }
    
    public class SportsBookModel
    {
        public long GameOddId { get; set; }
        public int SportsBookId { get; set; }
        public string? SportsBookName { get; set; }
        public long GameId { get; set; }
        public string? Updated { get; set; }
        public string? Created { get; set; }
        public double? HomeMoneyLine { get; set; }
        public double? AwayMoneyLine { get; set; }
        public double? DrawMoneyLine { get; set; }
        public double? HomePointSpread { get; set; }
        public double? AwayPointSpread { get; set; }
        public double? HomePointSpreadPayout { get; set; }
        public double? AwayPointSpreadPayout { get; set; }
        public double? OverUnder { get; set; }
        public double? OverPayout { get; set; }
        public double? UnderPayout { get; set; }
        public double? HomeTeamTotal { get; set; }
        public double? AwayTeamTotal { get; set; }
        public double? HomeTeamTotalPayout { get; set; }
        public double? AwayTeamTotalPayout { get; set; }
        public bool HomeBestSpread { get; set; }
        public bool AwayBestSpread { get; set; }
        public bool HomeBestMoneyline { get; set; }
        public bool AwayBestMoneyline { get; set; }
        public bool BestOver { get; set; }
        public bool BestUnder { get; set; }
        public string? Scope { get; set; }
        public string? Type { get; set; }
    }
}
