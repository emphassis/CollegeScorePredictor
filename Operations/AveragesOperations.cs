using System.Linq;
using System.Data;
using Microsoft.EntityFrameworkCore;

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
        private static bool AveragesPopulated = false;

        public static async Task PopulateAverages(AppDbContext db)
        {
            if (AveragesPopulated) return;
            var playerAverages = await db.GameLeader
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

        private class LeaderDataModel
        {
            public double PassingYards { get; set; }
            public double PassingAttempts { get; set; }
            public double PassingCompletions { get; set; }
            public double PassingTouchdowns { get; set; }
            public double PassingInterceptions { get; set; }
            public double RushingYards { get; set; }
            public double RushingAttempts { get; set; }
            public double RushingTouchdowns { get; set; }
            public double ReceivingYards { get; set; }
            public double ReceivingAttempts { get; set; }
            public double ReceivingTouchdowns { get; set; }
        }
    }
}
