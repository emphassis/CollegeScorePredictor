using Microsoft.EntityFrameworkCore;

namespace CollegeScorePredictor.Operations
{
    public static class ConferenceOperations
    {
        public static async Task<double> GetConferenceScoreAverage(AppDbContext db, int year, bool homeTeam, long conferenceOne, long conferenceTwo)
        {
            double average;
            if (homeTeam)
            {
                average = await (from g in db.GameResult
                                 where g.Year == year &&
                                 g.HomeTeamConference == conferenceOne &&
                                 g.AwayTeamConference == conferenceTwo
                                 select g.HomeTeamScore).AverageAsync();


            }
            else
            {
                average = await (from g in db.GameResult
                                 where g.Year == year &&
                                 g.AwayTeamConference == conferenceOne &&
                                 g.HomeTeamConference == conferenceTwo
                                 select g.AwayTeamScore).AverageAsync();
            }
            if (average == 0)
            {
                if (homeTeam)
                {
                    return 31.89;
                }
                else
                {
                    return 24.55;
                }
            }
            return average;
        }

        private enum Conference : int
        {
            AmericanAthletic = 0,
            AtlanticCoastal = 1,
            Big12 = 2,
            BigTen = 3,
            ConferenceUsa = 4,
            Independents = 5,
            MidAmerican = 6,
            MountainWest = 7,
            Pac12 = 8,
            SouthEastern = 9,
            Sunbelt = 10
        }
    }
}
