using CollegeScorePredictor.Models.Database;
using CollegeScorePredictor.Models.EspnTeams;
using CollegeScorePredictor.Operations;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CollegeScorePredictor.Services
{
    public class ConferenceTeamsService
    {
        private IDbContextFactory<AppDbContext> factory;
        public ConferenceTeamsService(IDbContextFactory<AppDbContext> _factory)
        {
            factory = _factory;
        }

        public async Task<bool> SetConferenceTeamsAsync()
        {
            var client = new HttpClient();
            var apiUrl = "https://site.web.api.espn.com/apis/site/v2/sports/football/college-football/teams?region=us&lang=en&contentorigin=espn&limit=400&groups=80&groupType=conference&enable=groups";
            var data = await client.GetStringAsync(apiUrl);
            EspnTeamsModel? model;

            try
            {
                model = JsonSerializer.Deserialize<EspnTeamsModel>(data);
                if (model == null)
                {
                    return false;
                }
            }
            catch { return false; }

            List<Groups> conferences = model.sports.First().leagues.First().groups;

            using (var db = await factory.CreateDbContextAsync())
            {
                foreach (Groups conference in conferences)
                {
                    foreach (Models.EspnTeams.Teams team in conference.teams)
                    {
                        var teamId = TeamOperations.GetTeamId(team.id);
                        var conferenceId = GetConferenceId(conference.name!);
                        var teamName = team.displayName!;

                        var dbo = new ConferenceTeamDbo
                        {
                            TeamId = teamId,
                            TeamConference = conferenceId,
                            ConferenceName = conference.name!,
                            TeamName = teamName,
                            IsFBS = true
                        };
                        db.ConferenceTeam.Add(dbo);
                    }
                }

                await db.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> DoConferenceTeamsExistAsync()
        {
            using (var db = await factory.CreateDbContextAsync())
            {
                return await (from c in db.ConferenceTeam
                              select c).AnyAsync();
            }
        }

        private byte GetConferenceId(string conferenceName)
        {
            switch (conferenceName)
            {
                case "American Athletic Conference":
                    return 0;
                case "Atlantic Coast Conference":
                    return 1;
                case "Big 12 Conference":
                    return 2;
                case "Big Ten Conference":
                    return 3;
                case "Conference USA":
                    return 4;
                case "FBS Independents":
                    return 5;
                case "Mid-American Conference":
                    return 6;
                case "Mountain West Conference":
                    return 7;
                case "Pac-12 Conference":
                    return 8;
                case "Southeastern Conference":
                    return 9;
                case "Sun Belt Conference":
                    return 10;
                default: return 100;
            }
        }
    }
}
