namespace CollegeScorePredictor.Models.EspnGameSummary
{
#pragma warning disable IDE1006 // Naming Styles

    public class GameSummaryLeadersModel
    {
        public List<LeadersList> leaders { get; set; } = new();
        public Team? team { get; set; }
    }

    public class LeadersList
    {
        public string? displayName { get; set; } //"Receiving Yards"
        public List<Leaders> leaders { get; set; } = new List<Leaders>();
        public string? name { get; set; }//"receivingYards" "passingYards" "rushingYards"
    }

    public class Leaders
    {
        public Athlete? athlete { get; set; }
        public string? displayValue { get; set; }
    }

    public class Athlete
    {
        public string? displayName { get; set; }
        public string? fullName { get; set; }
        public string? guid { get; set; }
        public object? headshot { get; set; }
        public string? id { get; set; }
        public string? jersey { get; set; }
        public string? lastName { get; set; }
        public object? links { get; set; }
        public Position? position { get; set; }
        public string? shortName { get; set; }
        public string? uid { get; set; }
    }

    public class Position
    {
       public string? abbreviation { get; set; } //"WR" "QB" "RB" "TE"
    }
#pragma warning restore IDE1006 // Naming Styles
}
