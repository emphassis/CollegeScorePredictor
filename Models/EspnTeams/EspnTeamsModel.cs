namespace CollegeScorePredictor.Models.EspnTeams
{
#pragma warning disable IDE1006 // Naming Styles
    public class EspnTeamsModel
    {
        public List<Sports> sports { get; set; } = new List<Sports>(); //.first() always just one
    }

    public class Sports
    {
        public string? id { get; set; }//think football is just id 20...
        public List<Leagues> leagues { get; set; } = new List<Leagues>(); //.first() always just one
        public string? name { get; set; }//football
        public string? slug { get; set; }
        public string? uid { get; set; }
    }

    public class Leagues
    {
        public string? abbreviation { get; set; } // NCAAF
        public List<Groups> groups { get; set; } = new List<Groups>();
        public string? id { get; set; } //23
        public string? name { get; set; } //NCAA - Football
        public Season season { get; set; } = new Season();
        public string? shortName { get; set; }
        public string? slug { get; set; }
        public string? uid { get; set; }
        public int year { get; set; }
    }

    public class Groups
    {
        public string? midsizeName { get; set; }
        public string? name { get; set; }
        public List<Teams> teams { get; set; } = new List<Teams>();
        public string? uid { get; set; }
    }

    public class Teams
    {
        public string? abbreviation { get; set; }
        public string? alternateColor { get; set; }
        public string? color { get; set; }
        public string? displayName { get; set; }
        public string? id { get; set; }
        public bool isActive { get; set; }
        public bool isAllStar { get; set; }
        public object? links { get; set; }
        public string? location { get; set; }
        public object? logos { get; set; }
        public string? name { get; set; }
        public string? nickname { get; set; }
        public string? shortDisplayName { get; set; }
        public string? slug { get; set; }
        public string? uid { get; set; }
    }

    public class Season
    {
        public string? displayName { get; set; }
        public int year { get; set; }
    }
}
