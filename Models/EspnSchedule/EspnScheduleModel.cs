using CollegeScorePredictor.Models.EspnGameSummary;

namespace CollegeScorePredictor.Models.EspnSchedule
{
#pragma warning disable IDE1006 // Naming Styles
    public class EspnScheduleModel
    {
        public List<Events> events { get; set; } = new List<Events>();
        public object? requestedSeason { get; set; }
        public object? season { get; set; }
        public string? status { get; set; }
        public object? team { get; set; }
        public string? timestamp { get; set; }
    }
    public class Events
    {
        public List<Competitions> competitions { get; set; } = new List<Competitions>();
        public string? date { get; set; }
        public string? id { get; set; }
        public object? links { get; set; }
        public string? name { get; set; }
        public object? season { get; set; }
        public object? seasonType { get; set; }
        public string? shortName { get; set; }
        public bool timeValid { get; set; }
        public object? week { get; set; }
    }

    
#pragma warning restore IDE1006 // Naming Styles
}
