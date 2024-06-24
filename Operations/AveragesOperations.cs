using System.Linq;
using System.Data;
using Microsoft.EntityFrameworkCore;
using CollegeScorePredictor.Models.Grade;

namespace CollegeScorePredictor.Operations
{
    public static class AveragesOperations
    {
        public static double TeamAveragePassingYards { get; set; }
        public static double TeamAveragePassingAttempts { get; set; }
        public static double TeamAveragePassingCompletions { get; set; }
        public static double TeamAveragePassingTouchdowns { get; set; }
        public static double TeamAveragePassingInterceptions { get; set; }
        public static double TeamAverageRushingYards { get; set; }
        public static double TeamAverageRushingAttempts { get; set; }
        public static double TeamAverageRushingTouchdowns { get; set; }
        public static double TeamAverageReceivingYards { get; set; }
        public static double TeamAverageReceivingAttempts { get; set; }
        public static double TeamAverageReceivingTouchdowns { get; set; }
        public static double PlayerAveragePassingYards { get; set; }
        public static double PlayerAveragePassingAttempts { get; set; }
        public static double PlayerAveragePassingCompletions { get; set; }
        public static double PlayerAveragePassingTouchdowns { get; set; }
        public static double PlayerAveragePassingInterceptions { get; set; }
        public static double PlayerAverageRushingYards { get; set; }
        public static double PlayerAverageRushingAttempts { get; set; }
        public static double PlayerAverageRushingTouchdowns { get; set; }
        public static double PlayerAverageReceivingYards { get; set; }
        public static double PlayerAverageReceivingAttempts { get; set; }
        public static double PlayerAverageReceivingTouchdowns { get; set; }
        public static double ConferenceAACWinAverage { get; set; }
        public static double ConferenceACCWinAverage { get; set; }
        public static double ConferenceBig12WinAverage { get; set; }
        public static double ConferenceBig10WinAverage { get; set; }
        public static double ConferenceUSAWinAverage { get; set; }
        public static double ConferenceIndependentWinAverage { get; set; }
        public static double ConferenceMACWinAverage { get; set; }
        public static double ConferenceMountainWestWinAverage { get; set; }
        public static double ConferencePac12WinAverage { get; set; }
        public static double ConferenceSECWinAverage { get; set; }
        public static double ConferenceSunBeltWinAverage { get; set; }
        private static bool AveragesPopulated = false;

        public static async Task PopulateAverages(AppDbContext db, int year)
        {
            if (AveragesPopulated) return;
            var playerAverages = await db.GameLeader
                .Where(x => x.Year == year)
                .GroupBy(x => true)
                .Select(g => new LeaderDataModel
                {
                    PassingYards = g.Average(x => x.PassingYards),
                    PassingAttempts = g.Average(x => x.PassingAttempts),
                    PassingCompletions = g.Average(x => x.PassingCompletions),
                    PassingTouchdowns = g.Average(x => x.PassingTouchdowns),
                    PassingInterceptions = g.Average(x => x.PassingInterceptions),
                    RushingYards = g.Average(x => x.RushingYards),
                    RushingAttempts = g.Average(x => x.RushingAttempts),
                    RushingTouchdowns = g.Average(x => x.RushingTouchdowns),
                    ReceivingYards = g.Average(x => x.ReceivingYards),
                    ReceivingAttempts = g.Average(x => x.ReceivingAttempts),
                    ReceivingTouchdowns = g.Average(x => x.ReceivingTouchdowns)
                }).FirstAsync();

            var teamAverages = await db.GameResult
                .Where(x => x.Year == year)
                .GroupBy(x => true)
                .Select(g => new LeaderDataModel
                {
                    PassingYards = g.Average(x => (x.AwayTeamNetPassingYards + x.HomeTeamNetPassingYards) / 2),
                    PassingAttempts = g.Average(x => (x.AwayTeamPassingAttempts + x.HomeTeamPassingAttempts) / 2),
                    PassingCompletions = g.Average(x => (x.AwayTeamPassingCompletions + x.HomeTeamPassingCompletions) / 2),
                    PassingTouchdowns = 1,
                    PassingInterceptions = g.Average(x => (x.AwayTeamInterceptions + x.HomeTeamInterceptions) / 2),
                    RushingYards = g.Average(x => (x.AwayTeamRushingYards + x.HomeTeamRushingYards) / 2),
                    RushingAttempts = g.Average(x => (x.AwayTeamRushingAttempts + x.HomeTeamRushingAttempts) / 2),
                    RushingTouchdowns = 1,
                    ReceivingYards = g.Average(x => (x.AwayTeamNetPassingYards + x.HomeTeamNetPassingYards) / 2),
                    ReceivingAttempts = g.Average(x => (x.AwayTeamPassingAttempts + x.HomeTeamPassingAttempts) / 2),
                    ReceivingTouchdowns = 1
                }).FirstAsync();


            TeamAveragePassingYards = teamAverages.PassingYards;
            TeamAveragePassingAttempts = teamAverages.PassingAttempts;
            TeamAveragePassingCompletions = teamAverages.PassingCompletions;
            TeamAveragePassingTouchdowns = 1;
            TeamAveragePassingInterceptions = teamAverages.PassingInterceptions;
            TeamAverageRushingYards = teamAverages.RushingYards;
            TeamAverageRushingAttempts = teamAverages.RushingAttempts;
            TeamAverageRushingTouchdowns = 1;
            TeamAverageReceivingYards = teamAverages.ReceivingYards;
            TeamAverageReceivingAttempts = teamAverages.ReceivingAttempts;
            TeamAverageReceivingTouchdowns = 1;
            PlayerAveragePassingYards = playerAverages.PassingYards;
            PlayerAveragePassingAttempts = playerAverages.PassingAttempts;
            PlayerAveragePassingCompletions = playerAverages.PassingCompletions;
            PlayerAveragePassingTouchdowns = playerAverages.PassingTouchdowns;
            PlayerAveragePassingInterceptions = playerAverages.PassingInterceptions;
            PlayerAverageRushingYards = playerAverages.RushingYards;
            PlayerAverageRushingAttempts = playerAverages.RushingAttempts;
            PlayerAverageRushingTouchdowns = playerAverages.RushingTouchdowns;
            PlayerAverageReceivingYards = playerAverages.ReceivingYards;
            PlayerAverageReceivingAttempts = playerAverages.ReceivingAttempts;
            PlayerAverageReceivingTouchdowns = playerAverages.ReceivingTouchdowns;

            AveragesPopulated = true;
            Console.WriteLine("Averages Populated");
        }

        public static double GetConferenceWinAverage(int conference)
        {
            switch (conference)
            {
                case 0:
                    return ConferenceAACWinAverage;
                case 1:
                    return ConferenceACCWinAverage;
                case 2:
                    return ConferenceBig12WinAverage;
                case 3:
                    return ConferenceBig10WinAverage;
                case 4:
                    return ConferenceUSAWinAverage;
                case 5:
                    return ConferenceIndependentWinAverage;
                case 6:
                    return ConferenceMACWinAverage;
                case 7:
                    return ConferenceMountainWestWinAverage;
                case 8:
                    return ConferencePac12WinAverage;
                case 9:
                    return ConferenceSECWinAverage;
                case 10:
                    return ConferenceSunBeltWinAverage;
                default:
                    return 0;
            }
        }

        private static async Task PopulateConferenceWins(AppDbContext db, int year)
        {
            for (var i = 0; i < 11; i++)
            {
                var conferenceGames = await (from e in db.GameResult
                                             where e.HomeTeamConference == i || e.AwayTeamConference == i
                                             && e.Year == year
                                             select new
                                             {
                                                 e.HomeTeamConference,
                                                 e.HomeTeamWon,
                                                 e.AwayTeamConference
                                             }).ToListAsync();

                var totalGames = conferenceGames.Where(x => x.HomeTeamConference == i).Count() + conferenceGames.Where(x => x.AwayTeamConference == i).Count();
                var wins = conferenceGames.Where(x => x.HomeTeamConference == i && x.HomeTeamWon).Count() + conferenceGames.Where(x => x.AwayTeamConference == i && !x.HomeTeamWon).Count();

                if (wins == 0 || totalGames == 0)
                {
                    continue;
                }
                else
                {
                    switch (i)
                    {
                        case 0:
                            ConferenceAACWinAverage = wins / totalGames;
                            break;
                        case 1:
                            ConferenceACCWinAverage = wins / totalGames;
                            break;
                        case 2:
                            ConferenceBig12WinAverage = wins / totalGames;
                            break;
                        case 3:
                            ConferenceBig10WinAverage = wins / totalGames;
                            break;
                        case 4:
                            ConferenceUSAWinAverage = wins / totalGames;
                            break;
                        case 5:
                            ConferenceIndependentWinAverage = wins / totalGames;
                            break;
                        case 6:
                            ConferenceMACWinAverage = wins / totalGames;
                            break;
                        case 7:
                            ConferenceMountainWestWinAverage = wins / totalGames;
                            break;
                        case 8:
                            ConferencePac12WinAverage = wins / totalGames;
                            break;
                        case 9:
                            ConferenceSECWinAverage = wins / totalGames;
                            break;
                        case 10:
                            ConferenceSunBeltWinAverage = wins / totalGames;
                            break;
                    }
                }
            }
        }
    }
}
