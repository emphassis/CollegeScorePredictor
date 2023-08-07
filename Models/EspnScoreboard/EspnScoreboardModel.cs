namespace CollegeScorePredictor.Models.EspnScoreboard
{
#pragma warning disable IDE1006 // Naming Styles
    public class EspnScoreboardModel
    {
        public List<Events> events { get; set; } = new List<Events>(); // this is the list of all games in this time frame
        public object? leagues { get; set; }
    }

    public class Events
    {
        public List<Competitions> competitions { get; set; } = new List<Competitions>(); // always first(). only one
        public string? date { get; set; }
        public string? id { get; set; }
        public object? links { get; set; }
        public string? name { get; set; }
        public Season season { get; set; } = new Season();
        public string? shortName { get; set; }
        public object? status { get; set; }
        public object? uid { get; set; }
        public Week week { get; set; } = new Week();
    }

    public class Week
    {
        public int number { get; set; }
    }

    public class Season
    {
        public string? slug { get; set; }
        public int type { get; set; }
        public int year { get; set; }
    }

    public class Competitions
    {
        public int attendance { get; set; }
        public List<Competitors> competitors { get; set; } = new List<Competitors>();
        public bool conferenceCompetition { get; set; } // important
        public string? date { get; set; }
        public object? format { get; set; }
        public object? geoBroadcasts { get; set; }
        public object? headlines { get; set; }
        public string? id { get; set; } //most important thing... Event Id
        public object? leaders { get; set; }
        public bool neutralSite { get; set; }
        public object? notes { get; set; }
        public bool playByPlayAvailable { get; set; } // maybe important for "drives"
        public bool recent { get; set; }
        public string? startDate { get; set; }
        public object? status { get; set; }
        public bool timeValid { get; set; }
        public object? type { get; set; }
        public string? uid { get; set; }
        public object? venue { get; set; }
        public string? a { get; set; }
    }

    public class Competitors
    {
        public CuratedRank curatedRank { get; set; } = new CuratedRank(); // top 10 or 99 it seems
        public string? homeAway { get; set; }
        public string? id { get; set; } // teamId
        public object? linescores { get; set; }
        public int order { get; set; }
        public List<Record> records { get; set; } = new List<Record>();
        public string? score { get; set; }
        public object? statistics { get; set; }
        public object? team { get; set; }
        public string? type { get; set; }
        public string? uid { get; set; }
        public bool winner { get; set; }
    }

    public class CuratedRank
    {
        public int current { get; set; }
    }

    public class Record
    {
        public string? abbreviation { get; set; }
        public string? name { get; set; }
        public string? summary { get; set; }//3-2, 3-3, 4-4
        public string? type { get; set; }//total, vsconf, home, road
    }
}
