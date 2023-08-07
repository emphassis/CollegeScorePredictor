using CollegeScorePredictor;
using CollegeScorePredictor.Models.Database;
using CollegeScorePredictor.Operations;
using Microsoft.EntityFrameworkCore;

namespace CollegeScorePredictor.AIModels.OverPrediction
{
    public class OverPredictionConferenceService
    {
        private IDbContextFactory<AppDbContext> factory;
        public OverPredictionConferenceService(IDbContextFactory<AppDbContext> _factory)
        {
            factory = _factory;
        }

        public async Task<PredictGameResponseModel> PredictConferenceGame(PredictGameRequestModel requestModel)
        {
            return null;
        //{
        //    using (var db = await factory.CreateDbContextAsync())
        //    {
        //        var games = await (from e in db.GameResult
        //                           where e.Year == requestModel.Year &&
        //                           e.HomeTeamId == requestModel.HomeTeamId || e.AwayTeamId == requestModel.HomeTeamId ||
        //                           e.HomeTeamId == requestModel.AwayTeamId || e.AwayTeamId == requestModel.AwayTeamId
        //                           select e).ToListAsync();

        //        var homeTeam = await (from e in db.ConferenceTeam
        //                              where e.TeamId == requestModel.HomeTeamId
        //                              select e).FirstAsync();

        //        var awayTeam = await (from e in db.ConferenceTeam
        //                              where e.TeamId == requestModel.AwayTeamId
        //                              select e).FirstAsync();

        //        List<GameResultDbo> gameStats = new List<GameResultDbo>();

        //        var lastYear = requestModel.Year - 1;
        //        if (requestModel.Week == 1)
        //        {
        //            gameStats = await (from e in db.GameResult
        //                               where e.Year == lastYear && e.Week >= 13
        //                               select e).ToListAsync();
        //        }
        //        else if (requestModel.Week == 2)
        //        {
        //            var gameStatsLastYear = await (from e in db.GameResult
        //                                           where e.Year == lastYear && e.Week >= 14
        //                                           select e).ToListAsync();
        //            var gameStatsThisYear = await (from e in db.GameResult
        //                                           where e.Year == requestModel.Year && e.Week < 2
        //                                           select e).ToListAsync();

        //            gameStats.AddRange(gameStatsLastYear);
        //            gameStats.AddRange(gameStatsThisYear);
        //        }
        //        else if (requestModel.Week == 3)
        //        {
        //            var gameStatsLastYear = await (from e in db.GameResult
        //                                           where e.Year == lastYear && e.Week >= 15
        //                                           select e).ToListAsync();
        //            var gameStatsThisYear = await (from e in db.GameResult
        //                                           where e.Year == requestModel.Year && e.Week < 3
        //                                           select e).ToListAsync();

        //            gameStats.AddRange(gameStatsLastYear);
        //            gameStats.AddRange(gameStatsThisYear);
        //        }
        //        else
        //        {
        //            gameStats = await (from e in db.GameResult
        //                               where e.Year == requestModel.Year && e.Week <= requestModel.Week
        //                               select e).ToListAsync();
        //        }


        //        var awayTeamPastGames = gameStats.Where(x => x.AwayTeamId == requestModel.AwayTeamId || x.HomeTeamId == requestModel.AwayTeamId);
        //        var homeTeamPastGames = gameStats.Where(x => x.AwayTeamId == requestModel.HomeTeamId || x.HomeTeamId == requestModel.HomeTeamId);

        //        var awayTeamAwayOpponents = awayTeamPastGames.Where(x => x.AwayTeamId == requestModel.AwayTeamId).Select(x => x).ToList();
        //        var awayTeamHomeOpponents = awayTeamPastGames.Where(x => x.HomeTeamId == requestModel.AwayTeamId).Select(x => x).ToList();

        //        var homeTeamAwayOpponents = homeTeamPastGames.Where(x => x.AwayTeamId == requestModel.HomeTeamId).Select(x => x).ToList();
        //        var homeTeamHomeOpponents = homeTeamPastGames.Where(x => x.HomeTeamId == requestModel.HomeTeamId).Select(x => x).ToList();

        //        #region First Down Prediction
        //        var atAwayPastFirstDowns = awayTeamAwayOpponents.Select(x => x.AwayTeamFirstDowns).Sum();
        //        var atHomePastFirstDowns = awayTeamHomeOpponents.Select(x => x.HomeTeamFirstDowns).Sum();
        //        double atPastFirstDowns = 0;

        //        var htAwayPastFirstDowns = homeTeamAwayOpponents.Select(x => x.AwayTeamFirstDowns).Sum();
        //        var htHomePastFirstDowns = homeTeamHomeOpponents.Select(x => x.HomeTeamFirstDowns).Sum();
        //        double htPastFirstDowns = 0;

        //        try
        //        {
        //            atPastFirstDowns = (atAwayPastFirstDowns + atHomePastFirstDowns) / awayTeamPastGames.Count();
        //            htPastFirstDowns = (htAwayPastFirstDowns + htHomePastFirstDowns) / homeTeamPastGames.Count();
        //        }
        //        catch { }

        //        var awayTeamFirstDownPrediction = new FirstDownModel.ModelInput()
        //        {
        //            TeamOneId = requestModel.AwayTeamId,
        //            TeamOnePastFirstDowns = (float)atPastFirstDowns,
        //            TeamTwoId = requestModel.HomeTeamId,
        //            TeamTwoFirstDownsAllowed = (float)htPastFirstDowns,
        //            Week = requestModel.Week,
        //            Year = requestModel.Year,
        //        };

        //        var homeTeamFirstDownPrediction = new FirstDownModel.ModelInput()
        //        {
        //            TeamOneId = requestModel.HomeTeamId,
        //            TeamOnePastFirstDowns = (float)htPastFirstDowns,
        //            TeamTwoId = requestModel.AwayTeamId,
        //            TeamTwoFirstDownsAllowed = (float)atPastFirstDowns,
        //            Week = requestModel.Week,
        //            Year = requestModel.Year,
        //        };

        //        //Load model and predict output
        //        var awayTeamFirstDowns = FirstDownModel.Predict(awayTeamFirstDownPrediction);
        //        var homeTeamFirstDowns = FirstDownModel.Predict(homeTeamFirstDownPrediction);
        //        #endregion

        //        #region Third Down Prediction
        //        var atAwayPastThirdDownAttempts = awayTeamAwayOpponents.Select(x => x.AwayTeamThirdDownAttempts).Sum();
        //        var atHomePastThirdDownAttempts = awayTeamHomeOpponents.Select(x => x.HomeTeamThirdDownAttempts).Sum();
        //        double atPastThirdDownAttempts = 0;
        //        var atAwayPastThirdDownCompletions = awayTeamAwayOpponents.Select(x => x.AwayTeamThirdDownCompletions).Sum();
        //        var atHomePastThirdDownCompletions = awayTeamHomeOpponents.Select(x => x.HomeTeamThirdDownCompletions).Sum();
        //        double atPastThirdDownCompletions = 0;

        //        var htAwayPastThirdDownAttempts = homeTeamAwayOpponents.Select(x => x.AwayTeamThirdDownAttempts).Sum();
        //        var htHomePastThirdDownAttempts = homeTeamHomeOpponents.Select(x => x.HomeTeamThirdDownAttempts).Sum();
        //        double htPastThirdDownAttempts = 0;
        //        var htAwayPastThirdDownCompletions = homeTeamAwayOpponents.Select(x => x.AwayTeamThirdDownCompletions).Sum();
        //        var htHomePastThirdDownCompletions = homeTeamHomeOpponents.Select(x => x.HomeTeamThirdDownCompletions).Sum();
        //        double htPastThirdDownCompletions = 0;

        //        var atOpponentAwayThirdDownAttempts = awayTeamAwayOpponents.Select(x => x.HomeTeamThirdDownAttempts).Sum();
        //        var atOpponentHomeThirdDownAttempts = awayTeamHomeOpponents.Select(x => x.AwayTeamThirdDownAttempts).Sum();
        //        double atOpponentThirdDownAttempts = 0;
        //        var htOpponentAwayThirdDownAttempts = homeTeamAwayOpponents.Select(x => x.HomeTeamThirdDownAttempts).Sum();
        //        var htOpponentHomeThirdDownAttempts = homeTeamHomeOpponents.Select(x => x.AwayTeamThirdDownAttempts).Sum();
        //        double htOpponentThirdDownAttempts = 0;

        //        var atOpponentAwayThirdDownCompletions = awayTeamAwayOpponents.Select(x => x.HomeTeamThirdDownCompletions).Sum();
        //        var atOpponentHomeThirdDownCompletions = awayTeamHomeOpponents.Select(x => x.AwayTeamThirdDownCompletions).Sum();
        //        double atOpponentThirdDownCompletions = 0;
        //        var htOpponentAwayThirdDownCompletions = homeTeamAwayOpponents.Select(x => x.HomeTeamThirdDownCompletions).Sum();
        //        var htOpponentHomeThirdDownCompletions = homeTeamHomeOpponents.Select(x => x.AwayTeamThirdDownCompletions).Sum();
        //        double htOpponentThirdDownCompletions = 0;

        //        try
        //        {
        //            atPastThirdDownAttempts = (atAwayPastThirdDownAttempts + atHomePastThirdDownAttempts) / awayTeamPastGames.Count();
        //            atPastThirdDownCompletions = (atAwayPastThirdDownCompletions + atHomePastThirdDownCompletions) / awayTeamPastGames.Count();
        //            atOpponentThirdDownAttempts = (atOpponentAwayThirdDownAttempts + atOpponentHomeThirdDownAttempts) / awayTeamPastGames.Count();
        //            atOpponentThirdDownCompletions = (atOpponentAwayThirdDownCompletions + atOpponentHomeThirdDownCompletions) / awayTeamPastGames.Count();
        //            htPastThirdDownAttempts = (htAwayPastThirdDownAttempts + htHomePastThirdDownAttempts) / homeTeamPastGames.Count();
        //            htPastThirdDownCompletions = (htAwayPastThirdDownCompletions + htHomePastThirdDownCompletions) / homeTeamPastGames.Count();
        //            htOpponentThirdDownAttempts = (htOpponentAwayThirdDownAttempts + htOpponentHomeThirdDownAttempts) / homeTeamPastGames.Count();
        //            htOpponentThirdDownCompletions = (htOpponentAwayThirdDownCompletions + htOpponentHomeThirdDownCompletions) / homeTeamPastGames.Count();
        //        }
        //        catch { }

        //        var awayTeamThirdDownAttemptsPrediction = new ThirdDownAttemptsModel.ModelInput()
        //        {
        //            TeamOneId = requestModel.AwayTeamId,
        //            TeamOnePastThirdDownAttempts = (float)atPastThirdDownAttempts,
        //            TeamOnePastThirdDownCompletions = (float)atPastThirdDownCompletions,
        //            TeamTwoId = requestModel.HomeTeamId,
        //            TeamTwoThirdDownsAttemptedAllowed = (float)htOpponentThirdDownAttempts,
        //            TeamTwoThirdDownsCompletedAllowed = (float)htOpponentThirdDownCompletions,
        //            Week = requestModel.Week,
        //            Year = requestModel.Year,
        //        };

        //        var awayTeamThirdDownCompletionsPrediction = new ThirdDownCompletionsModel.ModelInput()
        //        {
        //            TeamOneId = requestModel.AwayTeamId,
        //            TeamOnePastThirdDownAttempts = (float)atPastThirdDownAttempts,
        //            TeamOnePastThirdDownCompletions = (float)atPastThirdDownCompletions,
        //            TeamTwoId = requestModel.HomeTeamId,
        //            TeamTwoThirdDownsAttemptedAllowed = (float)htOpponentThirdDownAttempts,
        //            TeamTwoThirdDownsCompletedAllowed = (float)htOpponentThirdDownCompletions,
        //            Week = requestModel.Week,
        //            Year = requestModel.Year,
        //        };

        //        var homeTeamThirdDownAttemptsPrediction = new ThirdDownAttemptsModel.ModelInput()
        //        {
        //            TeamOneId = requestModel.HomeTeamId,
        //            TeamOnePastThirdDownAttempts = (float)htPastThirdDownAttempts,
        //            TeamOnePastThirdDownCompletions = (float)htPastThirdDownCompletions,
        //            TeamTwoId = requestModel.AwayTeamId,
        //            TeamTwoThirdDownsAttemptedAllowed = (float)atOpponentThirdDownAttempts,
        //            TeamTwoThirdDownsCompletedAllowed = (float)atOpponentThirdDownCompletions,
        //            Week = requestModel.Week,
        //            Year = requestModel.Year,
        //        };

        //        var homeTeamThirdDownCompletionsPrediction = new ThirdDownCompletionsModel.ModelInput()
        //        {
        //            TeamOneId = requestModel.HomeTeamId,
        //            TeamOnePastThirdDownAttempts = (float)htPastThirdDownAttempts,
        //            TeamOnePastThirdDownCompletions = (float)htPastThirdDownCompletions,
        //            TeamTwoId = requestModel.AwayTeamId,
        //            TeamTwoThirdDownsAttemptedAllowed = (float)atOpponentThirdDownAttempts,
        //            TeamTwoThirdDownsCompletedAllowed = (float)atOpponentThirdDownCompletions,
        //            Week = requestModel.Week,
        //            Year = requestModel.Year,
        //        };

        //        //Load model and predict output
        //        var awayTeamThirdDownCompletions = ThirdDownCompletionsModel.Predict(awayTeamThirdDownCompletionsPrediction);
        //        var awayTeamThirdDownAttempts = ThirdDownAttemptsModel.Predict(awayTeamThirdDownAttemptsPrediction);
        //        var homeTeamThirdDownCompletions = ThirdDownCompletionsModel.Predict(homeTeamThirdDownCompletionsPrediction);
        //        var homeTeamThirdDownAttempts = ThirdDownAttemptsModel.Predict(homeTeamThirdDownAttemptsPrediction);
        //        #endregion

        //        #region Fourth Down Prediction
        //        var atAwayPastFourthDownAttempts = awayTeamPastGames.Where(x => x.AwayTeamId == requestModel.AwayTeamId).Select(x => x.AwayTeamFourthDownAttempts).Sum();
        //        var atHomePastFourthDownAttempts = awayTeamPastGames.Where(x => x.HomeTeamId == requestModel.AwayTeamId).Select(x => x.HomeTeamFourthDownAttempts).Sum();
        //        double atPastFourthDownAttempts = 0;
        //        var atAwayPastFourthDownCompletions = awayTeamPastGames.Where(x => x.AwayTeamId == requestModel.AwayTeamId).Select(x => x.AwayTeamFourthDownCompletions).Sum();
        //        var atHomePastFourthDownCompletions = awayTeamPastGames.Where(x => x.HomeTeamId == requestModel.AwayTeamId).Select(x => x.HomeTeamFourthDownCompletions).Sum();
        //        double atPastFourthDownCompletions = 0;

        //        var htAwayPastFourthDownAttempts = homeTeamPastGames.Where(x => x.AwayTeamId == requestModel.AwayTeamId).Select(x => x.AwayTeamFourthDownAttempts).Sum();
        //        var htHomePastFourthDownAttempts = homeTeamPastGames.Where(x => x.HomeTeamId == requestModel.AwayTeamId).Select(x => x.HomeTeamFourthDownAttempts).Sum();
        //        double htPastFourthDownAttempts = 0;
        //        var htAwayPastFourthDownCompletions = homeTeamPastGames.Where(x => x.AwayTeamId == requestModel.AwayTeamId).Select(x => x.AwayTeamFourthDownCompletions).Sum();
        //        var htHomePastFourthDownCompletions = homeTeamPastGames.Where(x => x.HomeTeamId == requestModel.AwayTeamId).Select(x => x.HomeTeamFourthDownCompletions).Sum();
        //        double htPastFourthDownCompletions = 0;

        //        var atOpponentAwayFourthDownAttempts = awayTeamAwayOpponents.Select(x => x.HomeTeamFourthDownAttempts).Sum();
        //        var atOpponentHomeFourthDownAttempts = awayTeamAwayOpponents.Select(x => x.AwayTeamFourthDownAttempts).Sum();
        //        double atOpponentFourthDownAttempts = 0;
        //        var htOpponentAwayFourthDownAttempts = homeTeamAwayOpponents.Select(x => x.HomeTeamFourthDownAttempts).Sum();
        //        var htOpponentHomeFourthDownAttempts = homeTeamAwayOpponents.Select(x => x.AwayTeamFourthDownAttempts).Sum();
        //        double htOpponentFourthDownAttempts = 0;

        //        var atOpponentAwayFourthDownCompletions = awayTeamAwayOpponents.Select(x => x.HomeTeamFourthDownCompletions).Sum();
        //        var atOpponentHomeFourthDownCompletions = awayTeamAwayOpponents.Select(x => x.AwayTeamFourthDownCompletions).Sum();
        //        double atOpponentFourthDownCompletions = 0;
        //        var htOpponentAwayFourthDownCompletions = homeTeamAwayOpponents.Select(x => x.HomeTeamFourthDownCompletions).Sum();
        //        var htOpponentHomeFourthDownCompletions = homeTeamAwayOpponents.Select(x => x.AwayTeamFourthDownCompletions).Sum();
        //        double htOpponentFourthDownCompletions = 0;

        //        try
        //        {
        //            atPastFourthDownAttempts = (atAwayPastFourthDownAttempts + atHomePastFourthDownAttempts) / awayTeamPastGames.Count();
        //            atPastFourthDownCompletions = (atAwayPastFourthDownCompletions + atHomePastFourthDownCompletions) / awayTeamPastGames.Count();
        //            htPastFourthDownAttempts = (htAwayPastFourthDownAttempts + htHomePastFourthDownAttempts) / homeTeamPastGames.Count();
        //            htPastFourthDownCompletions = (htAwayPastFourthDownCompletions + htHomePastFourthDownCompletions) / homeTeamPastGames.Count();
        //            atOpponentFourthDownAttempts = (atOpponentAwayFourthDownAttempts + atOpponentHomeFourthDownAttempts) / awayTeamPastGames.Count();
        //            htOpponentFourthDownAttempts = (htOpponentAwayFourthDownAttempts + htOpponentHomeFourthDownAttempts) / homeTeamPastGames.Count();
        //            atOpponentFourthDownCompletions = (atOpponentAwayFourthDownCompletions + atOpponentHomeFourthDownCompletions) / awayTeamPastGames.Count();
        //            htOpponentFourthDownCompletions = (htOpponentAwayFourthDownCompletions + htOpponentHomeFourthDownCompletions) / homeTeamPastGames.Count();
        //        }
        //        catch { }

        //        var awayTeamFourthDownAttemptsPrediction = new FourthDownAttemptsModel.ModelInput()
        //        {
        //            TeamOneId = requestModel.AwayTeamId,
        //            TeamOnePastFourthDownAttempts = (float)atPastFourthDownAttempts,
        //            TeamOnePastFourthDownCompletions = (float)atPastFourthDownCompletions,
        //            TeamTwoId = requestModel.HomeTeamId,
        //            TeamTwoFourthDownsAttemptedAllowed = (float)htOpponentFourthDownAttempts,
        //            TeamTwoFourthDownsCompletedAllowed = (float)htOpponentFourthDownCompletions,
        //            Week = requestModel.Week,
        //            Year = requestModel.Year,
        //        };

        //        var awayTeamFourthDownCompletionsPrediction = new FourthDownCompletionsModel.ModelInput()
        //        {
        //            TeamOneId = requestModel.AwayTeamId,
        //            TeamOnePastFourthDownAttempts = (float)atPastFourthDownAttempts,
        //            TeamOnePastFourthDownCompletions = (float)atPastFourthDownCompletions,
        //            TeamTwoId = requestModel.HomeTeamId,
        //            TeamTwoFourthDownsAttemptedAllowed = (float)htOpponentFourthDownAttempts,
        //            TeamTwoFourthDownsCompletedAllowed = (float)htOpponentFourthDownCompletions,
        //            Week = requestModel.Week,
        //            Year = requestModel.Year,
        //        };

        //        var homeTeamFourthDownAttemptsPrediction = new FourthDownAttemptsModel.ModelInput()
        //        {
        //            TeamOneId = requestModel.HomeTeamId,
        //            TeamOnePastFourthDownAttempts = (float)htPastFourthDownAttempts,
        //            TeamOnePastFourthDownCompletions = (float)htPastFourthDownCompletions,
        //            TeamTwoId = requestModel.AwayTeamId,
        //            TeamTwoFourthDownsAttemptedAllowed = (float)atOpponentFourthDownAttempts,
        //            TeamTwoFourthDownsCompletedAllowed = (float)atOpponentFourthDownCompletions,
        //            Week = requestModel.Week,
        //            Year = requestModel.Year,
        //        };

        //        var homeTeamFourthDownCompletionsPrediction = new FourthDownCompletionsModel.ModelInput()
        //        {
        //            TeamOneId = requestModel.HomeTeamId,
        //            TeamOnePastFourthDownAttempts = (float)htPastFourthDownAttempts,
        //            TeamOnePastFourthDownCompletions = (float)htPastFourthDownCompletions,
        //            TeamTwoId = requestModel.AwayTeamId,
        //            TeamTwoFourthDownsAttemptedAllowed = (float)atOpponentFourthDownAttempts,
        //            TeamTwoFourthDownsCompletedAllowed = (float)atOpponentFourthDownCompletions,
        //            Week = requestModel.Week,
        //            Year = requestModel.Year,
        //        };

        //        //Load model and predict output
        //        var awayTeamFourthDownCompletions = FourthDownCompletionsModel.Predict(awayTeamFourthDownCompletionsPrediction);
        //        var awayTeamFourthDownAttempts = FourthDownAttemptsModel.Predict(awayTeamFourthDownAttemptsPrediction);
        //        var homeTeamFourthDownCompletions = FourthDownCompletionsModel.Predict(homeTeamFourthDownCompletionsPrediction);
        //        var homeTeamFourthDownAttempts = FourthDownAttemptsModel.Predict(homeTeamFourthDownAttemptsPrediction);
        //        #endregion

        //        #region Passing Yards Prediction
        //        var atOpponentAwayPassingAttempts = awayTeamAwayOpponents.Select(x => x.HomeTeamPassingAttempts).Sum();
        //        var atOpponentHomePassingAttempts = awayTeamHomeOpponents.Select(x => x.AwayTeamPassingAttempts).Sum();
        //        double atOpponentPassingAttempts = 0;
        //        var atOpponentAwayPassingCompletions = awayTeamAwayOpponents.Select(x => x.HomeTeamPassingCompletions).Sum();
        //        var atOpponentHomePassingCompletions = awayTeamHomeOpponents.Select(x => x.AwayTeamPassingCompletions).Sum();
        //        double atOpponentPassingCompletions = 0;


        //        var htOpponentAwayPassingAttempts = homeTeamAwayOpponents.Select(x => x.HomeTeamPassingAttempts).Sum();
        //        var htOpponentHomePassingAttempts = homeTeamHomeOpponents.Select(x => x.AwayTeamPassingAttempts).Sum();
        //        double htOpponentPassingAttempts = 0;
        //        var htOpponentAwayPassingCompletions = homeTeamAwayOpponents.Select(x => x.HomeTeamPassingCompletions).Sum();
        //        var htOpponentHomePassingCompletions = homeTeamHomeOpponents.Select(x => x.AwayTeamPassingCompletions).Sum();
        //        double htOpponentPassingCompletions = 0;


        //        var atAwayPassingAttempts = awayTeamAwayOpponents.Select(x => x.AwayTeamPassingAttempts).Sum();
        //        var atHomePassingAttempts = awayTeamHomeOpponents.Select(x => x.HomeTeamPassingAttempts).Sum();
        //        double atPassingAttempts = 0;
        //        var atAwayPassingCompletions = awayTeamAwayOpponents.Select(x => x.AwayTeamPassingCompletions).Sum();
        //        var atHomePassingCompletions = awayTeamHomeOpponents.Select(x => x.HomeTeamPassingCompletions).Sum();
        //        double atPassingCompletions = 0;


        //        var htAwayPassingAttempts = homeTeamAwayOpponents.Select(x => x.AwayTeamPassingAttempts).Sum();
        //        var htHomePassingAttempts = homeTeamHomeOpponents.Select(x => x.HomeTeamPassingAttempts).Sum();
        //        double htPassingAttempts = 0;
        //        var htAwayPassingCompletions = homeTeamAwayOpponents.Select(x => x.AwayTeamPassingCompletions).Sum();
        //        var htHomePassingCompletions = homeTeamHomeOpponents.Select(x => x.HomeTeamPassingCompletions).Sum();
        //        double htPassingCompletions = 0;

        //        try
        //        {
        //            atOpponentPassingAttempts = (atOpponentAwayPassingAttempts + atOpponentHomePassingAttempts) / awayTeamPastGames.Count();
        //            atOpponentPassingCompletions = (atOpponentAwayPassingCompletions + atOpponentHomePassingCompletions) / awayTeamPastGames.Count();
        //            htOpponentPassingAttempts = (htOpponentAwayPassingAttempts + htOpponentHomePassingAttempts) / homeTeamPastGames.Count();
        //            htOpponentPassingCompletions = (htOpponentAwayPassingCompletions + htOpponentHomePassingCompletions) / homeTeamPastGames.Count();
        //            atPassingAttempts = (atAwayPassingAttempts + atHomePassingAttempts) / awayTeamPastGames.Count();
        //            atPassingCompletions = (atAwayPassingCompletions + atHomePassingCompletions) / awayTeamPastGames.Count();
        //            htPassingAttempts = (htAwayPassingAttempts + htHomePassingAttempts) / homeTeamPastGames.Count();
        //            htPassingCompletions = (htAwayPassingCompletions + htHomePassingCompletions) / homeTeamPastGames.Count();
        //        }
        //        catch
        //        {

        //        }

        //        var awayTeamPassingYardsPrediction = new PassingYardsModel.ModelInput()
        //        {
        //            TeamOneId = requestModel.AwayTeamId,
        //            TeamOnePassingAttempts = (float)atPassingAttempts,
        //            TeamOnePassingCompletions = (float)atPassingCompletions,
        //            TeamTwoId = requestModel.HomeTeamId,
        //            TeamTwoOpponentPassingAttempts = (float)htOpponentPassingAttempts,
        //            TeamTwoOpponentPassingCompletions = (float)htOpponentPassingCompletions,
        //            Week = requestModel.Week,
        //            Year = requestModel.Year,
        //        };

        //        var homeTeamPassingYardsPrediction = new PassingYardsModel.ModelInput()
        //        {
        //            TeamOneId = requestModel.HomeTeamId,
        //            TeamOnePassingAttempts = (float)htPassingAttempts,
        //            TeamOnePassingCompletions = (float)htPassingCompletions,
        //            TeamTwoId = requestModel.AwayTeamId,
        //            TeamTwoOpponentPassingAttempts = (float)atOpponentPassingAttempts,
        //            TeamTwoOpponentPassingCompletions = (float)atOpponentPassingCompletions,
        //            Week = requestModel.Week,
        //            Year = requestModel.Year,
        //        };

        //        var awayTeamPassingYards = PassingYardsModel.Predict(awayTeamPassingYardsPrediction);
        //        var homeTeamPassingYards = PassingYardsModel.Predict(homeTeamPassingYardsPrediction);
        //        #endregion

        //        #region Yards Per Pass Prediction

        //        var atOpponentAwayYardsPerPass = awayTeamAwayOpponents.Select(x => x.HomeTeamYardsPerPass).Sum();
        //        var atOpponentHomeYardsPerPass = awayTeamHomeOpponents.Select(x => x.AwayTeamYardsPerPass).Sum();
        //        double atOpponentYardsPerPass = 0;
        //        var htOpponentAwayYardsPerPass = homeTeamAwayOpponents.Select(x => x.HomeTeamYardsPerPass).Sum();
        //        var htOpponentHomeYardsPerPass = homeTeamHomeOpponents.Select(x => x.AwayTeamYardsPerPass).Sum();
        //        double htOpponentYardsPerPass = 0;

        //        try
        //        {
        //            atOpponentYardsPerPass = (atOpponentAwayPassingAttempts + atOpponentHomePassingAttempts) / awayTeamPastGames.Count();
        //            htOpponentYardsPerPass = (htOpponentAwayYardsPerPass + htOpponentHomeYardsPerPass) / homeTeamPastGames.Count();
        //        }
        //        catch { }


        //        var awayTeamYardsPerPassPrediction = new YardsPerPassModel.ModelInput()
        //        {
        //            TeamOneId = requestModel.AwayTeamId,
        //            TeamOneNetPassingYards = awayTeamPassingYards.Score,
        //            TeamOnePassingAttempts = (float)atPassingAttempts,
        //            TeamOnePassingCompletions = (float)atPassingCompletions,
        //            TeamTwoId = requestModel.AwayTeamId,
        //            TeamTwoOpponentYardsPerPass = (float)htOpponentYardsPerPass,
        //            TeamTwoOpponentPassingAttempts = (float)htOpponentPassingAttempts,
        //            TeamTwoOpponentPassingCompletions = (float)htOpponentPassingCompletions,
        //            Week = requestModel.Week,
        //            Year = requestModel.Year,
        //        };

        //        var homeTeamYardsPerPassPrediction = new YardsPerPassModel.ModelInput()
        //        {
        //            TeamOneId = requestModel.HomeTeamId,
        //            TeamOneNetPassingYards = homeTeamPassingYards.Score,
        //            TeamOnePassingAttempts = (float)htPassingAttempts,
        //            TeamOnePassingCompletions = (float)htPassingCompletions,
        //            TeamTwoId = requestModel.AwayTeamId,
        //            TeamTwoOpponentYardsPerPass = (float)atOpponentYardsPerPass,
        //            TeamTwoOpponentPassingAttempts = (float)atOpponentPassingAttempts,
        //            TeamTwoOpponentPassingCompletions = (float)atOpponentPassingCompletions,
        //            Week = requestModel.Week,
        //            Year = requestModel.Year,
        //        };

        //        var awayTeamYardsPerPass = YardsPerPassModel.Predict(awayTeamYardsPerPassPrediction);
        //        var homeTeamYardsPerPass = YardsPerPassModel.Predict(homeTeamYardsPerPassPrediction);

        //        #endregion

        //        #region Rushing Yards Prediction

        //        var atOpponentAwayYardsPerRushAttempt = awayTeamAwayOpponents.Select(x => x.HomeTeamYardsPerRushAttempt).Sum();
        //        var atOpponentHomeYardsPerRushAttempt = awayTeamHomeOpponents.Select(x => x.AwayTeamYardsPerRushAttempt).Sum();
        //        double atOpponentYardsPerRushAttempt = 0;
        //        var atOpponentAwayRushingAttempts = awayTeamAwayOpponents.Select(x => x.HomeTeamRushingAttempts).Sum();
        //        var atOpponentHomeRushingAttempts = awayTeamHomeOpponents.Select(x => x.AwayTeamRushingAttempts).Sum();
        //        double atOpponentRushingAttempts = 0;

        //        var htOpponentAwayYardsPerRushAttempt = homeTeamAwayOpponents.Select(x => x.HomeTeamYardsPerRushAttempt).Sum();
        //        var htOpponentHomeYardsPerRushAttempt = homeTeamHomeOpponents.Select(x => x.AwayTeamYardsPerRushAttempt).Sum();
        //        double htOpponentYardsPerRushAttempt = 0;
        //        var htOpponentAwayRushingAttempts = homeTeamAwayOpponents.Select(x => x.HomeTeamRushingAttempts).Sum();
        //        var htOpponentHomeRushingAttempts = homeTeamHomeOpponents.Select(x => x.AwayTeamRushingAttempts).Sum();
        //        double htOpponentRushingAttempts = 0;

        //        var atAwayYardsPerRushAttempt = awayTeamAwayOpponents.Select(x => x.AwayTeamYardsPerRushAttempt).Sum();
        //        var atHomeYardsPerRushAttempt = awayTeamHomeOpponents.Select(x => x.HomeTeamYardsPerRushAttempt).Sum();
        //        double atYardsPerRushAttempt = 0;
        //        var atAwayRushingAttempts = awayTeamAwayOpponents.Select(x => x.AwayTeamRushingAttempts).Sum();
        //        var atHomeRushingAttempts = awayTeamHomeOpponents.Select(x => x.HomeTeamRushingAttempts).Sum();
        //        double atRushingAttempts = 0;

        //        var htAwayYardsPerRushAttempt = homeTeamAwayOpponents.Select(x => x.AwayTeamYardsPerRushAttempt).Sum();
        //        var htHomeYardsPerRushAttempt = homeTeamHomeOpponents.Select(x => x.HomeTeamYardsPerRushAttempt).Sum();
        //        double htYardsPerRushAttempt = 0;
        //        var htAwayRushingAttempts = homeTeamAwayOpponents.Select(x => x.AwayTeamRushingAttempts).Sum();
        //        var htHomeRushingAttempts = homeTeamHomeOpponents.Select(x => x.HomeTeamRushingAttempts).Sum();
        //        double htRushingAttempts = 0;

        //        try
        //        {
        //            atOpponentYardsPerRushAttempt = (atOpponentAwayYardsPerRushAttempt + atOpponentHomeYardsPerRushAttempt) / awayTeamPastGames.Count();
        //            atOpponentRushingAttempts = (atOpponentAwayRushingAttempts + atOpponentHomeRushingAttempts) / awayTeamPastGames.Count();
        //            htOpponentYardsPerRushAttempt = (htOpponentAwayYardsPerRushAttempt + htOpponentHomeYardsPerRushAttempt) / homeTeamPastGames.Count();
        //            htOpponentRushingAttempts = (htOpponentAwayRushingAttempts + htOpponentHomeRushingAttempts) / homeTeamPastGames.Count();

        //            atYardsPerRushAttempt = (atAwayYardsPerRushAttempt + atHomeYardsPerRushAttempt) / awayTeamPastGames.Count();
        //            atRushingAttempts = (atAwayRushingAttempts + atHomeRushingAttempts) / awayTeamPastGames.Count();
        //            htYardsPerRushAttempt = (htAwayYardsPerRushAttempt + htHomeYardsPerRushAttempt) / homeTeamPastGames.Count();
        //            htRushingAttempts = (htAwayRushingAttempts + htHomeRushingAttempts) / homeTeamPastGames.Count();
        //        }
        //        catch { }


        //        var awayTeamRushingYardsPrediction = new RushingYardsModel.ModelInput()
        //        {
        //            TeamOneId = requestModel.AwayTeamId,
        //            TeamOneRushingAttempts = (float)atRushingAttempts,
        //            TeamOneYardsPerRushAttempt = (float)atYardsPerRushAttempt,
        //            TeamTwoId = requestModel.HomeTeamId,
        //            TeamTwoOpponentRushingAttempts = (float)htOpponentRushingAttempts,
        //            TeamTwoOpponentYardsPerRushAttempt = (float)htOpponentYardsPerRushAttempt,
        //            Week = requestModel.Week,
        //            Year = requestModel.Year,
        //        };

        //        var homeTeamRushingYardsPrediction = new RushingYardsModel.ModelInput()
        //        {
        //            TeamOneId = requestModel.HomeTeamId,
        //            TeamOneRushingAttempts = (float)htRushingAttempts,
        //            TeamOneYardsPerRushAttempt = (float)htYardsPerRushAttempt,
        //            TeamTwoId = requestModel.AwayTeamId,
        //            TeamTwoOpponentRushingAttempts = (float)atOpponentRushingAttempts,
        //            TeamTwoOpponentYardsPerRushAttempt = (float)atOpponentYardsPerRushAttempt,
        //            Week = requestModel.Week,
        //            Year = requestModel.Year,
        //        };


        //        var awayTeamRushingYards = RushingYardsModel.Predict(awayTeamRushingYardsPrediction);
        //        var homeTeamRushingYards = RushingYardsModel.Predict(homeTeamRushingYardsPrediction);

        //        #endregion

        //        #region Fumbles Prediction

        //        var atOpponentAwayFumblesLost = awayTeamAwayOpponents.Select(x => x.HomeTeamFumblesLost).Sum();
        //        var atOpponentHomeFumblesLost = awayTeamHomeOpponents.Select(x => x.AwayTeamFumblesLost).Sum();
        //        double atOpponentFumblesLost = 0;
        //        var atAwayFumbles = awayTeamAwayOpponents.Select(x => x.AwayTeamFumblesLost).Sum();
        //        var atHomeFumbles = awayTeamHomeOpponents.Select(x => x.HomeTeamFumblesLost).Sum();
        //        double atFumbles = 0;


        //        var htOpponentAwayFumblesLost = homeTeamAwayOpponents.Select(x => x.HomeTeamFumblesLost).Sum();
        //        var htOpponentHomeFumblesLost = homeTeamHomeOpponents.Select(x => x.AwayTeamFumblesLost).Sum();
        //        double htOpponentFumblesLost = 0;
        //        var htAwayFumbles = homeTeamAwayOpponents.Select(x => x.AwayTeamFumblesLost).Sum();
        //        var htHomeFumbles = homeTeamHomeOpponents.Select(x => x.HomeTeamFumblesLost).Sum();
        //        double htFumbles = 0;

        //        try
        //        {
        //            atOpponentFumblesLost = (atOpponentAwayFumblesLost + atOpponentHomeFumblesLost) / awayTeamPastGames.Count();
        //            atFumbles = (atAwayFumbles + atHomeFumbles) / awayTeamPastGames.Count();
        //            htOpponentFumblesLost = (htOpponentAwayFumblesLost + htOpponentHomeFumblesLost) / homeTeamPastGames.Count();
        //            htFumbles = (htAwayFumbles + htHomeFumbles) / homeTeamPastGames.Count();
        //        }
        //        catch { }

        //        var awayTeamFumblesPrediction = new FumblesModel.ModelInput()
        //        {
        //            TeamOneId = requestModel.AwayTeamId,
        //            TeamOnePastFumblesLost = (float)atFumbles,
        //            TeamTwoId = requestModel.HomeTeamId,
        //            TeamTwoOpponentFumblesLost = (float)htOpponentFumblesLost,
        //            Week = requestModel.Week,
        //            Year = requestModel.Year,
        //        };

        //        var homeTeamFumblesPrediction = new FumblesModel.ModelInput()
        //        {
        //            TeamOneId = requestModel.HomeTeamId,
        //            TeamOnePastFumblesLost = (float)htFumbles,
        //            TeamTwoId = requestModel.AwayTeamId,
        //            TeamTwoOpponentFumblesLost = (float)atOpponentFumblesLost,
        //            Week = requestModel.Week,
        //            Year = requestModel.Year,
        //        };


        //        var awayTeamFumbles = FumblesModel.Predict(awayTeamFumblesPrediction);
        //        var homeTeamFumbles = FumblesModel.Predict(homeTeamFumblesPrediction);
        //        #endregion

        //        #region Interceptions Prediction

        //        var atOpponentAwayInterceptionsLost = awayTeamAwayOpponents.Select(x => x.HomeTeamInterceptions).Sum();
        //        var atOpponentHomeInterceptionsLost = awayTeamHomeOpponents.Select(x => x.AwayTeamInterceptions).Sum();
        //        double atOpponentInterceptionsLost = 0;
        //        var atAwayInterceptions = awayTeamAwayOpponents.Select(x => x.AwayTeamFumblesLost).Sum();
        //        var atHomeInterceptions = awayTeamHomeOpponents.Select(x => x.HomeTeamFumblesLost).Sum();
        //        double atInterceptions = 0;

        //        var htOpponentAwayInterceptionsLost = homeTeamAwayOpponents.Select(x => x.HomeTeamInterceptions).Sum();
        //        var htOpponentHomeInterceptionsLost = homeTeamHomeOpponents.Select(x => x.AwayTeamInterceptions).Sum();
        //        double htOpponentInterceptionsLost = 0;
        //        var htAwayInterceptions = homeTeamAwayOpponents.Select(x => x.AwayTeamFumblesLost).Sum();
        //        var htHomeInterceptions = homeTeamHomeOpponents.Select(x => x.HomeTeamFumblesLost).Sum();
        //        double htInterceptions = 0;

        //        try
        //        {
        //            atOpponentInterceptionsLost = (atOpponentAwayInterceptionsLost + atOpponentHomeInterceptionsLost) / awayTeamPastGames.Count();
        //            atInterceptions = (atAwayInterceptions + atHomeInterceptions) / awayTeamPastGames.Count();
        //            htOpponentInterceptionsLost = (htOpponentAwayInterceptionsLost + htOpponentHomeInterceptionsLost) / homeTeamPastGames.Count();
        //            htInterceptions = (htAwayInterceptions + htHomeInterceptions) / homeTeamPastGames.Count();
        //        }
        //        catch
        //        {

        //        }

        //        var awayTeamInterceptionsPrediction = new InterceptionsModel.ModelInput()
        //        {
        //            TeamOneId = requestModel.AwayTeamId,
        //            TeamOnePastInterceptions = (float)atInterceptions,
        //            TeamTwoId = requestModel.HomeTeamId,
        //            TeamTwoOpponentInterceptions = (float)htOpponentInterceptionsLost,
        //            Week = requestModel.Week,
        //            Year = requestModel.Year,
        //        };

        //        var homeTeamInterceptionsPrediction = new InterceptionsModel.ModelInput()
        //        {
        //            TeamOneId = requestModel.HomeTeamId,
        //            TeamOnePastInterceptions = (float)htInterceptions,
        //            TeamTwoId = requestModel.AwayTeamId,
        //            TeamTwoOpponentInterceptions = (float)atOpponentInterceptionsLost,
        //            Week = requestModel.Week,
        //            Year = requestModel.Year,
        //        };

        //        var awayTeamInterceptions = InterceptionsModel.Predict(awayTeamInterceptionsPrediction);
        //        var homeTeamInterceptions = InterceptionsModel.Predict(homeTeamInterceptionsPrediction);
        //        #endregion

        //        #region Over Prediction Prediction

        //        #region Inner Region Penalties/Plays
        //        var atAwayTotalPenalties = awayTeamAwayOpponents.Select(x => x.AwayTeamTotalPenalties).Sum();
        //        var atHomeTotalPenalties = awayTeamHomeOpponents.Select(x => x.HomeTeamTotalPenalties).Sum();
        //        double atTotalPenalties = 0;
        //        var atAwayTotalPenaltyYards = awayTeamAwayOpponents.Select(x => x.AwayTeamTotalPenaltyYards).Sum();
        //        var atHomeTotalPenaltyYards = awayTeamHomeOpponents.Select(x => x.HomeTeamTotalPenaltyYards).Sum();
        //        double atTotalPenaltyYards = 0;
        //        var atAwayPositivePlays = awayTeamAwayOpponents.Select(x => x.AwayTeamPositivePlays).Sum();
        //        var atHomePositivePlays = awayTeamHomeOpponents.Select(x => x.HomeTeamPositivePlays).Sum();
        //        double atTotalPositivePlays = 0;
        //        var atAwayNegativePlays = awayTeamAwayOpponents.Select(x => x.AwayTeamNegativePlays).Sum();
        //        var atHomeNegativePlays = awayTeamHomeOpponents.Select(x => x.HomeTeamNegativePlays).Sum();
        //        double atTotalNegativePlays = 0;
        //        var atAwayPossessionTime = awayTeamAwayOpponents.Select(x => x.AwayTeamPossessionTime).Sum();
        //        var atHomePossessionTime = awayTeamHomeOpponents.Select(x => x.HomeTeamPossessionTime).Sum();
        //        double atTotalPossessionTime = 0;


        //        var htAwayTotalPenalties = homeTeamAwayOpponents.Select(x => x.AwayTeamTotalPenalties).Sum();
        //        var htHomeTotalPenalties = homeTeamHomeOpponents.Select(x => x.HomeTeamTotalPenalties).Sum();
        //        double htTotalPenalties = 0;
        //        var htAwayTotalPenaltyYards = homeTeamAwayOpponents.Select(x => x.AwayTeamTotalPenaltyYards).Sum();
        //        var htHomeTotalPenaltyYards = homeTeamHomeOpponents.Select(x => x.HomeTeamTotalPenaltyYards).Sum();
        //        double htTotalPenaltyYards = 0;
        //        var htAwayPositivePlays = homeTeamAwayOpponents.Select(x => x.AwayTeamPositivePlays).Sum();
        //        var htHomePositivePlays = homeTeamHomeOpponents.Select(x => x.HomeTeamPositivePlays).Sum();
        //        double htTotalPositivePlays = 0;
        //        var htAwayNegativePlays = homeTeamAwayOpponents.Select(x => x.AwayTeamNegativePlays).Sum();
        //        var htHomeNegativePlays = homeTeamHomeOpponents.Select(x => x.HomeTeamNegativePlays).Sum();
        //        double htTotalNegativePlays = 0;
        //        var htAwayPossessionTime = homeTeamAwayOpponents.Select(x => x.AwayTeamPossessionTime).Sum();
        //        var htHomePossessionTime = homeTeamHomeOpponents.Select(x => x.HomeTeamPossessionTime).Sum();
        //        double htTotalPossessionTime = 0;
        //        #endregion

        //        #region Inner Region Downs
        //        double atHomeFirstDownsAllowed = awayTeamHomeOpponents.Select(x => x.AwayTeamFirstDowns).Sum();
        //        double atAwayFirstDownsAllowed = awayTeamAwayOpponents.Select(x => x.HomeTeamFirstDowns).Sum();
        //        double atHomeThirdDownAttemptsAllowed = awayTeamHomeOpponents.Select(x => x.AwayTeamThirdDownAttempts).Sum();
        //        double atAwayThirdDownAttemptsAllowed = awayTeamAwayOpponents.Select(x => x.HomeTeamThirdDownAttempts).Sum();
        //        double atHomeThirdDownCompletionsAllowed = awayTeamHomeOpponents.Select(x => x.AwayTeamThirdDownCompletions).Sum();
        //        double atAwayThirdDownCompletionsAllowed = awayTeamAwayOpponents.Select(x => x.HomeTeamThirdDownCompletions).Sum();
        //        double atHomeFourthDownAttemptsAllowed = awayTeamHomeOpponents.Select(x => x.AwayTeamFourthDownAttempts).Sum();
        //        double atAwayFourthDownAttemptsAllowed = awayTeamAwayOpponents.Select(x => x.HomeTeamFourthDownAttempts).Sum();
        //        double atHomeFourthDownCompletionsAllowed = awayTeamHomeOpponents.Select(x => x.AwayTeamFourthDownCompletions).Sum();
        //        double atAwayFourthDownCompletionsAllowed = awayTeamAwayOpponents.Select(x => x.HomeTeamFourthDownCompletions).Sum();
        //        double atFirstDownsAllowed = 0;
        //        double atThirdDownConversion = 0;
        //        double atFourthDownConversion = 0;

        //        double htHomeFirstDownsAllowed = homeTeamHomeOpponents.Select(x => x.AwayTeamFirstDowns).Sum();
        //        double htAwayFirstDownsAllowed = homeTeamAwayOpponents.Select(x => x.HomeTeamFirstDowns).Sum();
        //        double htHomeThirdDownAttemptsAllowed = homeTeamHomeOpponents.Select(x => x.AwayTeamThirdDownAttempts).Sum();
        //        double htAwayThirdDownAttemptsAllowed = homeTeamAwayOpponents.Select(x => x.HomeTeamThirdDownAttempts).Sum();
        //        double htHomeThirdDownCompletionsAllowed = homeTeamHomeOpponents.Select(x => x.AwayTeamThirdDownCompletions).Sum();
        //        double htAwayThirdDownCompletionsAllowed = homeTeamAwayOpponents.Select(x => x.HomeTeamThirdDownCompletions).Sum();
        //        double htHomeFourthDownAttemptsAllowed = homeTeamHomeOpponents.Select(x => x.AwayTeamFourthDownAttempts).Sum();
        //        double htAwayFourthDownAttemptsAllowed = homeTeamAwayOpponents.Select(x => x.HomeTeamFourthDownAttempts).Sum();
        //        double htHomeFourthDownCompletionsAllowed = homeTeamHomeOpponents.Select(x => x.AwayTeamFourthDownCompletions).Sum();
        //        double htAwayFourthDownCompletionsAllowed = homeTeamAwayOpponents.Select(x => x.HomeTeamFourthDownCompletions).Sum();
        //        double htFirstDownsAllowed = 0;
        //        double htThirdDownConversion = 0;
        //        double htFourthDownConversion = 0;
        //        #endregion

        //        #region Inner Region Yards
        //        double atAwayPassingYardsAllowed = awayTeamAwayOpponents.Select(x => x.HomeTeamNetPassingYards).Sum();
        //        double atHomePassingYardsAllowed = awayTeamHomeOpponents.Select(x => x.AwayTeamNetPassingYards).Sum();
        //        double atAwayRushingYardsAllowed = awayTeamAwayOpponents.Select(x => x.HomeTeamRushingYards).Sum();
        //        double atHomeRushingYardsAllowed = awayTeamHomeOpponents.Select(x => x.AwayTeamRushingYards).Sum();
        //        double atPassingYardsAllowed = 0;
        //        double atRushingYardsAllowed = 0;
        //        double atTotalYardsAllowed = 0;

        //        double htAwayPassingYardsAllowed = homeTeamAwayOpponents.Select(x => x.HomeTeamNetPassingYards).Sum();
        //        double htHomePassingYardsAllowed = homeTeamHomeOpponents.Select(x => x.AwayTeamNetPassingYards).Sum();
        //        double htAwayRushingYardsAllowed = homeTeamAwayOpponents.Select(x => x.HomeTeamRushingYards).Sum();
        //        double htHomeRushingYardsAllowed = homeTeamHomeOpponents.Select(x => x.AwayTeamRushingYards).Sum();
        //        double htPassingYardsAllowed = 0;
        //        double htRushingYardsAllowed = 0;
        //        double htTotalYardsAllowed = 0;
        //        #endregion
        //        try
        //        {
        //            atTotalPenalties = (atAwayTotalPenalties + atHomeTotalPenalties) / awayTeamPastGames.Count();
        //            atTotalPenaltyYards = (atAwayTotalPenaltyYards + atHomeTotalPenaltyYards) / awayTeamPastGames.Count();
        //            atTotalPositivePlays = (atAwayPositivePlays + atHomePositivePlays) / awayTeamPastGames.Count();
        //            atTotalNegativePlays = (atAwayNegativePlays + atHomeNegativePlays) / awayTeamPastGames.Count();
        //            atTotalPossessionTime = (atAwayPossessionTime + atHomePossessionTime) / awayTeamPastGames.Count();

        //            htTotalPenalties = (htAwayTotalPenalties + htHomeTotalPenalties) / homeTeamPastGames.Count();
        //            htTotalPenaltyYards = (htAwayTotalPenaltyYards + htHomeTotalPenaltyYards) / homeTeamPastGames.Count();
        //            htTotalPositivePlays = (htAwayPositivePlays + htHomePositivePlays) / homeTeamPastGames.Count();
        //            htTotalNegativePlays = (htAwayNegativePlays + htHomeNegativePlays) / homeTeamPastGames.Count();
        //            htTotalPossessionTime = (htAwayPossessionTime + htHomePossessionTime) / homeTeamPastGames.Count();

        //            atFirstDownsAllowed = (atAwayFirstDownsAllowed + atHomeFirstDownsAllowed) / awayTeamPastGames.Count();
        //            atThirdDownConversion = GetPercentage((atAwayThirdDownCompletionsAllowed + atHomeThirdDownCompletionsAllowed), (atAwayThirdDownAttemptsAllowed + atHomeThirdDownAttemptsAllowed));
        //            atFourthDownConversion = GetPercentage((atAwayFourthDownCompletionsAllowed + atHomeFourthDownCompletionsAllowed), (atAwayFourthDownAttemptsAllowed + atHomeFourthDownAttemptsAllowed));
        //            htFirstDownsAllowed = (htAwayFirstDownsAllowed + htHomeFirstDownsAllowed) / homeTeamPastGames.Count();
        //            htThirdDownConversion = GetPercentage((htAwayThirdDownCompletionsAllowed + htHomeThirdDownCompletionsAllowed), (htAwayThirdDownAttemptsAllowed + htHomeThirdDownAttemptsAllowed));
        //            htFourthDownConversion = GetPercentage((htAwayFourthDownCompletionsAllowed + htHomeFourthDownCompletionsAllowed), (htAwayFourthDownAttemptsAllowed + htHomeFourthDownAttemptsAllowed));

        //            atPassingYardsAllowed = (atAwayPassingYardsAllowed + atHomePassingYardsAllowed) / awayTeamPastGames.Count();
        //            atRushingYardsAllowed = (atAwayRushingYardsAllowed + atHomeRushingYardsAllowed) / awayTeamPastGames.Count();
        //            atTotalYardsAllowed = atPassingYardsAllowed + atRushingYardsAllowed;
        //            htPassingYardsAllowed = (htAwayPassingYardsAllowed + htHomePassingYardsAllowed) / homeTeamPastGames.Count();
        //            htRushingYardsAllowed = (htAwayRushingYardsAllowed + htHomeRushingYardsAllowed) / homeTeamPastGames.Count();
        //            htTotalYardsAllowed = htPassingYardsAllowed + htRushingYardsAllowed;
        //        }
        //        catch { }

        //        var awayTeamConference = await (from c in db.ConferenceTeam
        //                                        where c.TeamId == awayTeam.TeamId
        //                                        select c.TeamConference).FirstAsync();

        //        var homeTeamConference = await (from c in db.ConferenceTeam
        //                                        where c.TeamId == homeTeam.TeamId
        //                                        select c.TeamConference).FirstAsync();

        //        var awayTeamScorePrediction = new ConferenceScoreModelOne.ModelInput()
        //        {
        //            IsConference = requestModel.IsConference,
        //            IsNeutralSite = requestModel.IsNeutralSite,
        //            Week = requestModel.Week,
        //            Year = requestModel.Year,

        //            TeamId = requestModel.AwayTeamId,
        //            TeamConference = (int)awayTeamConference,
        //            TeamFirstDowns = awayTeamFirstDowns.Score,
        //            TeamThirdDownConversions = awayTeamThirdDownCompletions.Score / awayTeamThirdDownAttempts.Score,
        //            TeamFourthDownConversions = awayTeamFourthDownCompletions.Score / awayTeamFourthDownAttempts.Score,
        //            TeamTotalYards = awayTeamRushingYards.Score + awayTeamPassingYards.Score,
        //            TeamNetPassingYards = awayTeamPassingYards.Score,
        //            TeamRushingYards = awayTeamRushingYards.Score,
        //            TeamTotalPenalties = (float)atTotalPenalties,
        //            TeamTotalPenaltyYards = (float)atTotalPenaltyYards,
        //            TeamFumblesLost = awayTeamFumbles.Score,
        //            TeamInterceptions = awayTeamInterceptions.Score,
        //            TeamPossessionTime = (float)atTotalPossessionTime,
        //            TeamPositivePlays = (float)atTotalPositivePlays,
        //            TeamNegativePlays = (float)atTotalNegativePlays,

        //            OpponentId = requestModel.HomeTeamId,
        //            OpponentConference = (int)homeTeamConference,
        //            OpponentAverageFirstDownsAllowed = (float)htFirstDownsAllowed,
        //            OpponentAverageThirdDownConversionsAllowed = (float)htThirdDownConversion,
        //            OpponentAverageFourthDownConversionsAllowed = (float)htFourthDownConversion,
        //            OpponentAverageTotalYardsAllowed = (float)htTotalYardsAllowed,
        //            OpponentAveragePassingYardsAllowed = (float)htPassingYardsAllowed,
        //            OpponentAverageRushingYardsAllowed = (float)htRushingYardsAllowed,
        //            OpponentAverageTotalPenalties = (float)htTotalPenalties,
        //            OpponentAverageTotalPenaltyYards = (float)htAwayTotalPenaltyYards,
        //            OpponentAverageFumblesForced = (float)htOpponentFumblesLost,
        //            OpponentAverageInterceptionsForced = (float)htOpponentInterceptionsLost,
        //            OpponentAveragePossessionTime = (float)htTotalPossessionTime,
        //            OpponentAveragePositivePlays = (float)htTotalPositivePlays,
        //            OpponentAverageNegativePlays = (float)htTotalNegativePlays
        //        };

        //        var homeTeamScorePrediction = new ConferenceScoreModelOne.ModelInput()
        //        {
        //            IsConference = requestModel.IsConference,
        //            IsNeutralSite = requestModel.IsNeutralSite,
        //            Week = requestModel.Week,
        //            Year = requestModel.Year,

        //            TeamId = requestModel.HomeTeamId,
        //            TeamConference = (int)homeTeamConference,
        //            TeamFirstDowns = homeTeamFirstDowns.Score,
        //            TeamThirdDownConversions = homeTeamThirdDownCompletions.Score / homeTeamThirdDownAttempts.Score,
        //            TeamFourthDownConversions = homeTeamFourthDownCompletions.Score / homeTeamFourthDownAttempts.Score,
        //            TeamTotalYards = homeTeamRushingYards.Score + homeTeamPassingYards.Score,
        //            TeamNetPassingYards = homeTeamPassingYards.Score,
        //            TeamRushingYards = homeTeamRushingYards.Score,
        //            TeamTotalPenalties = (float)htTotalPenalties,
        //            TeamTotalPenaltyYards = (float)htTotalPenaltyYards,
        //            TeamFumblesLost = homeTeamFumbles.Score,
        //            TeamInterceptions = homeTeamInterceptions.Score,
        //            TeamPossessionTime = (float)htTotalPossessionTime,
        //            TeamPositivePlays = (float)htTotalPositivePlays,
        //            TeamNegativePlays = (float)htTotalNegativePlays,

        //            OpponentId = requestModel.AwayTeamId,
        //            OpponentConference = (int)awayTeamConference,
        //            OpponentAverageFirstDownsAllowed = (float)atFirstDownsAllowed,
        //            OpponentAverageThirdDownConversionsAllowed = (float)atThirdDownConversion,
        //            OpponentAverageFourthDownConversionsAllowed = (float)atFourthDownConversion,
        //            OpponentAverageTotalYardsAllowed = (float)atTotalYardsAllowed,
        //            OpponentAveragePassingYardsAllowed = (float)atPassingYardsAllowed,
        //            OpponentAverageRushingYardsAllowed = (float)atRushingYardsAllowed,
        //            OpponentAverageTotalPenalties = (float)atTotalPenalties,
        //            OpponentAverageTotalPenaltyYards = (float)atAwayTotalPenaltyYards,
        //            OpponentAverageFumblesForced = (float)atOpponentFumblesLost,
        //            OpponentAverageInterceptionsForced = (float)atOpponentInterceptionsLost,
        //            OpponentAveragePossessionTime = (float)atTotalPossessionTime,
        //            OpponentAveragePositivePlays = (float)atTotalPositivePlays,
        //            OpponentAverageNegativePlays = (float)atTotalNegativePlays
        //        };
        //        //Load model and predict output
        //        var awayTeamScoreModelOne = ConferenceScoreModelOne.Predict(awayTeamScorePrediction);
        //        var homeTeamScoreModelOne = ConferenceScoreModelOne.Predict(homeTeamScorePrediction);

        //        var awayTeamScorePrediction2 = new ConferenceScoreModelTwo.ModelInput()
        //        {
        //            IsConference = requestModel.IsConference,
        //            IsNeutralSite = requestModel.IsNeutralSite,
        //            Week = requestModel.Week,
        //            Year = requestModel.Year,

        //            TeamId = requestModel.AwayTeamId,
        //            TeamConference = (int)awayTeamConference,
        //            TeamFirstDowns = awayTeamFirstDowns.Score,
        //            TeamThirdDownConversions = awayTeamThirdDownCompletions.Score / awayTeamThirdDownAttempts.Score,
        //            TeamFourthDownConversions = awayTeamFourthDownCompletions.Score / awayTeamFourthDownAttempts.Score,
        //            TeamTotalYards = awayTeamRushingYards.Score + awayTeamPassingYards.Score,
        //            TeamNetPassingYards = awayTeamPassingYards.Score,
        //            TeamRushingYards = awayTeamRushingYards.Score,
        //            TeamTotalPenalties = (float)atTotalPenalties,
        //            TeamTotalPenaltyYards = (float)atTotalPenaltyYards,
        //            TeamFumblesLost = awayTeamFumbles.Score,
        //            TeamInterceptions = awayTeamInterceptions.Score,
        //            TeamPossessionTime = (float)atTotalPossessionTime,
        //            TeamPositivePlays = (float)atTotalPositivePlays,
        //            TeamNegativePlays = (float)atTotalNegativePlays,

        //            OpponentId = requestModel.HomeTeamId,
        //            OpponentConference = (int)homeTeamConference,
        //            OpponentAverageFirstDownsAllowed = (float)htFirstDownsAllowed,
        //            OpponentAverageThirdDownConversionsAllowed = (float)htThirdDownConversion,
        //            OpponentAverageFourthDownConversionsAllowed = (float)htFourthDownConversion,
        //            OpponentAverageTotalYardsAllowed = (float)htTotalYardsAllowed,
        //            OpponentAveragePassingYardsAllowed = (float)htPassingYardsAllowed,
        //            OpponentAverageRushingYardsAllowed = (float)htRushingYardsAllowed,
        //            OpponentAverageTotalPenalties = (float)htTotalPenalties,
        //            OpponentAverageTotalPenaltyYards = (float)htAwayTotalPenaltyYards,
        //            OpponentAverageFumblesForced = (float)htOpponentFumblesLost,
        //            OpponentAverageInterceptionsForced = (float)htOpponentInterceptionsLost,
        //            OpponentAveragePossessionTime = (float)htTotalPossessionTime,
        //            OpponentAveragePositivePlays = (float)htTotalPositivePlays,
        //            OpponentAverageNegativePlays = (float)htTotalNegativePlays
        //        };

        //        var homeTeamScorePrediction2 = new ConferenceScoreModelTwo.ModelInput()
        //        {
        //            IsConference = requestModel.IsConference,
        //            IsNeutralSite = requestModel.IsNeutralSite,
        //            Week = requestModel.Week,
        //            Year = requestModel.Year,

        //            TeamId = requestModel.HomeTeamId,
        //            TeamConference = (int)homeTeamConference,
        //            TeamFirstDowns = homeTeamFirstDowns.Score,
        //            TeamThirdDownConversions = homeTeamThirdDownCompletions.Score / homeTeamThirdDownAttempts.Score,
        //            TeamFourthDownConversions = homeTeamFourthDownCompletions.Score / homeTeamFourthDownAttempts.Score,
        //            TeamTotalYards = homeTeamRushingYards.Score + homeTeamPassingYards.Score,
        //            TeamNetPassingYards = homeTeamPassingYards.Score,
        //            TeamRushingYards = homeTeamRushingYards.Score,
        //            TeamTotalPenalties = (float)htTotalPenalties,
        //            TeamTotalPenaltyYards = (float)htTotalPenaltyYards,
        //            TeamFumblesLost = homeTeamFumbles.Score,
        //            TeamInterceptions = homeTeamInterceptions.Score,
        //            TeamPossessionTime = (float)htTotalPossessionTime,
        //            TeamPositivePlays = (float)htTotalPositivePlays,
        //            TeamNegativePlays = (float)htTotalNegativePlays,

        //            OpponentId = requestModel.AwayTeamId,
        //            OpponentConference = (int)awayTeamConference,
        //            OpponentAverageFirstDownsAllowed = (float)atFirstDownsAllowed,
        //            OpponentAverageThirdDownConversionsAllowed = (float)atThirdDownConversion,
        //            OpponentAverageFourthDownConversionsAllowed = (float)atFourthDownConversion,
        //            OpponentAverageTotalYardsAllowed = (float)atTotalYardsAllowed,
        //            OpponentAveragePassingYardsAllowed = (float)atPassingYardsAllowed,
        //            OpponentAverageRushingYardsAllowed = (float)atRushingYardsAllowed,
        //            OpponentAverageTotalPenalties = (float)atTotalPenalties,
        //            OpponentAverageTotalPenaltyYards = (float)atAwayTotalPenaltyYards,
        //            OpponentAverageFumblesForced = (float)atOpponentFumblesLost,
        //            OpponentAverageInterceptionsForced = (float)atOpponentInterceptionsLost,
        //            OpponentAveragePossessionTime = (float)atTotalPossessionTime,
        //            OpponentAveragePositivePlays = (float)atTotalPositivePlays,
        //            OpponentAverageNegativePlays = (float)atTotalNegativePlays
        //        };
        //        //Load model and predict output
        //        var awayTeamScoreModelTwo = ConferenceScoreModelTwo.Predict(awayTeamScorePrediction2);
        //        var homeTeamScoreModelTwo = ConferenceScoreModelTwo.Predict(homeTeamScorePrediction2);


        //        var awayTeamScorePrediction3 = new ConferenceScoreModelThree.ModelInput()
        //        {
        //            IsConference = requestModel.IsConference,
        //            IsNeutralSite = requestModel.IsNeutralSite,
        //            Week = requestModel.Week,
        //            Year = requestModel.Year,

        //            TeamId = requestModel.AwayTeamId,
        //            TeamConference = (int)awayTeamConference,
        //            TeamFirstDowns = awayTeamFirstDowns.Score,
        //            TeamThirdDownConversions = awayTeamThirdDownCompletions.Score / awayTeamThirdDownAttempts.Score,
        //            TeamFourthDownConversions = awayTeamFourthDownCompletions.Score / awayTeamFourthDownAttempts.Score,
        //            TeamTotalYards = awayTeamRushingYards.Score + awayTeamPassingYards.Score,
        //            TeamNetPassingYards = awayTeamPassingYards.Score,
        //            TeamRushingYards = awayTeamRushingYards.Score,
        //            TeamTotalPenalties = (float)atTotalPenalties,
        //            TeamTotalPenaltyYards = (float)atTotalPenaltyYards,
        //            TeamFumblesLost = awayTeamFumbles.Score,
        //            TeamInterceptions = awayTeamInterceptions.Score,
        //            TeamPossessionTime = (float)atTotalPossessionTime,
        //            TeamPositivePlays = (float)atTotalPositivePlays,
        //            TeamNegativePlays = (float)atTotalNegativePlays,

        //            OpponentId = requestModel.HomeTeamId,
        //            OpponentConference = (int)homeTeamConference,
        //            OpponentAverageFirstDownsAllowed = (float)htFirstDownsAllowed,
        //            OpponentAverageThirdDownConversionsAllowed = (float)htThirdDownConversion,
        //            OpponentAverageFourthDownConversionsAllowed = (float)htFourthDownConversion,
        //            OpponentAverageTotalYardsAllowed = (float)htTotalYardsAllowed,
        //            OpponentAveragePassingYardsAllowed = (float)htPassingYardsAllowed,
        //            OpponentAverageRushingYardsAllowed = (float)htRushingYardsAllowed,
        //            OpponentAverageTotalPenalties = (float)htTotalPenalties,
        //            OpponentAverageTotalPenaltyYards = (float)htAwayTotalPenaltyYards,
        //            OpponentAverageFumblesForced = (float)htOpponentFumblesLost,
        //            OpponentAverageInterceptionsForced = (float)htOpponentInterceptionsLost,
        //            OpponentAveragePossessionTime = (float)htTotalPossessionTime,
        //            OpponentAveragePositivePlays = (float)htTotalPositivePlays,
        //            OpponentAverageNegativePlays = (float)htTotalNegativePlays
        //        };

        //        var homeTeamScorePrediction3 = new ConferenceScoreModelThree.ModelInput()
        //        {
        //            IsConference = requestModel.IsConference,
        //            IsNeutralSite = requestModel.IsNeutralSite,
        //            Week = requestModel.Week,
        //            Year = requestModel.Year,

        //            TeamId = requestModel.HomeTeamId,
        //            TeamConference = (int)homeTeamConference,
        //            TeamFirstDowns = homeTeamFirstDowns.Score,
        //            TeamThirdDownConversions = homeTeamThirdDownCompletions.Score / homeTeamThirdDownAttempts.Score,
        //            TeamFourthDownConversions = homeTeamFourthDownCompletions.Score / homeTeamFourthDownAttempts.Score,
        //            TeamTotalYards = homeTeamRushingYards.Score + homeTeamPassingYards.Score,
        //            TeamNetPassingYards = homeTeamPassingYards.Score,
        //            TeamRushingYards = homeTeamRushingYards.Score,
        //            TeamTotalPenalties = (float)htTotalPenalties,
        //            TeamTotalPenaltyYards = (float)htTotalPenaltyYards,
        //            TeamFumblesLost = homeTeamFumbles.Score,
        //            TeamInterceptions = homeTeamInterceptions.Score,
        //            TeamPossessionTime = (float)htTotalPossessionTime,
        //            TeamPositivePlays = (float)htTotalPositivePlays,
        //            TeamNegativePlays = (float)htTotalNegativePlays,

        //            OpponentId = requestModel.AwayTeamId,
        //            OpponentConference = (int)awayTeamConference,
        //            OpponentAverageFirstDownsAllowed = (float)atFirstDownsAllowed,
        //            OpponentAverageThirdDownConversionsAllowed = (float)atThirdDownConversion,
        //            OpponentAverageFourthDownConversionsAllowed = (float)atFourthDownConversion,
        //            OpponentAverageTotalYardsAllowed = (float)atTotalYardsAllowed,
        //            OpponentAveragePassingYardsAllowed = (float)atPassingYardsAllowed,
        //            OpponentAverageRushingYardsAllowed = (float)atRushingYardsAllowed,
        //            OpponentAverageTotalPenalties = (float)atTotalPenalties,
        //            OpponentAverageTotalPenaltyYards = (float)atAwayTotalPenaltyYards,
        //            OpponentAverageFumblesForced = (float)atOpponentFumblesLost,
        //            OpponentAverageInterceptionsForced = (float)atOpponentInterceptionsLost,
        //            OpponentAveragePossessionTime = (float)atTotalPossessionTime,
        //            OpponentAveragePositivePlays = (float)atTotalPositivePlays,
        //            OpponentAverageNegativePlays = (float)atTotalNegativePlays
        //        };
        //        //Load model and predict output
        //        var awayTeamScoreModelThree = ConferenceScoreModelThree.Predict(awayTeamScorePrediction3);
        //        var homeTeamScoreModelThree = ConferenceScoreModelThree.Predict(homeTeamScorePrediction3);
        //        #endregion

        //        double homeTeamScoreOne = homeTeamScoreModelOne.Score + 2;
        //        double homeTeamScoreTwo = homeTeamScoreModelTwo.Score + 2;
        //        double homeTeamScoreThree = homeTeamScoreModelThree.Score + 2;
        //        double awayTeamScoreOne = awayTeamScoreModelOne.Score + 2;
        //        double awayTeamScoreTwo = awayTeamScoreModelTwo.Score + 2;
        //        double awayTeamScoreThree = awayTeamScoreModelThree.Score + 2;

        //        try
        //        {
        //            // homeTeamScoreOne = (await ConferenceOperations.GetConferenceScoreAverage(db, requestModel.Year, true, requestModel.HomeTeamId, requestModel.AwayTeamId)) + homeTeamScoreOne / 2;
        //            // homeTeamScoreTwo = (await ConferenceOperations.GetConferenceScoreAverage(db, requestModel.Year, true, requestModel.HomeTeamId, requestModel.AwayTeamId)) + homeTeamScoreTwo / 2;
        //            // homeTeamScoreThree = (await ConferenceOperations.GetConferenceScoreAverage(db, requestModel.Year, true, requestModel.HomeTeamId, requestModel.AwayTeamId)) + homeTeamScoreThree / 2;
        //            // awayTeamScoreOne = (await ConferenceOperations.GetConferenceScoreAverage(db, requestModel.Year, true, requestModel.AwayTeamId, requestModel.HomeTeamId)) + awayTeamScoreOne / 2;
        //            // awayTeamScoreTwo = (await ConferenceOperations.GetConferenceScoreAverage(db, requestModel.Year, true, requestModel.AwayTeamId, requestModel.HomeTeamId)) + awayTeamScoreTwo / 2;
        //            // awayTeamScoreThree = (await ConferenceOperations.GetConferenceScoreAverage(db, requestModel.Year, true, requestModel.AwayTeamId, requestModel.HomeTeamId)) + awayTeamScoreThree / 2;
        //        }
        //        catch { }

        //        PredictGameResponseModel response = new PredictGameResponseModel
        //        {
        //            HomeTeamId = requestModel.HomeTeamId,
        //            AwayTeamId = requestModel.AwayTeamId,
        //            HomeTeamName = homeTeam.TeamName,
        //            AwayTeamName = awayTeam.TeamName,
        //            Week = requestModel.Week,
        //            Year = requestModel.Year
        //        };

        //        var predictionList = new List<ScorePrediction>();

        //        var predictionOne = new ScorePrediction
        //        {
        //            HomeTeamScore = Math.Round(homeTeamScoreOne),
        //            AwayTeamScore = Math.Round(awayTeamScoreOne),
        //            OverUnderScore = Math.Round(homeTeamScoreOne + awayTeamScoreOne),
        //            ModelName = "Model One"
        //        };
        //        predictionList.Add(predictionOne);

        //        var predictionTwo = new ScorePrediction
        //        {
        //            HomeTeamScore = Math.Round(homeTeamScoreTwo),
        //            AwayTeamScore = Math.Round(awayTeamScoreTwo),
        //            OverUnderScore = Math.Round(homeTeamScoreTwo + awayTeamScoreTwo),
        //            ModelName = "Model Two"
        //        };
        //        predictionList.Add(predictionTwo);

        //        var predictionThree = new ScorePrediction
        //        {
        //            HomeTeamScore = Math.Round(homeTeamScoreThree),
        //            AwayTeamScore = Math.Round(awayTeamScoreThree),
        //            OverUnderScore = Math.Round(homeTeamScoreThree + awayTeamScoreThree),
        //            ModelName = "Model Three"
        //        };
        //        predictionList.Add(predictionThree);

        //        response.ScorePredictions = predictionList;

        //        #region Save Data
        //        var gamePrediction = new GamePredictionConferenceDbo
        //        {
        //            Week = requestModel.Week,
        //            Year = requestModel.Year,
        //            AwayTeamId = response.AwayTeamId,
        //            HomeTeamId = response.HomeTeamId,
        //            AwayTeamName = response.AwayTeamName,
        //            HomeTeamName = response.HomeTeamName,
        //        };
        //        db.GamePredictionConference.Add(gamePrediction);
        //        await db.SaveChangesAsync();

        //        var scorePredictionOne = new ScorePredictionConferenceDbo
        //        {
        //            GamePredictionConferenceId = gamePrediction.GamePredictionConferenceId,
        //            HomeTeamScore = (int)Math.Round(homeTeamScoreOne),
        //            AwayTeamScore = (int)Math.Round(awayTeamScoreOne),
        //            ModelName = "Model One"
        //        };

        //        var scorePredictionTwo = new ScorePredictionConferenceDbo
        //        {
        //            GamePredictionConferenceId = gamePrediction.GamePredictionConferenceId,
        //            HomeTeamScore = (int)Math.Round(homeTeamScoreTwo),
        //            AwayTeamScore = (int)Math.Round(awayTeamScoreTwo),
        //            ModelName = "Model Two"
        //        };

        //        var scorePredictionThree = new ScorePredictionConferenceDbo
        //        {
        //            GamePredictionConferenceId = gamePrediction.GamePredictionConferenceId,
        //            HomeTeamScore = (int)Math.Round(homeTeamScoreThree),
        //            AwayTeamScore = (int)Math.Round(awayTeamScoreThree),
        //            ModelName = "Model Three"
        //        };

        //        db.ScorePredictionConference.Add(scorePredictionOne);
        //        db.ScorePredictionConference.Add(scorePredictionTwo);
        //        db.ScorePredictionConference.Add(scorePredictionThree);

        //        await db.SaveChangesAsync();
        //        #endregion
        //        return response;
        //    }
        }

        public async Task<List<PredictGameResponseModel>> GetWeekConferencePredictions(int week, int year)
        {
            using (var db = await factory.CreateDbContextAsync())
            {
                var games = await (from g in db.GamePredictionConference
                                   where g.Week == week && g.Year == year
                                   select g).ToListAsync();

                var response = new List<PredictGameResponseModel>();

                foreach (var game in games)
                {
                    var scores = await (from s in db.ScorePredictionConference
                                        where s.GamePredictionConferenceId == game.GamePredictionConferenceId
                                        select s).ToListAsync();

                    var scorePrediction = new List<ScorePrediction>();

                    foreach (var score in scores)
                    {
                        var gameScore = new ScorePrediction
                        {
                            ModelName = score.ModelName,
                            HomeTeamScore = score.HomeTeamScore,
                            AwayTeamScore = score.AwayTeamScore,
                            OverUnderScore = score.HomeTeamScore + score.AwayTeamScore
                        };

                        scorePrediction.Add(gameScore);
                    }

                    var predictGame = new PredictGameResponseModel
                    {
                        HomeTeamId = game.HomeTeamId,
                        AwayTeamId = game.AwayTeamId,
                        HomeTeamName = game.HomeTeamName,
                        AwayTeamName = game.AwayTeamName,
                        Week = week,
                        Year = year,
                        ScorePredictions = scorePrediction
                    };

                    response.Add(predictGame);
                }

                return response;
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
