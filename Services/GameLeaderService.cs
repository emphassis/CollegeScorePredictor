using CollegeScorePredictor.Models.Database;
using CollegeScorePredictor.Models.Record;
using CollegeScorePredictor.Operations;
using Microsoft.EntityFrameworkCore;

namespace CollegeScorePredictor.Services
{
    public class GameLeaderService
    {
        private IDbContextFactory<AppDbContext> factory;
        public GameLeaderService(IDbContextFactory<AppDbContext> _factory)
        {
            factory = _factory;
        }

        public async Task GenerateAllGameLeaders()
        {
            using (var db = await factory.CreateDbContextAsync())
            {
                var leadersList = await (from l in db.GameLeader select l).CountAsync();
                Console.WriteLine("---~~~---~~~---~~~---~~~---");
                Console.WriteLine("---~~~---~~~---~~~---~~~---");
                Console.WriteLine("---" + leadersList +" current records---");
                Console.WriteLine("---~~~---~~~---~~~---~~~---");
                Console.WriteLine("---~~~---~~~---~~~---~~~---");

                var events = await (from i in db.GameResult
                                    orderby i.Year descending
                                    select new
                                    {
                                        i.EventId,
                                        i.Week,
                                        i.Year
                                    }).ToListAsync();

                foreach( var id in events)
                {
                    var doesRecordExist = await (from d in db.GameLeader where d.EventId == id.EventId select d.EventId).AnyAsync();
                    if (doesRecordExist)
                    {
                        continue;
                    }

                    var model = await PopulateGameLeaderData.PopulateGameLeaderDataAsync(id.EventId.ToString());

                    if(model == null)
                    {
                        Console.WriteLine("skipped event ID: " + id.EventId.ToString());
                        continue;
                    }

                    var homeGameLeader = new GameLeaderDbo
                    {
                        EventId = id.EventId,
                        TeamId = model.HomeTeamId,
                        Week = id.Week,
                        Year = id.Year,

                        PassingAttempts = model.GameLeadersModel.HomePassingAttempts,
                        PassingCompletions = model.GameLeadersModel.HomePassingCompletions,
                        PassingYards = model.GameLeadersModel.HomePassingYards,
                        PassingTouchdowns = model.GameLeadersModel.HomePassingTouchdowns,
                        PassingInterceptions = model.GameLeadersModel.HomePassingInterceptions,
                        PassingPlayer = model.GameLeadersModel.HomePassingPlayer,
                        RushingAttempts = model.GameLeadersModel.HomeRushingAttempts,
                        RushingYards = model.GameLeadersModel.HomeRushingYards,
                        RushingTouchdowns = model.GameLeadersModel.HomeRushingTouchdowns,
                        RushingPlayer = model.GameLeadersModel.HomeRushingPlayer,
                        RushingIsQuarterback = model.GameLeadersModel.HomeRushingIsQuarterback,
                        ReceivingAttempts = model.GameLeadersModel.HomeReceivingAttempts,
                        ReceivingYards = model.GameLeadersModel.HomeReceivingYards,
                        ReceivingTouchdowns = model.GameLeadersModel.HomeReceivingTouchdowns,
                        ReceivingPlayer = model.GameLeadersModel.HomeReceivingPlayer,
                        ReceivingIsTightEnd = model.GameLeadersModel.HomeReceivingIsTightEnd
                    };

                    var awayGameLeader = new GameLeaderDbo
                    {
                        EventId = id.EventId,
                        TeamId = model.AwayTeamId,
                        Week = id.Week,
                        Year = id.Year,

                        PassingAttempts = model.GameLeadersModel.AwayPassingAttempts,
                        PassingCompletions = model.GameLeadersModel.AwayPassingCompletions,
                        PassingYards = model.GameLeadersModel.AwayPassingYards,
                        PassingTouchdowns = model.GameLeadersModel.AwayPassingTouchdowns,
                        PassingInterceptions = model.GameLeadersModel.AwayPassingInterceptions,
                        PassingPlayer = model.GameLeadersModel.AwayPassingPlayer,
                        RushingAttempts = model.GameLeadersModel.AwayRushingAttempts,
                        RushingYards = model.GameLeadersModel.AwayRushingYards,
                        RushingTouchdowns = model.GameLeadersModel.AwayRushingTouchdowns,
                        RushingPlayer = model.GameLeadersModel.AwayRushingPlayer,
                        RushingIsQuarterback = model.GameLeadersModel.AwayRushingIsQuarterback,
                        ReceivingAttempts = model.GameLeadersModel.AwayReceivingAttempts,
                        ReceivingYards = model.GameLeadersModel.AwayReceivingYards,
                        ReceivingTouchdowns = model.GameLeadersModel.AwayReceivingTouchdowns,
                        ReceivingPlayer = model.GameLeadersModel.AwayReceivingPlayer,
                        ReceivingIsTightEnd = model.GameLeadersModel.AwayReceivingIsTightEnd
                    };

                    await db.GameLeader.AddAsync(homeGameLeader);
                    await db.GameLeader.AddAsync(awayGameLeader);
                    await db.SaveChangesAsync();
                    Console.WriteLine("Added game with event ID: " + id.EventId.ToString());
                }
            }
        }
    }

    public class PopulateGameLeaderDataModel
    {
        public GetGameLeadersModel GameLeadersModel { get; set; } = new();
        public long HomeTeamId { get; set; }
        public long AwayTeamId { get; set; }
    }
}
