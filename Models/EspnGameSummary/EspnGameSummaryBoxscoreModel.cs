namespace CollegeScorePredictor.Models.EspnGameSummary
{
#pragma warning disable IDE1006 // Naming Styles

    public class Boxscore
    {
        public object? players { get; set; }
        public List<Teams> teams { get; set; } = new List<Teams>();
    }
    public class Players
    {
        public List<PlayerStatistics> statistics { get; set; } = new List<PlayerStatistics>();
        public Team team { get; set; } = new Team();
    }

    public class PlayerStatistics
    {
        //TODO: not going to use this. but I could build a player profile on a later rendition
        //inside athletes there is a player ID. I can actually track stats of individual players
        public string? athletes { get; set; }
        public string? descriptions { get; set; }
        public string? keys { get; set; }
        public string? labels { get; set; }
        public string? name { get; set; } //this matches player statistics string enum
        public string? text { get; set; }
        public string? totals { get; set; }
    }


    public class Teams
    {
        public List<TeamStatistics> statistics { get; set; } = new List<TeamStatistics>();
        public Team team { get; set; } = new Team();
    }
    public class Team
    {
        public string? abbreviation { get; set; }
        public string? alternateColor { get; set; }
        public string? color { get; set; }
        public string? displayName { get; set; }
        public string? id { get; set; }
        public string? location { get; set; }
        public string? logo { get; set; }
        public string? name { get; set; }
        public string? shortDisplayName { get; set; }
        public string? slug { get; set; }
        public string? uid { get; set; }
    }
    public class TeamStatistics
    {
        public string? displayValue { get; set; }
        public string? label { get; set; }
        public string? name { get; set; }
    }
}
