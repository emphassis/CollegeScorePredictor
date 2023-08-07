using CollegeScorePredictor.AIModels.OffensivePrediction.Models;
using CollegeScorePredictor.AIModels.OverPrediction.Database;
using CollegeScorePredictor.Models.Database;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CollegeScorePredictor.AIModels.OverPrediction
{
    public class GenerateSubModelService
    {
        private IDbContextFactory<AppDbContext> factory;
        public GenerateSubModelService(IDbContextFactory<AppDbContext> _factory)
        {
            factory = _factory;
        }

        public async Task PopulateDatabaseWithOverPredictionData(int year, int? weeek)
        {
            using (var db = await factory.CreateDbContextAsync())
            {
                List<GameResultDbo> games = new List<GameResultDbo>();

                var gamesToSave = await (from e in db.GameResult
                                         where e.Year == year
                                         orderby e.Week ascending
                                         select e).ToListAsync();

                if (weeek != null)
                {
                    gamesToSave = gamesToSave.Where(x => x.Week == weeek).ToList();
                }

                var week = weeek;

                List<GameResultDbo> gameStats = new List<GameResultDbo>();

                foreach (var game in gamesToSave)
                {
                    if (gameStats.Count == 0)
                    {
                        var lastYear = year - 1;
                        if (week == 1)
                        {
                            var gameStatsLastYear = await (from e in db.GameResult
                                                           where e.Year == lastYear && e.Week >= 13
                                                           select e).ToListAsync();
                            var gameStatsThisYear = await (from e in db.GameResult
                                                           where e.Year == year && e.Week == 1
                                                           select e).ToListAsync();

                            gameStats.AddRange(gameStatsLastYear);
                            gameStats.AddRange(gameStatsThisYear);
                        }
                        else if (week == 2)
                        {
                            var gameStatsLastYear = await (from e in db.GameResult
                                                           where e.Year == lastYear && e.Week >= 14
                                                           select e).ToListAsync();
                            var gameStatsThisYear = await (from e in db.GameResult
                                                           where e.Year == year && e.Week <= 2
                                                           select e).ToListAsync();

                            gameStats.AddRange(gameStatsLastYear);
                            gameStats.AddRange(gameStatsThisYear);
                        }
                        else if (week == 3)
                        {
                            var gameStatsLastYear = await (from e in db.GameResult
                                                           where e.Year == lastYear && e.Week >= 15
                                                           select e).ToListAsync();
                            var gameStatsThisYear = await (from e in db.GameResult
                                                           where e.Year == year && e.Week <= 3
                                                           select e).ToListAsync();

                            gameStats.AddRange(gameStatsLastYear);
                            gameStats.AddRange(gameStatsThisYear);
                        }
                        else
                        {
                            gameStats = await (from e in db.GameResult
                                               where e.Year == year && e.Week <= week
                                               select e).ToListAsync();
                        }

                        games = gameStats.DistinctBy(x => x.EventId).ToList();
                    }

                    Console.WriteLine("Adding " + game.AwayTeamName + " at " + game.HomeTeamName + ". Week: " + game.Week);

                    var awayTeamPastGames = games.Where(x => x.AwayTeamId == game.AwayTeamId || x.HomeTeamId == game.AwayTeamId);
                    var homeTeamPastGames = games.Where(x => x.AwayTeamId == game.HomeTeamId || x.HomeTeamId == game.HomeTeamId);

                    var awayTeamAwayOpponents = awayTeamPastGames.Where(x => x.AwayTeamId == game.AwayTeamId).Select(x => x).ToList();
                    var awayTeamHomeOpponents = awayTeamPastGames.Where(x => x.HomeTeamId == game.AwayTeamId).Select(x => x).ToList();

                    var homeTeamAwayOpponents = homeTeamPastGames.Where(x => x.AwayTeamId == game.HomeTeamId).Select(x => x).ToList();
                    var homeTeamHomeOpponents = homeTeamPastGames.Where(x => x.HomeTeamId == game.HomeTeamId).Select(x => x).ToList();

                    bool atWon = game.AwayTeamWon;
                    bool htWon = game.HomeTeamWon;

                    #region First Down Prediction
                    var atAwayPastFirstDowns = awayTeamPastGames.Where(x => x.AwayTeamId == game.AwayTeamId).Select(x => x.AwayTeamFirstDowns).Sum();
                    var atHomePastFirstDowns = awayTeamPastGames.Where(x => x.HomeTeamId == game.AwayTeamId).Select(x => x.HomeTeamFirstDowns).Sum();
                    double atPastFirstDowns = 0;

                    var htAwayPastFirstDowns = homeTeamPastGames.Where(x => x.AwayTeamId == game.HomeTeamId).Select(x => x.AwayTeamFirstDowns).Sum();
                    var htHomePastFirstDowns = homeTeamPastGames.Where(x => x.HomeTeamId == game.HomeTeamId).Select(x => x.HomeTeamFirstDowns).Sum();
                    double htPastFirstDowns = 0;

                    try
                    {
                        atPastFirstDowns = (atAwayPastFirstDowns + atHomePastFirstDowns) / awayTeamPastGames.Count();
                        htPastFirstDowns = (htAwayPastFirstDowns + htHomePastFirstDowns) / homeTeamPastGames.Count();
                    }
                    catch { }
                    #endregion

                    #region Third Down Prediction
                    var atAwayPastThirdDownAttempts = awayTeamPastGames.Where(x => x.AwayTeamId == game.AwayTeamId).Select(x => x.AwayTeamThirdDownAttempts).Sum();
                    var atHomePastThirdDownAttempts = awayTeamPastGames.Where(x => x.HomeTeamId == game.AwayTeamId).Select(x => x.HomeTeamThirdDownAttempts).Sum();
                    double atPastThirdDownAttempts = 0;
                    var atAwayPastThirdDownCompletions = awayTeamPastGames.Where(x => x.AwayTeamId == game.AwayTeamId).Select(x => x.AwayTeamThirdDownCompletions).Sum();
                    var atHomePastThirdDownCompletions = awayTeamPastGames.Where(x => x.HomeTeamId == game.AwayTeamId).Select(x => x.HomeTeamThirdDownCompletions).Sum();
                    double atPastThirdDownCompletions = 0;

                    var htAwayPastThirdDownAttempts = homeTeamPastGames.Where(x => x.AwayTeamId == game.AwayTeamId).Select(x => x.AwayTeamThirdDownAttempts).Sum();
                    var htHomePastThirdDownAttempts = homeTeamPastGames.Where(x => x.HomeTeamId == game.AwayTeamId).Select(x => x.HomeTeamThirdDownAttempts).Sum();
                    double htPastThirdDownAttempts = 0;
                    var htAwayPastThirdDownCompletions = homeTeamPastGames.Where(x => x.AwayTeamId == game.AwayTeamId).Select(x => x.AwayTeamThirdDownCompletions).Sum();
                    var htHomePastThirdDownCompletions = homeTeamPastGames.Where(x => x.HomeTeamId == game.AwayTeamId).Select(x => x.HomeTeamThirdDownCompletions).Sum();
                    double htPastThirdDownCompletions = 0;

                    var atOpponentAwayThirdDownAttempts = awayTeamAwayOpponents.Select(x => x.HomeTeamThirdDownAttempts).Sum();
                    var atOpponentHomeThirdDownAttempts = awayTeamAwayOpponents.Select(x => x.AwayTeamThirdDownAttempts).Sum();
                    double atOpponentThirdDownAttempts = 0;
                    var htOpponentAwayThirdDownAttempts = homeTeamAwayOpponents.Select(x => x.HomeTeamThirdDownAttempts).Sum();
                    var htOpponentHomeThirdDownAttempts = homeTeamAwayOpponents.Select(x => x.AwayTeamThirdDownAttempts).Sum();
                    double htOpponentThirdDownAttempts = 0;

                    var atOpponentAwayThirdDownCompletions = awayTeamAwayOpponents.Select(x => x.HomeTeamThirdDownCompletions).Sum();
                    var atOpponentHomeThirdDownCompletions = awayTeamAwayOpponents.Select(x => x.AwayTeamThirdDownCompletions).Sum();
                    double atOpponentThirdDownCompletions = 0;
                    var htOpponentAwayThirdDownCompletions = homeTeamAwayOpponents.Select(x => x.HomeTeamThirdDownCompletions).Sum();
                    var htOpponentHomeThirdDownCompletions = homeTeamAwayOpponents.Select(x => x.AwayTeamThirdDownCompletions).Sum();
                    double htOpponentThirdDownCompletions = 0;

                    try
                    {
                        atPastThirdDownAttempts = (atAwayPastThirdDownAttempts + atHomePastThirdDownAttempts) / awayTeamPastGames.Count();
                        atPastThirdDownCompletions = (atAwayPastThirdDownCompletions + atHomePastThirdDownCompletions) / awayTeamPastGames.Count();
                        htPastThirdDownAttempts = (htAwayPastThirdDownAttempts + htHomePastThirdDownAttempts) / homeTeamPastGames.Count();
                        htPastThirdDownCompletions = (htAwayPastThirdDownCompletions + htHomePastThirdDownCompletions) / homeTeamPastGames.Count();
                        htOpponentThirdDownAttempts = (htOpponentAwayThirdDownAttempts + htOpponentHomeThirdDownAttempts) / homeTeamPastGames.Count();
                        atOpponentThirdDownAttempts = (atOpponentAwayThirdDownAttempts + atOpponentHomeThirdDownAttempts) / awayTeamPastGames.Count();
                        atOpponentThirdDownCompletions = (atOpponentAwayThirdDownCompletions + atOpponentHomeThirdDownCompletions) / awayTeamPastGames.Count();
                        htOpponentThirdDownCompletions = (htOpponentAwayThirdDownCompletions + htOpponentHomeThirdDownCompletions) / homeTeamPastGames.Count();
                    }
                    catch { }


                    double atThirdDownPercentage = GetPercentage(game.AwayTeamThirdDownCompletions, game.AwayTeamThirdDownAttempts);
                    double htThirdDownPercentage = GetPercentage(game.HomeTeamThirdDownCompletions, game.HomeTeamThirdDownAttempts);
                    double atThirdDownPercentageAllowed = GetPercentage(atPastThirdDownCompletions, atPastThirdDownAttempts);
                    double htThirdDownPercentageAllowed = GetPercentage(htPastThirdDownCompletions, htPastThirdDownAttempts);
                    #endregion

                    #region Fourth Down Prediction
                    var atAwayPastFourthDownAttempts = awayTeamPastGames.Where(x => x.AwayTeamId == game.AwayTeamId).Select(x => x.AwayTeamFourthDownAttempts).Sum();
                    var atHomePastFourthDownAttempts = awayTeamPastGames.Where(x => x.HomeTeamId == game.AwayTeamId).Select(x => x.HomeTeamFourthDownAttempts).Sum();
                    double atPastFourthDownAttempts = 0;
                    var atAwayPastFourthDownCompletions = awayTeamPastGames.Where(x => x.AwayTeamId == game.AwayTeamId).Select(x => x.AwayTeamFourthDownCompletions).Sum();
                    var atHomePastFourthDownCompletions = awayTeamPastGames.Where(x => x.HomeTeamId == game.AwayTeamId).Select(x => x.HomeTeamFourthDownCompletions).Sum();
                    double atPastFourthDownCompletions = 0;

                    var htAwayPastFourthDownAttempts = homeTeamPastGames.Where(x => x.AwayTeamId == game.AwayTeamId).Select(x => x.AwayTeamFourthDownAttempts).Sum();
                    var htHomePastFourthDownAttempts = homeTeamPastGames.Where(x => x.HomeTeamId == game.AwayTeamId).Select(x => x.HomeTeamFourthDownAttempts).Sum();
                    double htPastFourthDownAttempts = 0;
                    var htAwayPastFourthDownCompletions = homeTeamPastGames.Where(x => x.AwayTeamId == game.AwayTeamId).Select(x => x.AwayTeamFourthDownCompletions).Sum();
                    var htHomePastFourthDownCompletions = homeTeamPastGames.Where(x => x.HomeTeamId == game.AwayTeamId).Select(x => x.HomeTeamFourthDownCompletions).Sum();
                    double htPastFourthDownCompletions = 0;

                    var atOpponentAwayFourthDownAttempts = awayTeamAwayOpponents.Select(x => x.HomeTeamFourthDownAttempts).Sum();
                    var atOpponentHomeFourthDownAttempts = awayTeamAwayOpponents.Select(x => x.AwayTeamFourthDownAttempts).Sum();
                    double atOpponentFourthDownAttempts = 0;
                    var htOpponentAwayFourthDownAttempts = homeTeamAwayOpponents.Select(x => x.HomeTeamFourthDownAttempts).Sum();
                    var htOpponentHomeFourthDownAttempts = homeTeamAwayOpponents.Select(x => x.AwayTeamFourthDownAttempts).Sum();
                    double htOpponentFourthDownAttempts = 0;

                    var atOpponentAwayFourthDownCompletions = awayTeamAwayOpponents.Select(x => x.HomeTeamFourthDownCompletions).Sum();
                    var atOpponentHomeFourthDownCompletions = awayTeamAwayOpponents.Select(x => x.AwayTeamFourthDownCompletions).Sum();
                    double atOpponentFourthDownCompletions = 0;
                    var htOpponentAwayFourthDownCompletions = homeTeamAwayOpponents.Select(x => x.HomeTeamFourthDownCompletions).Sum();
                    var htOpponentHomeFourthDownCompletions = homeTeamAwayOpponents.Select(x => x.AwayTeamFourthDownCompletions).Sum();
                    double htOpponentFourthDownCompletions = 0;

                    try
                    {
                        atPastFourthDownAttempts = (atAwayPastFourthDownAttempts + atHomePastFourthDownAttempts) / awayTeamPastGames.Count();
                        atPastFourthDownCompletions = (atAwayPastFourthDownCompletions + atHomePastFourthDownCompletions) / awayTeamPastGames.Count();
                        htPastFourthDownAttempts = (htAwayPastFourthDownAttempts + htHomePastFourthDownAttempts) / homeTeamPastGames.Count();
                        htPastFourthDownCompletions = (htAwayPastFourthDownCompletions + htHomePastFourthDownCompletions) / homeTeamPastGames.Count();
                        atOpponentFourthDownAttempts = (atOpponentAwayFourthDownAttempts + atOpponentHomeFourthDownAttempts) / awayTeamPastGames.Count();
                        htOpponentFourthDownAttempts = (htOpponentAwayFourthDownAttempts + htOpponentHomeFourthDownAttempts) / homeTeamPastGames.Count();
                        atOpponentFourthDownCompletions = (atOpponentAwayFourthDownCompletions + atOpponentHomeFourthDownCompletions) / awayTeamPastGames.Count();
                        htOpponentFourthDownCompletions = (htOpponentAwayFourthDownCompletions + htOpponentHomeFourthDownCompletions) / homeTeamPastGames.Count();
                    }
                    catch { }
                    double atFourthDownPercentage = GetPercentage(game.AwayTeamFourthDownCompletions, game.AwayTeamFourthDownAttempts);
                    double htFourthDownPercentage = GetPercentage(game.HomeTeamFourthDownCompletions, game.HomeTeamFourthDownAttempts);

                    double atFourthDownPercentageAllowed = GetPercentage(atPastFourthDownCompletions, atPastFourthDownAttempts);
                    double htFourthDownPercentageAllowed = GetPercentage(htPastFourthDownCompletions, htPastFourthDownAttempts);
                    #endregion

                    #region Passing Yards Prediction
                    var atOpponentAwayPassingAttempts = awayTeamAwayOpponents.Select(x => x.HomeTeamPassingAttempts).Sum();
                    var atOpponentHomePassingAttempts = awayTeamHomeOpponents.Select(x => x.AwayTeamPassingAttempts).Sum();
                    double atOpponentPassingAttempts = 0;
                    var atOpponentAwayPassingCompletions = awayTeamAwayOpponents.Select(x => x.HomeTeamPassingCompletions).Sum();
                    var atOpponentHomePassingCompletions = awayTeamHomeOpponents.Select(x => x.AwayTeamPassingCompletions).Sum();
                    double atOpponentPassingCompletions = 0;


                    var htOpponentAwayPassingAttempts = homeTeamAwayOpponents.Select(x => x.HomeTeamPassingAttempts).Sum();
                    var htOpponentHomePassingAttempts = homeTeamHomeOpponents.Select(x => x.AwayTeamPassingAttempts).Sum();
                    double htOpponentPassingAttempts = 0;
                    var htOpponentAwayPassingCompletions = homeTeamAwayOpponents.Select(x => x.HomeTeamPassingCompletions).Sum();
                    var htOpponentHomePassingCompletions = homeTeamHomeOpponents.Select(x => x.AwayTeamPassingCompletions).Sum();
                    double htOpponentPassingCompletions = 0;

                    try
                    {
                        atOpponentPassingAttempts = (atOpponentAwayPassingAttempts + atOpponentHomePassingAttempts) / awayTeamPastGames.Count();
                        atOpponentPassingCompletions = (atOpponentAwayPassingCompletions + atOpponentHomePassingCompletions) / awayTeamPastGames.Count();
                        htOpponentPassingAttempts = (htOpponentAwayPassingAttempts + htOpponentHomePassingAttempts) / homeTeamPastGames.Count();
                        htOpponentPassingCompletions = (htOpponentAwayPassingCompletions + htOpponentHomePassingCompletions) / homeTeamPastGames.Count();
                    }
                    catch
                    {

                    }
                    #endregion

                    #region Yards Per Pass Prediction

                    var atOpponentAwayYardsPerPass = awayTeamAwayOpponents.Select(x => x.HomeTeamYardsPerPass).Sum();
                    var atOpponentHomeYardsPerPass = awayTeamHomeOpponents.Select(x => x.AwayTeamYardsPerPass).Sum();
                    double atOpponentYardsPerPass = 0;
                    var htOpponentAwayYardsPerPass = homeTeamAwayOpponents.Select(x => x.HomeTeamYardsPerPass).Sum();
                    var htOpponentHomeYardsPerPass = homeTeamHomeOpponents.Select(x => x.AwayTeamYardsPerPass).Sum();
                    double htOpponentYardsPerPass = 0;

                    try
                    {
                        atOpponentYardsPerPass = (atOpponentAwayPassingAttempts + atOpponentHomePassingAttempts) / awayTeamPastGames.Count();
                        htOpponentYardsPerPass = (htOpponentAwayYardsPerPass + htOpponentHomeYardsPerPass) / homeTeamPastGames.Count();
                    }
                    catch { }
                    #endregion

                    #region Rushing Yards Prediction

                    var atOpponentAwayYardsPerRushAttempt = awayTeamAwayOpponents.Select(x => x.HomeTeamYardsPerRushAttempt).Sum();
                    var atOpponentHomeYardsPerRushAttempt = awayTeamHomeOpponents.Select(x => x.AwayTeamYardsPerRushAttempt).Sum();
                    double atOpponentYardsPerRushAttempt = 0;
                    var atOpponentAwayRushingAttempts = awayTeamAwayOpponents.Select(x => x.HomeTeamRushingAttempts).Sum();
                    var atOpponentHomeRushingAttempts = awayTeamHomeOpponents.Select(x => x.AwayTeamRushingAttempts).Sum();
                    double atOpponentRushingAttempts = 0;

                    var htOpponentAwayYardsPerRushAttempt = homeTeamAwayOpponents.Select(x => x.HomeTeamYardsPerRushAttempt).Sum();
                    var htOpponentHomeYardsPerRushAttempt = homeTeamHomeOpponents.Select(x => x.AwayTeamYardsPerRushAttempt).Sum();
                    double htOpponentYardsPerRushAttempt = 0;
                    var htOpponentAwayRushingAttempts = homeTeamAwayOpponents.Select(x => x.HomeTeamRushingAttempts).Sum();
                    var htOpponentHomeRushingAttempts = homeTeamHomeOpponents.Select(x => x.AwayTeamRushingAttempts).Sum();
                    double htOpponentRushingAttempts = 0;

                    try
                    {
                        atOpponentYardsPerRushAttempt = (atOpponentAwayYardsPerRushAttempt + atOpponentHomeYardsPerRushAttempt) / awayTeamPastGames.Count();
                        atOpponentRushingAttempts = (atOpponentAwayRushingAttempts + atOpponentHomeRushingAttempts) / awayTeamPastGames.Count();
                        htOpponentYardsPerRushAttempt = (htOpponentAwayYardsPerRushAttempt + htOpponentHomeYardsPerRushAttempt) / homeTeamPastGames.Count();
                        htOpponentRushingAttempts = (htOpponentAwayRushingAttempts + htOpponentHomeRushingAttempts) / homeTeamPastGames.Count();
                    }
                    catch { }
                    #endregion

                    #region Fumbles Prediction

                    var atOpponentAwayFumblesLost = awayTeamAwayOpponents.Select(x => x.HomeTeamFumblesLost).Sum();
                    var atOpponentHomeFumblesLost = awayTeamHomeOpponents.Select(x => x.AwayTeamFumblesLost).Sum();
                    double atOpponentFumblesLost = 0;
                    var atAwayFumbles = awayTeamPastGames.Where(x => x.AwayTeamId == game.AwayTeamId).Select(x => x.AwayTeamFumblesLost).Sum();
                    var atHomeFumbles = awayTeamPastGames.Where(x => x.HomeTeamId == game.AwayTeamId).Select(x => x.HomeTeamFumblesLost).Sum();
                    double atFumbles = 0;


                    var htOpponentAwayFumblesLost = homeTeamAwayOpponents.Select(x => x.HomeTeamFumblesLost).Sum();
                    var htOpponentHomeFumblesLost = homeTeamHomeOpponents.Select(x => x.AwayTeamFumblesLost).Sum();
                    double htOpponentFumblesLost = 0;
                    var htAwayFumbles = homeTeamPastGames.Where(x => x.AwayTeamId == game.HomeTeamId).Select(x => x.AwayTeamFumblesLost).Sum();
                    var htHomeFumbles = homeTeamPastGames.Where(x => x.HomeTeamId == game.HomeTeamId).Select(x => x.HomeTeamFumblesLost).Sum();
                    double htFumbles = 0;

                    try
                    {
                        atOpponentFumblesLost = (atOpponentAwayFumblesLost + atOpponentHomeFumblesLost) / awayTeamPastGames.Count();
                        atFumbles = (atAwayFumbles + atHomeFumbles) / awayTeamPastGames.Count();
                        htOpponentFumblesLost = (htOpponentAwayFumblesLost + htOpponentHomeFumblesLost) / homeTeamPastGames.Count();
                        htFumbles = (htAwayFumbles + htHomeFumbles) / homeTeamPastGames.Count();
                    }
                    catch { }
                    #endregion

                    #region Interceptions Prediction

                    var atOpponentAwayInterceptionsLost = awayTeamAwayOpponents.Select(x => x.HomeTeamInterceptions).Sum();
                    var atOpponentHomeInterceptionsLost = awayTeamHomeOpponents.Select(x => x.AwayTeamInterceptions).Sum();
                    double atOpponentInterceptionsLost = 0;
                    var atAwayInterceptions = awayTeamPastGames.Where(x => x.AwayTeamId == game.AwayTeamId).Select(x => x.AwayTeamFumblesLost).Sum();
                    var atHomeInterceptions = awayTeamPastGames.Where(x => x.HomeTeamId == game.AwayTeamId).Select(x => x.HomeTeamFumblesLost).Sum();
                    double atInterceptions = 0;

                    var htOpponentAwayInterceptionsLost = homeTeamAwayOpponents.Select(x => x.HomeTeamInterceptions).Sum();
                    var htOpponentHomeInterceptionsLost = homeTeamHomeOpponents.Select(x => x.AwayTeamInterceptions).Sum();
                    double htOpponentInterceptionsLost = 0;
                    var htAwayInterceptions = homeTeamPastGames.Where(x => x.AwayTeamId == game.HomeTeamId).Select(x => x.AwayTeamFumblesLost).Sum();
                    var htHomeInterceptions = homeTeamPastGames.Where(x => x.HomeTeamId == game.HomeTeamId).Select(x => x.HomeTeamFumblesLost).Sum();
                    double htInterceptions = 0;

                    try
                    {
                        atOpponentInterceptionsLost = (atOpponentAwayInterceptionsLost + atOpponentHomeInterceptionsLost) / awayTeamPastGames.Count();
                        atInterceptions = (atAwayInterceptions + atHomeInterceptions) / awayTeamPastGames.Count();
                        htOpponentInterceptionsLost = (htOpponentAwayInterceptionsLost + htOpponentHomeInterceptionsLost) / homeTeamPastGames.Count();
                        htInterceptions = (htAwayInterceptions + htHomeInterceptions) / homeTeamPastGames.Count();
                    }
                    catch
                    {

                    }

                    #endregion

                    #region Nest Region Getting Past Yards

                    var atAwayRushingYardsAllowed = awayTeamAwayOpponents.Select(x => x.HomeTeamRushingYards).Sum();
                    var atHomeRushingYardsAllowed = awayTeamHomeOpponents.Select(x => x.AwayTeamRushingYards).Sum();
                    double atRushingYardsAllowed = 0.0;
                    var htAwayRushingYardsAllowed = homeTeamAwayOpponents.Select(x => x.HomeTeamRushingYards).Sum();
                    var htHomeRushingYardsAllowed = homeTeamHomeOpponents.Select(x => x.AwayTeamRushingYards).Sum();
                    double htRushingYardsAllowed = 0.0;

                    var atAwayNetPassingYardsAllowed = awayTeamAwayOpponents.Select(x => x.HomeTeamNetPassingYards).Sum();
                    var atHomeNetPassingYardsAllowed = awayTeamHomeOpponents.Select(x => x.AwayTeamNetPassingYards).Sum();
                    double atNetPassingYardsAllowed = 0.0;
                    var htAwayNetPassingYardsAllowed = homeTeamAwayOpponents.Select(x => x.HomeTeamNetPassingYards).Sum();
                    var htHomeNetPassingYardsAllowed = homeTeamHomeOpponents.Select(x => x.AwayTeamNetPassingYards).Sum();
                    double htNetPassingYardsAllowed = 0.0;

                    var atAwayYardsPerPassAllowed = awayTeamAwayOpponents.Select(x => x.HomeTeamYardsPerPass).Sum();
                    var atHomeYardsPerPassAllowed = awayTeamHomeOpponents.Select(x => x.AwayTeamYardsPerPass).Sum();
                    double atYardsPerPassAllowed = 0.0;
                    var htAwayYardsPerPassAllowed = homeTeamAwayOpponents.Select(x => x.HomeTeamYardsPerPass).Sum();
                    var htHomeYardsPerPassAllowed = homeTeamHomeOpponents.Select(x => x.AwayTeamYardsPerPass).Sum();
                    double htYardsPerPassAllowed = 0.0;

                    var atAwayYardsPerRushAllowed = awayTeamAwayOpponents.Select(x => x.HomeTeamYardsPerRushAttempt).Sum();
                    var atHomeYardsPerRushAllowed = awayTeamHomeOpponents.Select(x => x.AwayTeamYardsPerRushAttempt).Sum();
                    double atYardsPerRushAllowed = 0.0;
                    var htAwayYardsPerRushAllowed = homeTeamAwayOpponents.Select(x => x.HomeTeamYardsPerRushAttempt).Sum();
                    var htHomeYardsPerRushAllowed = homeTeamHomeOpponents.Select(x => x.AwayTeamYardsPerRushAttempt).Sum();
                    double htYardsPerRushAllowed = 0.0;

                    try
                    {
                        atRushingYardsAllowed = (atAwayRushingYardsAllowed + atHomeRushingYardsAllowed) / awayTeamPastGames.Count();
                        htRushingYardsAllowed = (htAwayRushingYardsAllowed + htHomeRushingYardsAllowed) / homeTeamPastGames.Count();
                        atNetPassingYardsAllowed = (atAwayNetPassingYardsAllowed + atHomeNetPassingYardsAllowed) / awayTeamPastGames.Count();
                        htNetPassingYardsAllowed = (htAwayNetPassingYardsAllowed + htHomeNetPassingYardsAllowed) / homeTeamPastGames.Count();
                        atYardsPerPassAllowed = (atAwayYardsPerPassAllowed + atHomeYardsPerPassAllowed) / awayTeamPastGames.Count();
                        htYardsPerPassAllowed = (htAwayYardsPerPassAllowed + htHomeYardsPerPassAllowed) / homeTeamPastGames.Count();
                        atYardsPerRushAllowed = (atAwayYardsPerRushAllowed + atHomeYardsPerRushAllowed) / awayTeamPastGames.Count();
                        htYardsPerRushAllowed = (htAwayYardsPerRushAllowed + htHomeYardsPerRushAllowed) / homeTeamPastGames.Count();
                    }
                    catch
                    {

                    }

                    double atTotalYardsAllowed = atRushingYardsAllowed + atNetPassingYardsAllowed;
                    double htTotalYardsAllowed = htRushingYardsAllowed + htNetPassingYardsAllowed;
                    #endregion

                    #region Nest Region Getting Past Penalties

                    var atAwayPenalties = awayTeamAwayOpponents.Select(x => x.AwayTeamTotalPenalties).Sum();
                    var atHomePenalties = awayTeamHomeOpponents.Select(x => x.HomeTeamTotalPenalties).Sum();
                    double atTotalPenalties = 1;
                    var htAwayPenalties = homeTeamAwayOpponents.Select(x => x.AwayTeamTotalPenalties).Sum();
                    var htHomePenalties = homeTeamHomeOpponents.Select(x => x.HomeTeamTotalPenalties).Sum();
                    double htTotalPenalties = 1;


                    var atAwayPenaltyYards = awayTeamAwayOpponents.Select(x => x.AwayTeamTotalPenaltyYards).Sum();
                    var atHomePenaltyYards = awayTeamHomeOpponents.Select(x => x.HomeTeamTotalPenaltyYards).Sum();
                    double atTotalPenaltyYards = 1;
                    var htAwayPenaltyYards = homeTeamAwayOpponents.Select(x => x.AwayTeamTotalPenaltyYards).Sum();
                    var htHomePenaltyYards = homeTeamHomeOpponents.Select(x => x.HomeTeamTotalPenaltyYards).Sum();
                    double htTotalPenaltyYards = 1;

                    try
                    {
                        atTotalPenalties = (atAwayPenalties + atHomePenalties) / awayTeamPastGames.Count();
                        htTotalPenalties = (htAwayPenalties + htHomePenalties) / homeTeamPastGames.Count();
                        atTotalPenaltyYards = (atAwayPenaltyYards + atHomePenaltyYards) / awayTeamPastGames.Count();
                        htTotalPenaltyYards = (htAwayPenaltyYards + htHomePenaltyYards) / homeTeamPastGames.Count();
                    }
                    catch
                    {

                    }
                    #endregion

                    #region Nest Region Getting Past Possession Time And Plays

                    var atAwayPossession = awayTeamAwayOpponents.Select(x => x.AwayTeamPossessionTime).Sum();
                    var atHomePossession = awayTeamHomeOpponents.Select(x => x.HomeTeamPossessionTime).Sum();
                    double atAveragePossession = 0;

                    var atAwayPositivePlays = awayTeamAwayOpponents.Select(x => x.AwayTeamPositivePlays).Sum();
                    var atHomePositivePlays = awayTeamHomeOpponents.Select(x => x.HomeTeamPositivePlays).Sum();
                    double atAveragePositivePlays = 0;

                    var atAwayNegativePlays = awayTeamAwayOpponents.Select(x => x.AwayTeamNegativePlays).Sum();
                    var atHomeNegativePlays = awayTeamHomeOpponents.Select(x => x.HomeTeamNegativePlays).Sum();
                    double atAverageNegativePlays = 0;

                    var htAwayPossession = homeTeamAwayOpponents.Select(x => x.AwayTeamPossessionTime).Sum();
                    var htHomePossession = homeTeamHomeOpponents.Select(x => x.HomeTeamPossessionTime).Sum();
                    double htAveragePossession = 0;

                    var htAwayPositivePlays = homeTeamAwayOpponents.Select(x => x.AwayTeamPositivePlays).Sum();
                    var htHomePositivePlays = homeTeamHomeOpponents.Select(x => x.HomeTeamPositivePlays).Sum();
                    double htAveragePositivePlays = 0;

                    var htAwayNegativePlays = homeTeamAwayOpponents.Select(x => x.AwayTeamNegativePlays).Sum();
                    var htHomeNegativePlays = homeTeamHomeOpponents.Select(x => x.HomeTeamNegativePlays).Sum();
                    double htAverageNegativePlays = 0;

                    try
                    {
                        atAveragePossession = (atAwayPossession + atHomePossession) / awayTeamPastGames.Count();
                        atAveragePositivePlays = (atAwayPositivePlays + atHomePositivePlays) / awayTeamPastGames.Count();
                        atAverageNegativePlays = (atAwayNegativePlays + atHomeNegativePlays) / awayTeamPastGames.Count();

                        htAveragePossession = (htAwayPossession + htHomePossession) / homeTeamPastGames.Count();
                        htAveragePositivePlays = (htAwayPositivePlays + htHomePositivePlays) / homeTeamPastGames.Count();
                        htAverageNegativePlays = (htAwayNegativePlays + htHomeNegativePlays) / homeTeamPastGames.Count();
                    }
                    catch
                    {

                    }
                    #endregion

                    #region Conference and Wins

                    var awayTeamConference = 100;
                    var homeTeamConference = 100;
                    try
                    {
                        var aTeamConference = await (from c in db.ConferenceTeam
                                                     where c.TeamId == game.AwayTeamId
                                                     select c).FirstOrDefaultAsync();
                        if (aTeamConference != null)
                        {
                            awayTeamConference = aTeamConference.TeamConference;
                        }
                    }
                    catch { }
                    try
                    {

                        var hTeamConference = await (from c in db.ConferenceTeam
                                                     where c.TeamId == game.HomeTeamId
                                                     select c).FirstOrDefaultAsync();

                        if (hTeamConference != null)
                        {
                            homeTeamConference = hTeamConference.TeamConference;
                        }
                    }
                    catch { }

                    var awayAwayWins = awayTeamAwayOpponents.Where(x => x.AwayTeamWon).Count();
                    var awayHomeWins = awayTeamHomeOpponents.Where(x => x.HomeTeamWon).Count();
                    var awayWins = awayAwayWins + awayHomeWins;

                    var homeAwayWins = homeTeamAwayOpponents.Where(x => x.AwayTeamWon).Count();
                    var homeHomeWins = homeTeamHomeOpponents.Where(x => x.HomeTeamWon).Count();
                    var homeWins = homeAwayWins + homeHomeWins;

                    var awayAwayLosses = awayTeamAwayOpponents.Where(x => !x.AwayTeamWon).Count();
                    var awayHomeLosses = awayTeamHomeOpponents.Where(x => !x.HomeTeamWon).Count();
                    var awayLosses = awayAwayLosses + awayHomeLosses;

                    var homeAwayLosses = homeTeamAwayOpponents.Where(x => !x.AwayTeamWon).Count();
                    var homeHomeLosses = homeTeamHomeOpponents.Where(x => !x.HomeTeamWon).Count();
                    var homeLosses = homeAwayLosses + homeHomeLosses;
                    #endregion

                    #region Newest Stats

                    double awayTeamPointsPerPlay = 0;
                    double homeTeamPointsPerPlay = 0;

                    double awayTeamTurnoverMargin = 0;
                    double homeTeamTurnoverMargin = 0;

                    double awayTeamFieldGoalsAllowed = 0;
                    var atAwayFieldGoalsAllowed = awayTeamAwayOpponents.Select(x => x.HomeTeamFieldGoals).Sum();
                    var atHomeFieldGoalsAllowed = awayTeamHomeOpponents.Select(x => x.AwayTeamFieldGoals).Sum();
                    double homeTeamFieldGoalsAllowed = 0;
                    var htAwayFieldGoalsAllowed = homeTeamAwayOpponents.Select(x => x.HomeTeamFieldGoals).Sum();
                    var htHomeFieldGoalsAllowed = homeTeamHomeOpponents.Select(x => x.AwayTeamFieldGoals).Sum();

                    double awayTeamSpecialTeamsPointsAllowed = 0;
                    var atAwayTeamSpecialTeamsPointsAllowed = awayTeamAwayOpponents.Select(x => x.HomeTeamSpecialTeamsPoints).Sum();
                    var atHomeTeamSpecialTeamsPointsAllowed = awayTeamHomeOpponents.Select(x => x.AwayTeamSpecialTeamsPoints).Sum();
                    double homeTeamSpecialTeamsPointsAllowed = 0;
                    var htAwayTeamSpecialTeamsPointsAllowed = homeTeamAwayOpponents.Select(x => x.HomeTeamSpecialTeamsPoints).Sum();
                    var htHomeTeamSpecialTeamsPointsAllowed = homeTeamHomeOpponents.Select(x => x.AwayTeamSpecialTeamsPoints).Sum();

                    try
                    {
                        if(game.AwayTeamScore > 0 && game.AwayTeamPositivePlays > 0 && game.AwayTeamNegativePlays > 0)
                        {
                            awayTeamPointsPerPlay = Math.Round(game.AwayTeamScore / (game.AwayTeamPositivePlays + game.AwayTeamNegativePlays), 4);
                        }
                        if (game.HomeTeamScore > 0 && game.HomeTeamPositivePlays > 0 && game.HomeTeamNegativePlays > 0)
                        {
                            homeTeamPointsPerPlay = game.HomeTeamScore / (game.HomeTeamPositivePlays + game.HomeTeamNegativePlays);
                        }

                        //Console.WriteLine("------------------------------!!!!!!!!!!!!!!!!!!!!!!!!!!------------------------------");
                        //Console.WriteLine(awayTeamPointsPerPlay);
                        //Console.WriteLine(homeTeamPointsPerPlay);
                        //Console.WriteLine("------------------------------!!!!!!!!!!!!!!!!!!!!!!!!!!------------------------------");

                        awayTeamTurnoverMargin = (game.HomeTeamFumblesLost + game.HomeTeamInterceptions) - (game.AwayTeamFumblesLost + game.AwayTeamInterceptions);
                        homeTeamTurnoverMargin = (game.AwayTeamFumblesLost + game.AwayTeamInterceptions) - (game.HomeTeamFumblesLost + game.HomeTeamInterceptions);

                        awayTeamFieldGoalsAllowed = (atAwayFieldGoalsAllowed + atHomeFieldGoalsAllowed) / awayTeamPastGames.Count();
                        homeTeamFieldGoalsAllowed = (htAwayFieldGoalsAllowed + htHomeFieldGoalsAllowed) / homeTeamPastGames.Count();

                        awayTeamSpecialTeamsPointsAllowed = (atAwayTeamSpecialTeamsPointsAllowed + atHomeTeamSpecialTeamsPointsAllowed) / awayTeamPastGames.Count();
                        homeTeamSpecialTeamsPointsAllowed = (htAwayTeamSpecialTeamsPointsAllowed + htHomeTeamSpecialTeamsPointsAllowed) / homeTeamPastGames.Count();
                    }
                    catch
                    {

                    }
                    #endregion

                    #region OffensiveModel
                    var awayTeamOffensiveModel = new OffensiveModelDbo
                    {
                        EventId = game.EventId,
                        IsConference = game.IsConference,
                        IsNeutralSite = game.IsNeutralSite,
                        Week = (int)week!,
                        Year = year,
                        TeamId = game.AwayTeamId,
                        TeamWins = awayWins,
                        TeamLosses = awayLosses,
                        TeamScore = (float)Math.Round(game.AwayTeamScore),
                        TeamWon = atWon,
                        TeamConference = awayTeamConference,
                        TeamChanceToWin = (float)Math.Round((game.AwayTeamChanceToWin.HasValue ? game.AwayTeamChanceToWin.Value : 0.0)),
                        TeamFirstDowns = (float)Math.Round(game.AwayTeamFirstDowns),
                        TeamTotalYards = (float)Math.Round(game.AwayTeamTotalYards),
                        TeamNetPassingYards = (float)Math.Round(game.AwayTeamNetPassingYards),
                        TeamYardsPerPass = (float)Math.Round(game.AwayTeamYardsPerPass),
                        TeamPassingAttempts = (float)Math.Round(game.AwayTeamPassingAttempts),
                        TeamRushingYards = (float)Math.Round(game.AwayTeamRushingYards),
                        TeamRushingAttempts = (float)Math.Round(game.AwayTeamRushingAttempts),
                        TeamYardsPerRushAttempt = (float)Math.Round(game.AwayTeamYardsPerRushAttempt),
                        TeamTotalPenalties = (float)Math.Round(game.AwayTeamTotalPenalties),
                        TeamTotalPenaltyYards = (float)Math.Round(game.AwayTeamTotalPenaltyYards),
                        TeamFumblesLost = (float)Math.Round(game.AwayTeamFumblesLost),
                        TeamInterceptions = (float)Math.Round(game.AwayTeamInterceptions),
                        TeamPossessionTime = (float)Math.Round(game.AwayTeamPossessionTime),
                        TeamPositivePlays = (float)Math.Round(game.AwayTeamPositivePlays),
                        TeamNegativePlays = (float)Math.Round(game.AwayTeamNegativePlays),
                        TeamDefensiveTacklesForLoss = (float)Math.Round(game.AwayTeamDefensiveTacklesForLoss),
                        TeamDefensiveSacks = (float)Math.Round(game.AwayTeamDefensiveSacks),
                        TeamFieldGoals = (float)Math.Round(game.AwayTeamFieldGoals),
                        TeamDefensiveSafety = (float)Math.Round(game.AwayTeamDefensiveSafety),
                        TeamSpecialTeamsPoints = (float)Math.Round(game.AwayTeamSpecialTeamsPoints),
                        TeamPointsPerPlay = (float)Math.Round(awayTeamPointsPerPlay),
                        TeamTurnoverMargin = (float)Math.Round(awayTeamTurnoverMargin),

                        OpponentTeamId = game.HomeTeamId,
                        OpponentWins = homeWins,
                        OpponentLosses = homeLosses,
                        OpponentConference = homeTeamConference,
                        OpponentScore = (float)Math.Round(game.HomeTeamScore),
                        OpponentFirstDownsAllowed = (float)Math.Round(htPastFirstDowns),
                        OpponentTotalYardsAllowed = (float)Math.Round(htTotalYardsAllowed),
                        OpponentPassingYardsAllowed = (float)Math.Round(Math.Round((htNetPassingYardsAllowed), 4)),
                        OpponentYardsPerPassAllowed = (float)Math.Round(htYardsPerPassAllowed),
                        OpponentYardsPerRushAttemptAllowed = (float)Math.Round(htYardsPerRushAllowed),
                        OpponentRushingYardsAllowed = (float)Math.Round(htRushingYardsAllowed),
                        OpponentTotalPenalties = (float)Math.Round(htTotalPenalties),
                        OpponentTotalPenaltyYards = (float)Math.Round(htTotalPenaltyYards),
                        OpponentFumblesForced = (float)Math.Round(htOpponentFumblesLost),
                        OpponentInterceptionsForced = (float)Math.Round(htOpponentInterceptionsLost),
                        OpponentPossessionTime = (float)Math.Round(htAveragePossession),
                        OpponentPositivePlays = (float)Math.Round(htAveragePositivePlays),
                        OpponentNegativePlays = (float)Math.Round(htAverageNegativePlays),
                        OpponentDefensiveTacklesForLoss = (float)Math.Round(game.HomeTeamDefensiveTacklesForLoss),
                        OpponentDefensiveSacks = (float)Math.Round(game.HomeTeamDefensiveSacks),
                        OpponentFieldGoalsAllowed = (float)Math.Round(homeTeamFieldGoalsAllowed),
                        OpponentDefensiveSafety = (float)Math.Round(game.HomeTeamDefensiveSafety),
                        OpponentSpecialTeamsPointsAllowed = (float)Math.Round(homeTeamSpecialTeamsPointsAllowed),
                        OpponentPointsPerPlay = (float)Math.Round(homeTeamPointsPerPlay),
                        OpponentTurnoverMargin = (float)Math.Round(homeTeamTurnoverMargin),
                        CreatedDate = DateTime.UtcNow
                    };

                    var homeTeamOffensiveModel = new OffensiveModelDbo
                    {
                        EventId = game.EventId,
                        IsConference = game.IsConference,
                        IsNeutralSite = game.IsNeutralSite,
                        Week = (int)week!,
                        Year = year,
                        TeamId = game.HomeTeamId,
                        TeamWins = homeWins,
                        TeamLosses = homeLosses,
                        TeamScore = (float)Math.Round(game.HomeTeamScore),
                        TeamWon = htWon,
                        TeamConference = (int)homeTeamConference,
                        TeamChanceToWin = (float)Math.Round((game.HomeTeamChanceToWin.HasValue ? game.HomeTeamChanceToWin.Value : 0.0)),
                        TeamFirstDowns = (float)Math.Round(game.HomeTeamFirstDowns),
                        TeamTotalYards = (float)Math.Round(game.HomeTeamTotalYards),
                        TeamNetPassingYards = (float)Math.Round(game.HomeTeamNetPassingYards),
                        TeamYardsPerPass = (float)Math.Round(game.HomeTeamYardsPerPass),
                        TeamPassingAttempts = (float)Math.Round(game.HomeTeamPassingAttempts),
                        TeamRushingYards = (float)Math.Round(game.HomeTeamRushingYards),
                        TeamRushingAttempts = (float)Math.Round(game.HomeTeamRushingAttempts),
                        TeamYardsPerRushAttempt = (float)Math.Round(game.HomeTeamYardsPerRushAttempt),
                        TeamTotalPenalties = (float)Math.Round(game.HomeTeamTotalPenalties),
                        TeamTotalPenaltyYards = (float)Math.Round(game.HomeTeamTotalPenaltyYards),
                        TeamFumblesLost = (float)Math.Round(game.HomeTeamFumblesLost),
                        TeamInterceptions = (float)Math.Round(game.HomeTeamInterceptions),
                        TeamPossessionTime = (float)Math.Round(game.HomeTeamPossessionTime),
                        TeamPositivePlays = (float)Math.Round(game.HomeTeamPositivePlays),
                        TeamNegativePlays = (float)Math.Round(game.HomeTeamNegativePlays),
                        TeamDefensiveTacklesForLoss = (float)Math.Round(game.HomeTeamDefensiveTacklesForLoss),
                        TeamDefensiveSacks = (float)Math.Round(game.HomeTeamDefensiveSacks),
                        TeamFieldGoals = (float)Math.Round(game.HomeTeamFieldGoals),
                        TeamDefensiveSafety = (float)Math.Round(game.HomeTeamDefensiveSafety),
                        TeamSpecialTeamsPoints = (float)Math.Round(game.HomeTeamSpecialTeamsPoints),
                        TeamPointsPerPlay = (float)Math.Round(homeTeamPointsPerPlay),
                        TeamTurnoverMargin = (float)Math.Round(homeTeamTurnoverMargin),

                        OpponentTeamId = game.AwayTeamId,
                        OpponentWins = awayWins,
                        OpponentLosses = awayLosses,
                        OpponentConference = awayTeamConference,
                        OpponentScore = (float)Math.Round(game.AwayTeamScore),
                        OpponentFirstDownsAllowed = (float)Math.Round(atPastFirstDowns),
                        OpponentTotalYardsAllowed = (float)Math.Round(atTotalYardsAllowed),
                        OpponentPassingYardsAllowed = (float)Math.Round(atNetPassingYardsAllowed),
                        OpponentYardsPerPassAllowed = (float)Math.Round(atYardsPerPassAllowed),
                        OpponentYardsPerRushAttemptAllowed = (float)Math.Round(atYardsPerRushAllowed),
                        OpponentRushingYardsAllowed = (float)Math.Round(atRushingYardsAllowed),
                        OpponentTotalPenalties = (float)Math.Round(atTotalPenalties),
                        OpponentTotalPenaltyYards = (float)Math.Round(atTotalPenaltyYards),
                        OpponentFumblesForced = (float)Math.Round(atOpponentFumblesLost),
                        OpponentInterceptionsForced = (float)Math.Round(atOpponentInterceptionsLost),
                        OpponentPossessionTime = (float)Math.Round(atAveragePossession),
                        OpponentPositivePlays = (float)Math.Round(atAveragePositivePlays),
                        OpponentNegativePlays = (float)Math.Round(atAverageNegativePlays),
                        OpponentDefensiveTacklesForLoss = (float)Math.Round(game.AwayTeamDefensiveTacklesForLoss),
                        OpponentDefensiveSacks = (float)Math.Round(game.AwayTeamDefensiveSacks),
                        OpponentFieldGoalsAllowed = (float)Math.Round(awayTeamFieldGoalsAllowed),
                        OpponentDefensiveSafety = (float)Math.Round(game.AwayTeamDefensiveSafety),
                        OpponentSpecialTeamsPointsAllowed = (float)Math.Round(awayTeamSpecialTeamsPointsAllowed),
                        OpponentPointsPerPlay = (float)Math.Round(awayTeamPointsPerPlay),
                        OpponentTurnoverMargin = (float)Math.Round(awayTeamTurnoverMargin),
                        CreatedDate = DateTime.UtcNow
                    };

                    await db.OffensiveModel.AddAsync(awayTeamOffensiveModel);
                    await db.OffensiveModel.AddAsync(homeTeamOffensiveModel);
                    #endregion

                    try
                    {
                        await db.SaveChangesAsync();
                    }
                    catch(SqlException ex)
                    {
                        Console.WriteLine(game.AwayTeamName + " at " + game.HomeTeamName + " failed to save.");
                        Console.WriteLine(ex);
                    }
                }

                Console.WriteLine("Finished Saving Year : " + year);
            }
        }

        private double GetPercentage(double completions, double attempts)
        {
            if (attempts > 0 && completions > 0)
            {
                return completions / attempts;
            }
            else if (attempts > 0 && completions == 0)
            {
                return 0;
            }
            else
            {
                return 100;
            }
        }
    }
}
