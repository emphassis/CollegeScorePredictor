using System.ComponentModel.DataAnnotations;

namespace CollegeScorePredictor.Models.Database
{
    public class ConferenceTeamDbo
    {
        [Key]
        public long ConferenceTeamId { get; set; }
        public long TeamId { get; set; }
        public long ActionNetworkId { get; set; }
        public byte TeamConference { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public string ConferenceName { get; set; } = string.Empty;
        public bool IsFBS { get; set; }
    }
}
