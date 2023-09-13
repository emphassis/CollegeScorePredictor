namespace CollegeScorePredictor.Models.EspnGameSummary
{
#pragma warning disable IDE1006 // Naming Styles

    public class Header
    {
        public List<Competitions> competitions { get; set; } = new List<Competitions>();
        public string? id { get; set; }
        public object? league { get; set; }
        public object? links { get; set; }
        public Season season { get; set; } = new Season();
        public bool timeValid { get; set; }
        public string? uid { get; set; }
        public int week { get; set; }
    }
    public class Season
    {
        public int type { get; set; }//2 seems to be regular season
        public int year { get; set; }
    }
    public class Competitions
    {
        public bool boxscoreAvailable { get; set; } = false;
        public object? boxscoreSource { get; set; }
        public object? broadcasts { get; set; }
        public bool commentaryAvailable { get; set; }
        public List<Competitors> competitors { get; set; } = new List<Competitors>();
        public bool conferenceCompetition { get; set; } //maybe
        public object? date { get; set; }
        public object? groups { get; set; }
        public object? id { get; set; }
        public bool liveAvailable { get; set; }
        public bool neutralSite { get; set; } //maybe
        public bool onWatchESPN { get; set; }
        public object? playByPlaySource { get; set; }//maybe
    }
    public class Competitors
    {
        public string? homeAway { get; set; }
        public string? id { get; set; }
        public object? linescores { get; set; } // array of four, scores per quarter? 
        public int order { get; set; }
        public bool possession { get; set; }
        public object? record { get; set; }
        public object? score { get; set; }
        public object? team { get; set; }//hopefully don't need
        public string? uid { get; set; }
        public bool winner { get; set; }
    }
}
