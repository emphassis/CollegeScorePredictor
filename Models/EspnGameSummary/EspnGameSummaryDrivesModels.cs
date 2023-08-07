namespace CollegeScorePredictor.Models.EspnGameSummary
{
#pragma warning disable IDE1006 // Naming Styles
    public class Drives
    {
        public object? current { get; set; }
        public List<Previous> previous { get; set; } = new List<Previous>();
    }

    public class Previous
    {
        public string? description { get; set; }
        public string? displayResult { get; set; }
        public object? end { get; set; }
        public string id { get; set; }
        public bool isScore { get; set; }
        public int offensivePlays { get; set; }
        public List<Plays> plays { get; set; } = new List<Plays>();
        public string? result { get; set; }
        public string? shortDisplayResult { get; set; }
        public object? start { get; set; }
        public DrivesTeam team { get; set; } = new DrivesTeam();
        public object? timeElapsed { get; set; }
        public int yards { get; set; }
    }

    public class Plays
    {
        public int awayScore { get; set; }
        public object? clock { get; set; }
        public object? end { get; set; }
        public int homeScore { get; set; }
        public string? id { get; set; }
        public string? modified { get; set; }
        public object? period { get; set; }
        public bool priority { get; set; }
        public bool scoringPlay { get; set; }
        public string? sequenceNumber { get; set; }
        public object? start { get; set; }
        public int statYardage { get; set; }
        public string? text { get; set; }
        public PlayType type { get; set; } = new PlayType();
        public string? wallclock { get; set; }
    }

    public class PlayType
    {
        public string? id { get; set; }
        public string? text { get; set; }
    }

    public class DrivesTeam
    {
        public string? abbreviation { get; set; }
        public string? displayName { get; set; }
        public object? logos { get; set; }
        public string? name { get; set; }
        public string? shortDisplayName { get; set; }
    }
}
