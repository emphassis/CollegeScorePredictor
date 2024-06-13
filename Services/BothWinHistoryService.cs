using Microsoft.EntityFrameworkCore;

namespace CollegeScorePredictor.Services
{
    public class BothWinHistoryService
    {
        private IDbContextFactory<AppDbContext> factory;
        public BothWinHistoryService(IDbContextFactory<AppDbContext> _factory)
        {
            factory = _factory;
        }

        public async Task PopulateOverPredictionDbWithWinHistory()
        {
            //using(var db = await factory.CreateDbContextAsync())
            //{
            //    var overPrediction = await (from o in db.OverPrediction
            //                                select new
            //                                {
            //                                    o.OverPredictionId,
            //                                    o.TeamId,
            //                                    o.OpponentId,
            //                                    o.TeamWon,
            //                                    o.Week,
            //                                    o.Year
            //                                }).ToListAsync();

            //    foreach(var prediction in overPrediction)
            //    {
            //        var teamWins = (from o in overPrediction where o.TeamId == prediction.TeamId && o.TeamWon && o.Year == prediction.Year && o.Week < prediction.Week select o).Count();
            //        var teamLosses = (from o in overPrediction where o.TeamId == prediction.TeamId && !o.TeamWon && o.Year == prediction.Year && o.Week < prediction.Week select o).Count();
            //        var opponentWins = (from o in overPrediction where o.TeamId == prediction.OpponentId && o.TeamWon && o.Year == prediction.Year && o.Week < prediction.Week select o).Count();
            //        var opponentLosses = (from o in overPrediction where o.TeamId == prediction.OpponentId && !o.TeamWon && o.Year == prediction.Year && o.Week < prediction.Week select o).Count();

            //        var predictionToUpdate = db.OverPrediction.Where(x => x.OverPredictionId == prediction.OverPredictionId).First();
            //        predictionToUpdate.TeamWins = teamWins;
            //        predictionToUpdate.TeamLosses = teamLosses;
            //        predictionToUpdate.OpponentWins = opponentWins;
            //        predictionToUpdate.OpponentLosses = opponentLosses;
            //    }

            //    await db.SaveChangesAsync();

            //    var overPredictionConference = await (from o in db.OverPredictionConference
            //                                select new
            //                                {
            //                                    o.OverPredictionId,
            //                                    o.TeamId,
            //                                    o.OpponentId,
            //                                    o.TeamWon,
            //                                    o.Week,
            //                                    o.Year
            //                                }).ToListAsync();

            //    foreach (var prediction in overPredictionConference)
            //    {
            //        var teamWins = (from o in overPredictionConference where o.TeamId == prediction.TeamId && o.TeamWon && o.Year == prediction.Year && o.Week < prediction.Week select o).Count();
            //        var teamLosses = (from o in overPredictionConference where o.TeamId == prediction.TeamId && !o.TeamWon && o.Year == prediction.Year && o.Week < prediction.Week select o).Count();
            //        var opponentWins = (from o in overPredictionConference where o.TeamId == prediction.OpponentId && o.TeamWon && o.Year == prediction.Year && o.Week < prediction.Week select o).Count();
            //        var opponentLosses = (from o in overPredictionConference where o.TeamId == prediction.OpponentId && !o.TeamWon && o.Year == prediction.Year && o.Week < prediction.Week select o).Count();

            //        var predictionToUpdate = db.OverPredictionConference.Where(x => x.OverPredictionId == prediction.OverPredictionId).First();
            //        predictionToUpdate.TeamWins = teamWins;
            //        predictionToUpdate.TeamLosses = teamLosses;
            //        predictionToUpdate.OpponentWins = opponentWins;
            //        predictionToUpdate.OpponentLosses = opponentLosses;
            //    }

            //    await db.SaveChangesAsync();
            //}
        }
    }
}
