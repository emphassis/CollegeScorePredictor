namespace CollegeScorePredictor.Models.EspnGameSummary
{
#pragma warning disable IDE1006 // Naming Styles
    public class ScoringPlays
    {
        public int awayScore { get; set; }
        public Clock clock { get; set; } = new Clock();
        public int homeScore { get; set; }
        public string? id { get; set; }
        public Period period { get; set; } = new Period();
        public ScoringType scoringType { get; set; } = new ScoringType();
        public ScoringPlaysTeam team { get; set; } = new ScoringPlaysTeam();
        public string? text { get; set; }
        public object? type { get; set; }
    }

    public class Clock
    {
        public object? value { get; set; }
        public string? displayValue { get; set; }
    }
    public class Period
    {
        public int number { get; set; }
    }

    public class ScoringType
    {
        public string? name { get; set; }
        public string? displayName { get; set; }
        public string? abbreviation { get; set; }
    }

    public class ScoringPlaysTeam
    {
        public string? abbreviation { get; set; }
        public string? displayName { get; set; }
        public string? id { get; set; }
        public object? links { get; set; }
        public object? logo { get; set; }
        public object? logos { get; set; }
        public string? uid { get; set; }
    }

    public class Type
    {
        public string? abbreviation { get; set; }
        public string? id { get; set; }
        public string? text { get; set; }
    }
}
