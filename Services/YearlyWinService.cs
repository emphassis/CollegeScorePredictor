using CollegeScorePredictor.AIModels.OverPrediction;
using CollegeScorePredictor.Models.Database;
using CollegeScorePredictor.Models.EspnSchedule;
using CollegeScorePredictor.Models.EspnScoreboard;
using CollegeScorePredictor.Models.Win;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using System.Text.Json;

namespace CollegeScorePredictor.Services
{
    public class YearlyWinService
    {
        private IDbContextFactory<AppDbContext> factory;
        public YearlyWinService(IDbContextFactory<AppDbContext> _factory)
        {
            factory = _factory;
        }

        public async Task<YearlyWinResponseModel> SearchYearlyWinAsync(int year)
        {

            using (var db = await factory.CreateDbContextAsync())
            {
                YearlyWinResponseModel response = new YearlyWinResponseModel();
                response.TeamWins = new List<TeamWinsModel>();

                var teams = await (from t in db.ConferenceTeam select t).ToListAsync();

                foreach (var team in teams)
                {
                    var teamHomeGames = await (from h in db.GameResult
                                               where h.HomeTeamId == team.TeamId && h.Year == year
                                               select h).ToListAsync();

                    var teamAwayGames = await (from a in db.GameResult
                                               where a.AwayTeamId == team.TeamId && a.Year == year
                                               select a).ToListAsync();

                    var teamPredictionAwayGames = await (from a in db.YearlyWin
                                                         where a.AwayTeamId == team.TeamId && a.Year == year
                                                         select new WinModel
                                                         {
                                                             AwayTeamWin = a.AwayTeamWin,
                                                             HomeTeamWin = a.HomeTeamWin
                                                         }).Distinct().ToListAsync();

                    var teamPredictionHomeGames = await (from h in db.YearlyWin
                                                         where h.HomeTeamId == team.TeamId && h.Year == year
                                                         select new WinModel
                                                         {
                                                             AwayTeamWin = h.AwayTeamWin,
                                                             HomeTeamWin = h.HomeTeamWin
                                                         }).Distinct().ToListAsync();

                    var model = new TeamWinsModel();
                    model.TeamId = team.TeamId;
                    model.TeamName = team.TeamName;
                    model.Wins = teamHomeGames.Where(x => x.HomeTeamWon).Count() + teamAwayGames.Where(x => x.AwayTeamWon).Count();
                    model.Losses = teamHomeGames.Where(x => x.AwayTeamWon).Count() + teamAwayGames.Where(x => x.HomeTeamWon).Count();
                    model.PredictedWins = teamPredictionAwayGames.Where(x => x.AwayTeamWin > (x.HomeTeamWin + .2)).Count() + teamPredictionHomeGames.Where(x => x.HomeTeamWin > (x.AwayTeamWin + .2)).Count();
                    model.PredictedLosses = teamPredictionAwayGames.Where(x => x.AwayTeamWin < (x.HomeTeamWin + .2)).Count() + teamPredictionHomeGames.Where(x => x.HomeTeamWin < (x.AwayTeamWin + .2)).Count();
                    model.PredictedTossups = teamPredictionAwayGames.Where(x => x.AwayTeamWin == x.HomeTeamWin).Count() + teamPredictionHomeGames.Where(x => x.HomeTeamWin == x.AwayTeamWin).Count();

                    response.TeamWins.Add(model);
                }

                response.PredictedLessThanOne = response.TeamWins.Where(x=>x.PredictedWins == x.Wins).Count();
                response.PredictedLessThanTwo = 0;
                response.PredictedLessThanThree = 0;
                return response;
            }
        }

        public class WinModel
        {
            public double AwayTeamWin { get; set; }
            public double HomeTeamWin { get; set; }
        }

        public async Task CreateYearlyWinAsync(int year)
        {
            using (var db = await factory.CreateDbContextAsync())
            {
                var teams = await (from t in db.ConferenceTeam select t).ToListAsync();
                var lastYear = year - 1;
                foreach (var team in teams)
                {

                    //https://site.web.api.espn.com/apis/site/v2/sports/football/college-football/teams/98/schedule?region=us&lang=en&season=2022&seasontype=2
                    var apiUrl = "https://site.web.api.espn.com/apis/site/v2/sports/football/college-football/teams/" + team.TeamId + "/schedule?region=us&lang=en&season=" + year + "&seasontype=2";
                    Console.WriteLine("getting schedule for " + team.TeamName);
                    EspnScheduleModel? model;
                    try
                    {
                        var client = new HttpClient();
                        var data = await client.GetStringAsync(apiUrl);
                        model = JsonSerializer.Deserialize<EspnScheduleModel>(data);

                        var opponentEvents = model.events;

                        foreach (var opponentEvent in opponentEvents)
                        {
                            foreach (var opponent in opponentEvent.competitions)
                            {
                                try
                                {
                                    var awayScore = 0.0f;
                                    var homeScore = 0.0f;
                                    YearlyWinDbo response;

                                    var opponentCompetitor = opponent.competitors.Where(x => int.Parse(x.id!) != team.TeamId).Select(x => x).First();
                                    if (opponentCompetitor == null)
                                    {
                                        Console.WriteLine("Opponent Competitor is null");
                                        continue;
                                    }

                                    var opponentId = long.Parse(opponentCompetitor.id!);
                                    var isTeamHome = !opponentCompetitor.homeAway!.Contains("home");

                                    var homeTeamId = isTeamHome ? team.TeamId : opponentId;
                                    var awayTeamId = !isTeamHome ? team.TeamId : opponentId;

                                    ConferenceTeamDbo opponentConferenceTeam;
                                    try
                                    {
                                        opponentConferenceTeam = teams.Where(x => x.TeamId == opponentId).Select(x => x).First();

                                        response = await PredictGame(new PredictGameRequestModel
                                        {
                                            IsNeutralSite = false,
                                            IsConference = (team.TeamConference == opponentConferenceTeam.TeamConference),
                                            AwayTeamId = awayTeamId,
                                            HomeTeamId = homeTeamId,
                                            Year = lastYear,
                                            Week = 20
                                        });

                                        response.Year = year;
                                    }
                                    catch
                                    {
                                        opponentConferenceTeam = new ConferenceTeamDbo
                                        {
                                            TeamConference = 100
                                        };

                                        awayScore = isTeamHome ? .2f : 1f;
                                        homeScore = !isTeamHome ? .2f : 1f;

                                        response = new YearlyWinDbo
                                        {
                                            HomeTeamWin = homeScore,
                                            AwayTeamWin = homeScore,
                                            AwayTeamId = awayTeamId,
                                            HomeTeamId = homeTeamId,
                                            Year = year
                                        };
                                    }

                                    await db.YearlyWin.AddAsync(response);
                                    await db.SaveChangesAsync();
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine(ex.Message);
                                    Debug.WriteLine(ex.StackTrace);
                                    Debug.WriteLine("Broke inside foreach loop");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        Debug.WriteLine(ex.StackTrace);
                        Console.WriteLine("Failed to get schedule for " + team.TeamName);
                        //continue;
                        // Console.WriteLine(ex.Message);
                    }
                }
                Console.WriteLine("______________________________________________");
                Console.WriteLine("______________________________________________");
                Console.WriteLine("______________________________________________");
                Console.WriteLine("______________________________________________");
                Console.WriteLine("________FINISHED CALCULATING WINS!!!__________");
                Console.WriteLine("______________________________________________");
                Console.WriteLine("______________________________________________");
                Console.WriteLine("______________________________________________");
                Console.WriteLine("______________________________________________");
            }
        }

        public async Task<YearlyWinDbo> PredictGame(PredictGameRequestModel requestModel)
        {
            using (var db = await factory.CreateDbContextAsync())
            {
                var games = await (from e in db.GameResult
                                   where e.Year == requestModel.Year &&
                                   e.HomeTeamId == requestModel.HomeTeamId || e.AwayTeamId == requestModel.HomeTeamId ||
                                   e.HomeTeamId == requestModel.AwayTeamId || e.AwayTeamId == requestModel.AwayTeamId
                                   select e).ToListAsync();

                var homeTeam = await (from e in db.ConferenceTeam
                                      where e.TeamId == requestModel.HomeTeamId
                                      select e).FirstAsync();

                var awayTeam = await (from e in db.ConferenceTeam
                                      where e.TeamId == requestModel.AwayTeamId
                                      select e).FirstAsync();

                List<GameResultDbo> gameStats = games;

                var awayTeamPastGames = gameStats.Where(x => x.AwayTeamId == requestModel.AwayTeamId || x.HomeTeamId == requestModel.AwayTeamId);
                var homeTeamPastGames = gameStats.Where(x => x.AwayTeamId == requestModel.HomeTeamId || x.HomeTeamId == requestModel.HomeTeamId);

                var awayTeamAwayOpponents = awayTeamPastGames.Where(x => x.AwayTeamId == requestModel.AwayTeamId).Select(x => x).ToList();
                var awayTeamHomeOpponents = awayTeamPastGames.Where(x => x.HomeTeamId == requestModel.AwayTeamId).Select(x => x).ToList();

                var homeTeamAwayOpponents = homeTeamPastGames.Where(x => x.AwayTeamId == requestModel.HomeTeamId).Select(x => x).ToList();
                var homeTeamHomeOpponents = homeTeamPastGames.Where(x => x.HomeTeamId == requestModel.HomeTeamId).Select(x => x).ToList();

                #region First Down Prediction
                var atAwayPastFirstDowns = awayTeamAwayOpponents.Select(x => x.AwayTeamFirstDowns).Sum();
                var atHomePastFirstDowns = awayTeamHomeOpponents.Select(x => x.HomeTeamFirstDowns).Sum();
                double atPastFirstDowns = 0;

                var htAwayPastFirstDowns = homeTeamAwayOpponents.Select(x => x.AwayTeamFirstDowns).Sum();
                var htHomePastFirstDowns = homeTeamHomeOpponents.Select(x => x.HomeTeamFirstDowns).Sum();
                double htPastFirstDowns = 0;

                try
                {
                    atPastFirstDowns = (atAwayPastFirstDowns + atHomePastFirstDowns) / awayTeamPastGames.Count();
                    htPastFirstDowns = (htAwayPastFirstDowns + htHomePastFirstDowns) / homeTeamPastGames.Count();
                }
                catch { }

                #endregion

                #region Third Down Prediction
                var atAwayPastThirdDownAttempts = awayTeamAwayOpponents.Select(x => x.AwayTeamThirdDownAttempts).Sum();
                var atHomePastThirdDownAttempts = awayTeamHomeOpponents.Select(x => x.HomeTeamThirdDownAttempts).Sum();
                double atPastThirdDownAttempts = 0;
                var atAwayPastThirdDownCompletions = awayTeamAwayOpponents.Select(x => x.AwayTeamThirdDownCompletions).Sum();
                var atHomePastThirdDownCompletions = awayTeamHomeOpponents.Select(x => x.HomeTeamThirdDownCompletions).Sum();
                double atPastThirdDownCompletions = 0;

                var htAwayPastThirdDownAttempts = homeTeamAwayOpponents.Select(x => x.AwayTeamThirdDownAttempts).Sum();
                var htHomePastThirdDownAttempts = homeTeamHomeOpponents.Select(x => x.HomeTeamThirdDownAttempts).Sum();
                double htPastThirdDownAttempts = 0;
                var htAwayPastThirdDownCompletions = homeTeamAwayOpponents.Select(x => x.AwayTeamThirdDownCompletions).Sum();
                var htHomePastThirdDownCompletions = homeTeamHomeOpponents.Select(x => x.HomeTeamThirdDownCompletions).Sum();
                double htPastThirdDownCompletions = 0;

                var atOpponentAwayThirdDownAttempts = awayTeamAwayOpponents.Select(x => x.HomeTeamThirdDownAttempts).Sum();
                var atOpponentHomeThirdDownAttempts = awayTeamHomeOpponents.Select(x => x.AwayTeamThirdDownAttempts).Sum();
                double atOpponentThirdDownAttempts = 0;
                var htOpponentAwayThirdDownAttempts = homeTeamAwayOpponents.Select(x => x.HomeTeamThirdDownAttempts).Sum();
                var htOpponentHomeThirdDownAttempts = homeTeamHomeOpponents.Select(x => x.AwayTeamThirdDownAttempts).Sum();
                double htOpponentThirdDownAttempts = 0;

                var atOpponentAwayThirdDownCompletions = awayTeamAwayOpponents.Select(x => x.HomeTeamThirdDownCompletions).Sum();
                var atOpponentHomeThirdDownCompletions = awayTeamHomeOpponents.Select(x => x.AwayTeamThirdDownCompletions).Sum();
                double atOpponentThirdDownCompletions = 0;
                var htOpponentAwayThirdDownCompletions = homeTeamAwayOpponents.Select(x => x.HomeTeamThirdDownCompletions).Sum();
                var htOpponentHomeThirdDownCompletions = homeTeamHomeOpponents.Select(x => x.AwayTeamThirdDownCompletions).Sum();
                double htOpponentThirdDownCompletions = 0;

                double atPastThirdDownConversions = 0;
                double htPastThirdDownConversions = 0;
                double atOpponentThirdDownConversions = 0;
                double htOpponentThirdDownConversions = 0;

                try
                {
                    atPastThirdDownAttempts = (atAwayPastThirdDownAttempts + atHomePastThirdDownAttempts) / awayTeamPastGames.Count();
                    atPastThirdDownCompletions = (atAwayPastThirdDownCompletions + atHomePastThirdDownCompletions) / awayTeamPastGames.Count();
                    atOpponentThirdDownAttempts = (atOpponentAwayThirdDownAttempts + atOpponentHomeThirdDownAttempts) / awayTeamPastGames.Count();
                    atOpponentThirdDownCompletions = (atOpponentAwayThirdDownCompletions + atOpponentHomeThirdDownCompletions) / awayTeamPastGames.Count();
                    htPastThirdDownAttempts = (htAwayPastThirdDownAttempts + htHomePastThirdDownAttempts) / homeTeamPastGames.Count();
                    htPastThirdDownCompletions = (htAwayPastThirdDownCompletions + htHomePastThirdDownCompletions) / homeTeamPastGames.Count();
                    htOpponentThirdDownAttempts = (htOpponentAwayThirdDownAttempts + htOpponentHomeThirdDownAttempts) / homeTeamPastGames.Count();
                    htOpponentThirdDownCompletions = (htOpponentAwayThirdDownCompletions + htOpponentHomeThirdDownCompletions) / homeTeamPastGames.Count();

                    atPastThirdDownConversions = atPastThirdDownCompletions / atPastThirdDownAttempts;
                    htPastThirdDownConversions = htPastThirdDownCompletions / htPastThirdDownAttempts;
                    atOpponentThirdDownConversions = atOpponentThirdDownCompletions / atOpponentThirdDownAttempts;
                    htOpponentThirdDownConversions = htOpponentThirdDownCompletions / htOpponentThirdDownAttempts;
                }
                catch { }
                #endregion

                #region Fourth Down Prediction
                var atAwayPastFourthDownAttempts = awayTeamPastGames.Where(x => x.AwayTeamId == requestModel.AwayTeamId).Select(x => x.AwayTeamFourthDownAttempts).Sum();
                var atHomePastFourthDownAttempts = awayTeamPastGames.Where(x => x.HomeTeamId == requestModel.AwayTeamId).Select(x => x.HomeTeamFourthDownAttempts).Sum();
                double atPastFourthDownAttempts = 0;
                var atAwayPastFourthDownCompletions = awayTeamPastGames.Where(x => x.AwayTeamId == requestModel.AwayTeamId).Select(x => x.AwayTeamFourthDownCompletions).Sum();
                var atHomePastFourthDownCompletions = awayTeamPastGames.Where(x => x.HomeTeamId == requestModel.AwayTeamId).Select(x => x.HomeTeamFourthDownCompletions).Sum();
                double atPastFourthDownCompletions = 0;

                var htAwayPastFourthDownAttempts = homeTeamPastGames.Where(x => x.AwayTeamId == requestModel.AwayTeamId).Select(x => x.AwayTeamFourthDownAttempts).Sum();
                var htHomePastFourthDownAttempts = homeTeamPastGames.Where(x => x.HomeTeamId == requestModel.AwayTeamId).Select(x => x.HomeTeamFourthDownAttempts).Sum();
                double htPastFourthDownAttempts = 0;
                var htAwayPastFourthDownCompletions = homeTeamPastGames.Where(x => x.AwayTeamId == requestModel.AwayTeamId).Select(x => x.AwayTeamFourthDownCompletions).Sum();
                var htHomePastFourthDownCompletions = homeTeamPastGames.Where(x => x.HomeTeamId == requestModel.AwayTeamId).Select(x => x.HomeTeamFourthDownCompletions).Sum();
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

                double atPastFourthDownConversions = 0;
                double htPastFourthDownConversions = 0;
                double atOpponentFourthDownConversions = 0;
                double htOpponentFourthDownConversions = 0;

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

                    atPastFourthDownConversions = atPastFourthDownCompletions / atPastFourthDownAttempts;
                    htPastFourthDownConversions = htPastFourthDownCompletions / htPastFourthDownAttempts;
                    atOpponentFourthDownConversions = atOpponentFourthDownCompletions / atOpponentFourthDownAttempts;
                    htOpponentFourthDownConversions = htOpponentFourthDownCompletions / htOpponentFourthDownAttempts;
                }
                catch { }
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


                var atAwayPassingAttempts = awayTeamAwayOpponents.Select(x => x.AwayTeamPassingAttempts).Sum();
                var atHomePassingAttempts = awayTeamHomeOpponents.Select(x => x.HomeTeamPassingAttempts).Sum();
                double atPassingAttempts = 0;
                var atAwayPassingCompletions = awayTeamAwayOpponents.Select(x => x.AwayTeamPassingCompletions).Sum();
                var atHomePassingCompletions = awayTeamHomeOpponents.Select(x => x.HomeTeamPassingCompletions).Sum();
                double atPassingCompletions = 0;


                var htAwayPassingAttempts = homeTeamAwayOpponents.Select(x => x.AwayTeamPassingAttempts).Sum();
                var htHomePassingAttempts = homeTeamHomeOpponents.Select(x => x.HomeTeamPassingAttempts).Sum();
                double htPassingAttempts = 0;
                var htAwayPassingCompletions = homeTeamAwayOpponents.Select(x => x.AwayTeamPassingCompletions).Sum();
                var htHomePassingCompletions = homeTeamHomeOpponents.Select(x => x.HomeTeamPassingCompletions).Sum();
                double htPassingCompletions = 0;


                var atAwayNetPassingYards = awayTeamAwayOpponents.Select(x => x.AwayTeamNetPassingYards).Sum();
                var atHomeNetPassingYards = awayTeamHomeOpponents.Select(x => x.HomeTeamNetPassingYards).Sum();
                double atNetPassingYards = 0;
                var htAwayNetPassingYards = homeTeamAwayOpponents.Select(x => x.AwayTeamNetPassingYards).Sum();
                var htHomeNetPassingYards = homeTeamHomeOpponents.Select(x => x.HomeTeamNetPassingYards).Sum();
                double htNetPassingYards = 0;

                try
                {
                    atOpponentPassingAttempts = (atOpponentAwayPassingAttempts + atOpponentHomePassingAttempts) / awayTeamPastGames.Count();
                    atOpponentPassingCompletions = (atOpponentAwayPassingCompletions + atOpponentHomePassingCompletions) / awayTeamPastGames.Count();
                    htOpponentPassingAttempts = (htOpponentAwayPassingAttempts + htOpponentHomePassingAttempts) / homeTeamPastGames.Count();
                    htOpponentPassingCompletions = (htOpponentAwayPassingCompletions + htOpponentHomePassingCompletions) / homeTeamPastGames.Count();
                    atPassingAttempts = (atAwayPassingAttempts + atHomePassingAttempts) / awayTeamPastGames.Count();
                    atPassingCompletions = (atAwayPassingCompletions + atHomePassingCompletions) / awayTeamPastGames.Count();
                    htPassingAttempts = (htAwayPassingAttempts + htHomePassingAttempts) / homeTeamPastGames.Count();
                    htPassingCompletions = (htAwayPassingCompletions + htHomePassingCompletions) / homeTeamPastGames.Count();

                    atNetPassingYards = (atAwayNetPassingYards + atHomeNetPassingYards) / awayTeamPastGames.Count();
                    htNetPassingYards = (htAwayNetPassingYards + htHomeNetPassingYards) / homeTeamPastGames.Count();
                }
                catch
                {

                }

                //var awayTeamPassingYardsPrediction = new PassingYardsModel.ModelInput()
                //{
                //    TeamOneId = requestModel.AwayTeamId,
                //    TeamOnePassingAttempts = (float)atPassingAttempts,
                //    TeamOnePassingCompletions = (float)atPassingCompletions,
                //    TeamTwoId = requestModel.HomeTeamId,
                //    TeamTwoOpponentPassingAttempts = (float)htOpponentPassingAttempts,
                //    TeamTwoOpponentPassingCompletions = (float)htOpponentPassingCompletions,
                //    Week = requestModel.Week,
                //    Year = requestModel.Year,
                //};

                //var homeTeamPassingYardsPrediction = new PassingYardsModel.ModelInput()
                //{
                //    TeamOneId = requestModel.HomeTeamId,
                //    TeamOnePassingAttempts = (float)htPassingAttempts,
                //    TeamOnePassingCompletions = (float)htPassingCompletions,
                //    TeamTwoId = requestModel.AwayTeamId,
                //    TeamTwoOpponentPassingAttempts = (float)atOpponentPassingAttempts,
                //    TeamTwoOpponentPassingCompletions = (float)atOpponentPassingCompletions,
                //    Week = requestModel.Week,
                //    Year = requestModel.Year,
                //};

                //var awayTeamPassingYards = PassingYardsModel.Predict(awayTeamPassingYardsPrediction);
                //var homeTeamPassingYards = PassingYardsModel.Predict(homeTeamPassingYardsPrediction);
                #endregion

                #region Yards Per Pass Prediction

                var atOpponentAwayYardsPerPass = awayTeamAwayOpponents.Select(x => x.HomeTeamYardsPerPass).Sum();
                var atOpponentHomeYardsPerPass = awayTeamHomeOpponents.Select(x => x.AwayTeamYardsPerPass).Sum();
                double atOpponentYardsPerPass = 0;
                var htOpponentAwayYardsPerPass = homeTeamAwayOpponents.Select(x => x.HomeTeamYardsPerPass).Sum();
                var htOpponentHomeYardsPerPass = homeTeamHomeOpponents.Select(x => x.AwayTeamYardsPerPass).Sum();
                double htOpponentYardsPerPass = 0;

                var atAwayYardsPerPass = awayTeamAwayOpponents.Select(x => x.AwayTeamYardsPerPass).Sum();
                var atHomeYardsPerPass = awayTeamHomeOpponents.Select(x => x.HomeTeamYardsPerPass).Sum();
                double atYardsPerPass = 0;
                var htAwayYardsPerPass = homeTeamAwayOpponents.Select(x => x.AwayTeamYardsPerPass).Sum();
                var htHomeYardsPerPass = homeTeamHomeOpponents.Select(x => x.HomeTeamYardsPerPass).Sum();
                double htYardsPerPass = 0;

                try
                {
                    atOpponentYardsPerPass = (atOpponentAwayPassingAttempts + atOpponentHomePassingAttempts) / awayTeamPastGames.Count();
                    htOpponentYardsPerPass = (htOpponentAwayYardsPerPass + htOpponentHomeYardsPerPass) / homeTeamPastGames.Count();
                    atYardsPerPass = (atAwayYardsPerPass + atHomeYardsPerPass) / awayTeamPastGames.Count();
                    htYardsPerPass = (htAwayYardsPerPass + htHomeYardsPerPass) / homeTeamPastGames.Count();
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

                var atAwayYardsPerRushAttempt = awayTeamAwayOpponents.Select(x => x.AwayTeamYardsPerRushAttempt).Sum();
                var atHomeYardsPerRushAttempt = awayTeamHomeOpponents.Select(x => x.HomeTeamYardsPerRushAttempt).Sum();
                double atYardsPerRushAttempt = 0;
                var atAwayRushingAttempts = awayTeamAwayOpponents.Select(x => x.AwayTeamRushingAttempts).Sum();
                var atHomeRushingAttempts = awayTeamHomeOpponents.Select(x => x.HomeTeamRushingAttempts).Sum();
                double atRushingAttempts = 0;

                var htAwayYardsPerRushAttempt = homeTeamAwayOpponents.Select(x => x.AwayTeamYardsPerRushAttempt).Sum();
                var htHomeYardsPerRushAttempt = homeTeamHomeOpponents.Select(x => x.HomeTeamYardsPerRushAttempt).Sum();
                double htYardsPerRushAttempt = 0;
                var htAwayRushingAttempts = homeTeamAwayOpponents.Select(x => x.AwayTeamRushingAttempts).Sum();
                var htHomeRushingAttempts = homeTeamHomeOpponents.Select(x => x.HomeTeamRushingAttempts).Sum();
                double htRushingAttempts = 0;


                var atAwayRushingYards = awayTeamAwayOpponents.Select(x => x.AwayTeamRushingYards).Sum();
                var atHomeRushingYards = awayTeamHomeOpponents.Select(x => x.HomeTeamRushingYards).Sum();
                double atRushingYards = 0;
                var htAwayRushingYards = homeTeamAwayOpponents.Select(x => x.AwayTeamRushingYards).Sum();
                var htHomeRushingYards = homeTeamHomeOpponents.Select(x => x.HomeTeamRushingYards).Sum();
                double htRushingYards = 0;
                try
                {
                    atOpponentYardsPerRushAttempt = (atOpponentAwayYardsPerRushAttempt + atOpponentHomeYardsPerRushAttempt) / awayTeamPastGames.Count();
                    atOpponentRushingAttempts = (atOpponentAwayRushingAttempts + atOpponentHomeRushingAttempts) / awayTeamPastGames.Count();
                    htOpponentYardsPerRushAttempt = (htOpponentAwayYardsPerRushAttempt + htOpponentHomeYardsPerRushAttempt) / homeTeamPastGames.Count();
                    htOpponentRushingAttempts = (htOpponentAwayRushingAttempts + htOpponentHomeRushingAttempts) / homeTeamPastGames.Count();

                    atYardsPerRushAttempt = (atAwayYardsPerRushAttempt + atHomeYardsPerRushAttempt) / awayTeamPastGames.Count();
                    atRushingAttempts = (atAwayRushingAttempts + atHomeRushingAttempts) / awayTeamPastGames.Count();
                    htYardsPerRushAttempt = (htAwayYardsPerRushAttempt + htHomeYardsPerRushAttempt) / homeTeamPastGames.Count();
                    htRushingAttempts = (htAwayRushingAttempts + htHomeRushingAttempts) / homeTeamPastGames.Count();

                    atRushingYards = (atAwayRushingYards + atHomeRushingYards) / awayTeamPastGames.Count();
                    htRushingYards = (htAwayRushingYards + htHomeRushingYards) / homeTeamPastGames.Count();
                }
                catch { }


                //var awayTeamRushingYardsPrediction = new RushingYardsModel.ModelInput()
                //{
                //    TeamOneId = requestModel.AwayTeamId,
                //    TeamOneRushingAttempts = (float)atRushingAttempts,
                //    TeamOneYardsPerRushAttempt = (float)atYardsPerRushAttempt,
                //    TeamTwoId = requestModel.HomeTeamId,
                //    TeamTwoOpponentRushingAttempts = (float)htOpponentRushingAttempts,
                //    TeamTwoOpponentYardsPerRushAttempt = (float)htOpponentYardsPerRushAttempt,
                //    Week = requestModel.Week,
                //    Year = requestModel.Year,
                //};

                //var homeTeamRushingYardsPrediction = new RushingYardsModel.ModelInput()
                //{
                //    TeamOneId = requestModel.HomeTeamId,
                //    TeamOneRushingAttempts = (float)htRushingAttempts,
                //    TeamOneYardsPerRushAttempt = (float)htYardsPerRushAttempt,
                //    TeamTwoId = requestModel.AwayTeamId,
                //    TeamTwoOpponentRushingAttempts = (float)atOpponentRushingAttempts,
                //    TeamTwoOpponentYardsPerRushAttempt = (float)atOpponentYardsPerRushAttempt,
                //    Week = requestModel.Week,
                //    Year = requestModel.Year,
                //};


                //var awayTeamRushingYards = RushingYardsModel.Predict(awayTeamRushingYardsPrediction);
                //var homeTeamRushingYards = RushingYardsModel.Predict(homeTeamRushingYardsPrediction);

                #endregion

                #region Fumbles Prediction

                var atOpponentAwayFumblesLost = awayTeamAwayOpponents.Select(x => x.HomeTeamFumblesLost).Sum();
                var atOpponentHomeFumblesLost = awayTeamHomeOpponents.Select(x => x.AwayTeamFumblesLost).Sum();
                double atOpponentFumblesLost = 0;
                var atAwayFumbles = awayTeamAwayOpponents.Select(x => x.AwayTeamFumblesLost).Sum();
                var atHomeFumbles = awayTeamHomeOpponents.Select(x => x.HomeTeamFumblesLost).Sum();
                double atFumbles = 0;


                var htOpponentAwayFumblesLost = homeTeamAwayOpponents.Select(x => x.HomeTeamFumblesLost).Sum();
                var htOpponentHomeFumblesLost = homeTeamHomeOpponents.Select(x => x.AwayTeamFumblesLost).Sum();
                double htOpponentFumblesLost = 0;
                var htAwayFumbles = homeTeamAwayOpponents.Select(x => x.AwayTeamFumblesLost).Sum();
                var htHomeFumbles = homeTeamHomeOpponents.Select(x => x.HomeTeamFumblesLost).Sum();
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
                var atAwayInterceptions = awayTeamAwayOpponents.Select(x => x.AwayTeamFumblesLost).Sum();
                var atHomeInterceptions = awayTeamHomeOpponents.Select(x => x.HomeTeamFumblesLost).Sum();
                double atInterceptions = 0;

                var htOpponentAwayInterceptionsLost = homeTeamAwayOpponents.Select(x => x.HomeTeamInterceptions).Sum();
                var htOpponentHomeInterceptionsLost = homeTeamHomeOpponents.Select(x => x.AwayTeamInterceptions).Sum();
                double htOpponentInterceptionsLost = 0;
                var htAwayInterceptions = homeTeamAwayOpponents.Select(x => x.AwayTeamFumblesLost).Sum();
                var htHomeInterceptions = homeTeamHomeOpponents.Select(x => x.HomeTeamFumblesLost).Sum();
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

                #region Inner Region Penalties/Plays
                var atAwayTotalPenalties = awayTeamAwayOpponents.Select(x => x.AwayTeamTotalPenalties).Sum();
                var atHomeTotalPenalties = awayTeamHomeOpponents.Select(x => x.HomeTeamTotalPenalties).Sum();
                double atTotalPenalties = 0;
                var atAwayTotalPenaltyYards = awayTeamAwayOpponents.Select(x => x.AwayTeamTotalPenaltyYards).Sum();
                var atHomeTotalPenaltyYards = awayTeamHomeOpponents.Select(x => x.HomeTeamTotalPenaltyYards).Sum();
                double atTotalPenaltyYards = 0;
                var atAwayPositivePlays = awayTeamAwayOpponents.Select(x => x.AwayTeamPositivePlays).Sum();
                var atHomePositivePlays = awayTeamHomeOpponents.Select(x => x.HomeTeamPositivePlays).Sum();
                double atTotalPositivePlays = 0;
                var atAwayNegativePlays = awayTeamAwayOpponents.Select(x => x.AwayTeamNegativePlays).Sum();
                var atHomeNegativePlays = awayTeamHomeOpponents.Select(x => x.HomeTeamNegativePlays).Sum();
                double atTotalNegativePlays = 0;
                var atAwayPossessionTime = awayTeamAwayOpponents.Select(x => x.AwayTeamPossessionTime).Sum();
                var atHomePossessionTime = awayTeamHomeOpponents.Select(x => x.HomeTeamPossessionTime).Sum();
                double atTotalPossessionTime = 0;


                var htAwayTotalPenalties = homeTeamAwayOpponents.Select(x => x.AwayTeamTotalPenalties).Sum();
                var htHomeTotalPenalties = homeTeamHomeOpponents.Select(x => x.HomeTeamTotalPenalties).Sum();
                double htTotalPenalties = 0;
                var htAwayTotalPenaltyYards = homeTeamAwayOpponents.Select(x => x.AwayTeamTotalPenaltyYards).Sum();
                var htHomeTotalPenaltyYards = homeTeamHomeOpponents.Select(x => x.HomeTeamTotalPenaltyYards).Sum();
                double htTotalPenaltyYards = 0;
                var htAwayPositivePlays = homeTeamAwayOpponents.Select(x => x.AwayTeamPositivePlays).Sum();
                var htHomePositivePlays = homeTeamHomeOpponents.Select(x => x.HomeTeamPositivePlays).Sum();
                double htTotalPositivePlays = 0;
                var htAwayNegativePlays = homeTeamAwayOpponents.Select(x => x.AwayTeamNegativePlays).Sum();
                var htHomeNegativePlays = homeTeamHomeOpponents.Select(x => x.HomeTeamNegativePlays).Sum();
                double htTotalNegativePlays = 0;
                var htAwayPossessionTime = homeTeamAwayOpponents.Select(x => x.AwayTeamPossessionTime).Sum();
                var htHomePossessionTime = homeTeamHomeOpponents.Select(x => x.HomeTeamPossessionTime).Sum();
                double htTotalPossessionTime = 0;
                #endregion

                #region Inner Region Downs
                double atHomeFirstDownsAllowed = awayTeamHomeOpponents.Select(x => x.AwayTeamFirstDowns).Sum();
                double atAwayFirstDownsAllowed = awayTeamAwayOpponents.Select(x => x.HomeTeamFirstDowns).Sum();
                double atHomeThirdDownAttemptsAllowed = awayTeamHomeOpponents.Select(x => x.AwayTeamThirdDownAttempts).Sum();
                double atAwayThirdDownAttemptsAllowed = awayTeamAwayOpponents.Select(x => x.HomeTeamThirdDownAttempts).Sum();
                double atHomeThirdDownCompletionsAllowed = awayTeamHomeOpponents.Select(x => x.AwayTeamThirdDownCompletions).Sum();
                double atAwayThirdDownCompletionsAllowed = awayTeamAwayOpponents.Select(x => x.HomeTeamThirdDownCompletions).Sum();
                double atHomeFourthDownAttemptsAllowed = awayTeamHomeOpponents.Select(x => x.AwayTeamFourthDownAttempts).Sum();
                double atAwayFourthDownAttemptsAllowed = awayTeamAwayOpponents.Select(x => x.HomeTeamFourthDownAttempts).Sum();
                double atHomeFourthDownCompletionsAllowed = awayTeamHomeOpponents.Select(x => x.AwayTeamFourthDownCompletions).Sum();
                double atAwayFourthDownCompletionsAllowed = awayTeamAwayOpponents.Select(x => x.HomeTeamFourthDownCompletions).Sum();
                double atFirstDownsAllowed = 0;
                double atThirdDownConversion = 0;
                double atFourthDownConversion = 0;

                double htHomeFirstDownsAllowed = homeTeamHomeOpponents.Select(x => x.AwayTeamFirstDowns).Sum();
                double htAwayFirstDownsAllowed = homeTeamAwayOpponents.Select(x => x.HomeTeamFirstDowns).Sum();
                double htHomeThirdDownAttemptsAllowed = homeTeamHomeOpponents.Select(x => x.AwayTeamThirdDownAttempts).Sum();
                double htAwayThirdDownAttemptsAllowed = homeTeamAwayOpponents.Select(x => x.HomeTeamThirdDownAttempts).Sum();
                double htHomeThirdDownCompletionsAllowed = homeTeamHomeOpponents.Select(x => x.AwayTeamThirdDownCompletions).Sum();
                double htAwayThirdDownCompletionsAllowed = homeTeamAwayOpponents.Select(x => x.HomeTeamThirdDownCompletions).Sum();
                double htHomeFourthDownAttemptsAllowed = homeTeamHomeOpponents.Select(x => x.AwayTeamFourthDownAttempts).Sum();
                double htAwayFourthDownAttemptsAllowed = homeTeamAwayOpponents.Select(x => x.HomeTeamFourthDownAttempts).Sum();
                double htHomeFourthDownCompletionsAllowed = homeTeamHomeOpponents.Select(x => x.AwayTeamFourthDownCompletions).Sum();
                double htAwayFourthDownCompletionsAllowed = homeTeamAwayOpponents.Select(x => x.HomeTeamFourthDownCompletions).Sum();
                double htFirstDownsAllowed = 0;
                double htThirdDownConversion = 0;
                double htFourthDownConversion = 0;
                #endregion

                #region Inner Region Yards
                double atAwayPassingYardsAllowed = awayTeamAwayOpponents.Select(x => x.HomeTeamNetPassingYards).Sum();
                double atHomePassingYardsAllowed = awayTeamHomeOpponents.Select(x => x.AwayTeamNetPassingYards).Sum();
                double atAwayRushingYardsAllowed = awayTeamAwayOpponents.Select(x => x.HomeTeamRushingYards).Sum();
                double atHomeRushingYardsAllowed = awayTeamHomeOpponents.Select(x => x.AwayTeamRushingYards).Sum();
                double atPassingYardsAllowed = 0;
                double atRushingYardsAllowed = 0;
                double atTotalYardsAllowed = 0;

                double htAwayPassingYardsAllowed = homeTeamAwayOpponents.Select(x => x.HomeTeamNetPassingYards).Sum();
                double htHomePassingYardsAllowed = homeTeamHomeOpponents.Select(x => x.AwayTeamNetPassingYards).Sum();
                double htAwayRushingYardsAllowed = homeTeamAwayOpponents.Select(x => x.HomeTeamRushingYards).Sum();
                double htHomeRushingYardsAllowed = homeTeamHomeOpponents.Select(x => x.AwayTeamRushingYards).Sum();
                double htPassingYardsAllowed = 0;
                double htRushingYardsAllowed = 0;
                double htTotalYardsAllowed = 0;
                #endregion

                #region Conferences and Misc Yards/Down Conversions/etc
                try
                {
                    atTotalPenalties = (atAwayTotalPenalties + atHomeTotalPenalties) / awayTeamPastGames.Count();
                    atTotalPenaltyYards = (atAwayTotalPenaltyYards + atHomeTotalPenaltyYards) / awayTeamPastGames.Count();
                    atTotalPositivePlays = (atAwayPositivePlays + atHomePositivePlays) / awayTeamPastGames.Count();
                    atTotalNegativePlays = (atAwayNegativePlays + atHomeNegativePlays) / awayTeamPastGames.Count();
                    atTotalPossessionTime = (atAwayPossessionTime + atHomePossessionTime) / awayTeamPastGames.Count();

                    htTotalPenalties = (htAwayTotalPenalties + htHomeTotalPenalties) / homeTeamPastGames.Count();
                    htTotalPenaltyYards = (htAwayTotalPenaltyYards + htHomeTotalPenaltyYards) / homeTeamPastGames.Count();
                    htTotalPositivePlays = (htAwayPositivePlays + htHomePositivePlays) / homeTeamPastGames.Count();
                    htTotalNegativePlays = (htAwayNegativePlays + htHomeNegativePlays) / homeTeamPastGames.Count();
                    htTotalPossessionTime = (htAwayPossessionTime + htHomePossessionTime) / homeTeamPastGames.Count();

                    atFirstDownsAllowed = (atAwayFirstDownsAllowed + atHomeFirstDownsAllowed) / awayTeamPastGames.Count();
                    htFirstDownsAllowed = (htAwayFirstDownsAllowed + htHomeFirstDownsAllowed) / homeTeamPastGames.Count();

                    atPassingYardsAllowed = (atAwayPassingYardsAllowed + atHomePassingYardsAllowed) / awayTeamPastGames.Count();
                    atRushingYardsAllowed = (atAwayRushingYardsAllowed + atHomeRushingYardsAllowed) / awayTeamPastGames.Count();
                    atTotalYardsAllowed = atPassingYardsAllowed + atRushingYardsAllowed;
                    htPassingYardsAllowed = (htAwayPassingYardsAllowed + htHomePassingYardsAllowed) / homeTeamPastGames.Count();
                    htRushingYardsAllowed = (htAwayRushingYardsAllowed + htHomeRushingYardsAllowed) / homeTeamPastGames.Count();
                    htTotalYardsAllowed = htPassingYardsAllowed + htRushingYardsAllowed;
                }
                catch { }

                var awayTeamConference = await (from c in db.ConferenceTeam
                                                where c.TeamId == awayTeam.TeamId
                                                select c.TeamConference).FirstAsync();

                var homeTeamConference = await (from c in db.ConferenceTeam
                                                where c.TeamId == homeTeam.TeamId
                                                select c.TeamConference).FirstAsync();
                #endregion

                #region Wins / Losses
                var awayTeamAwayWins = awayTeamAwayOpponents.Where(x => x.AwayTeamWon).Count();
                var awayTeamHomeWins = awayTeamHomeOpponents.Where(x => x.HomeTeamWon).Count();
                var homeTeamAwayWins = homeTeamAwayOpponents.Where(x => x.AwayTeamWon).Count();
                var homeTeamHomeWins = homeTeamHomeOpponents.Where(x => x.HomeTeamWon).Count();

                var awayTeamAwayLosses = awayTeamAwayOpponents.Where(x => x.HomeTeamWon).Count();
                var awayTeamHomeLosses = awayTeamHomeOpponents.Where(x => x.AwayTeamWon).Count();
                var homeTeamAwayLosses = homeTeamAwayOpponents.Where(x => x.HomeTeamWon).Count();
                var homeTeamHomeLosses = homeTeamHomeOpponents.Where(x => x.AwayTeamWon).Count();

                double awayTeamAwayChanceToWin = 0.0;
                double awayTeamHomeChanceToWin = 0.0;
                double homeTeamAwayChanceToWin = 0.0;
                double homeTeamHomeChanceToWin = 0.0;

                var awayTeamWins = awayTeamAwayWins + awayTeamHomeWins;
                var homeTeamWins = homeTeamAwayWins + homeTeamHomeWins;
                var awayTeamLosses = awayTeamAwayLosses + awayTeamHomeLosses;
                var homeTeamLosses = homeTeamAwayLosses + homeTeamHomeLosses;
                var awayTeamChanceToWin = 0.0;
                var homeTeamChanceToWin = 0.0;

                try
                {
                    awayTeamAwayChanceToWin = (double)awayTeamAwayOpponents.Select(x => x.AwayTeamChanceToWin).Sum()!;
                    awayTeamHomeChanceToWin = (double)awayTeamHomeOpponents.Select(x => x.HomeTeamChanceToWin).Sum()!;
                    homeTeamAwayChanceToWin = (double)homeTeamAwayOpponents.Select(x => x.AwayTeamChanceToWin).Sum()!;
                    homeTeamHomeChanceToWin = (double)homeTeamHomeOpponents.Select(x => x.HomeTeamChanceToWin).Sum()!;

                    awayTeamChanceToWin = (awayTeamAwayChanceToWin + awayTeamHomeChanceToWin) / awayTeamPastGames.Count();
                    homeTeamChanceToWin = (homeTeamAwayChanceToWin + homeTeamHomeChanceToWin) / homeTeamPastGames.Count();
                }
                catch { }
                #endregion

                #region OpponentDefensiveTacklesForLoss

                var awayTeamAwayDefensiveTacklesForLoss = awayTeamAwayOpponents.Select(x => x.AwayTeamDefensiveTacklesForLoss).Sum();
                var awayTeamHomeDefensiveTacklesForLoss = awayTeamHomeOpponents.Select(x => x.HomeTeamDefensiveTacklesForLoss).Sum();

                var homeTeamAwayDefensiveTacklesForLoss = homeTeamAwayOpponents.Select(x => x.AwayTeamDefensiveTacklesForLoss).Sum();
                var homeTeamHomeDefensiveTacklesForLoss = homeTeamHomeOpponents.Select(x => x.HomeTeamDefensiveTacklesForLoss).Sum();

                var awayTeamDefensiveTacklesForLoss = 0.0;
                var homeTeamDefensiveTacklesForLoss = 0.0;

                try
                {
                    if (awayTeamAwayDefensiveTacklesForLoss > 0.0 && awayTeamHomeDefensiveTacklesForLoss > 0)
                    {
                        awayTeamDefensiveTacklesForLoss = (awayTeamAwayDefensiveTacklesForLoss + awayTeamHomeDefensiveTacklesForLoss) / awayTeamPastGames.Count();
                    }
                    if (homeTeamAwayDefensiveTacklesForLoss > 0.0 && homeTeamHomeDefensiveTacklesForLoss > 0)
                    {
                        homeTeamDefensiveTacklesForLoss = (homeTeamAwayDefensiveTacklesForLoss + homeTeamHomeDefensiveTacklesForLoss) / homeTeamPastGames.Count();
                    }
                }
                catch { }
                #endregion

                #region OpponentDefensiveSacks
                var awayTeamAwayDefensiveSacks = awayTeamAwayOpponents.Select(x => x.AwayTeamDefensiveSacks).Sum();
                var awayTeamHomeDefensiveSacks = awayTeamHomeOpponents.Select(x => x.HomeTeamDefensiveSacks).Sum();

                var homeTeamAwayDefensiveSacks = homeTeamAwayOpponents.Select(x => x.AwayTeamDefensiveSacks).Sum();
                var homeTeamHomeDefensiveSacks = homeTeamHomeOpponents.Select(x => x.HomeTeamDefensiveSacks).Sum();

                var awayTeamDefensiveSacks = 0.0;
                var homeTeamDefensiveSacks = 0.0;

                try
                {
                    if (awayTeamAwayDefensiveSacks > 0.0 && awayTeamHomeDefensiveSacks > 0)
                    {
                        awayTeamDefensiveSacks = (awayTeamAwayDefensiveSacks + awayTeamHomeDefensiveSacks) / awayTeamPastGames.Count();
                    }
                    if (homeTeamAwayDefensiveSacks > 0.0 && homeTeamHomeDefensiveSacks > 0)
                    {
                        homeTeamDefensiveSacks = (homeTeamAwayDefensiveSacks + homeTeamHomeDefensiveSacks) / homeTeamPastGames.Count();
                    }
                }
                catch { }
                #endregion

                #region Field Goals
                var awayTeamAwayFieldGoalsAllowed = awayTeamAwayOpponents.Select(x => x.HomeTeamFieldGoals).Sum();
                var awayTeamHomeFieldGoalsAllowed = awayTeamHomeOpponents.Select(x => x.AwayTeamFieldGoals).Sum();
                var homeTeamAwayFieldGoalsAllowed = homeTeamAwayOpponents.Select(x => x.HomeTeamFieldGoals).Sum();
                var homeTeamHomeFieldGoalsAllowed = homeTeamHomeOpponents.Select(x => x.AwayTeamFieldGoals).Sum();

                var awayTeamFieldGoalsAllowed = 0.0;
                var homeTeamFieldGoalsAllowed = 0.0;

                var awayTeamAwayFieldGoals = awayTeamAwayOpponents.Select(x => x.AwayTeamFieldGoals).Sum();
                var awayTeamHomeFieldGoals = awayTeamHomeOpponents.Select(x => x.HomeTeamFieldGoals).Sum();
                var homeTeamAwayFieldGoals = homeTeamAwayOpponents.Select(x => x.AwayTeamFieldGoals).Sum();
                var homeTeamHomeFieldGoals = homeTeamHomeOpponents.Select(x => x.HomeTeamFieldGoals).Sum();

                var awayTeamFieldGoals = 0.0;
                var homeTeamFieldGoals = 0.0;

                try
                {
                    if (awayTeamAwayFieldGoalsAllowed > 0.0 && awayTeamHomeFieldGoalsAllowed > 0)
                    {
                        awayTeamFieldGoalsAllowed = (awayTeamAwayFieldGoalsAllowed + awayTeamHomeFieldGoalsAllowed) / awayTeamPastGames.Count();
                    }
                    if (homeTeamAwayFieldGoalsAllowed > 0.0 && homeTeamFieldGoalsAllowed > 0)
                    {
                        homeTeamFieldGoalsAllowed = (homeTeamAwayFieldGoalsAllowed + homeTeamFieldGoalsAllowed) / homeTeamPastGames.Count();
                    }
                    if (awayTeamAwayFieldGoals > 0.0 && awayTeamHomeFieldGoals > 0)
                    {
                        awayTeamFieldGoals = (awayTeamAwayFieldGoals + awayTeamHomeFieldGoals) / awayTeamPastGames.Count();
                    }
                    if (homeTeamAwayFieldGoals > 0.0 && homeTeamFieldGoals > 0)
                    {
                        homeTeamFieldGoals = (homeTeamAwayFieldGoals + homeTeamFieldGoals) / homeTeamPastGames.Count();
                    }
                }
                catch { }
                #endregion

                #region OpponentDefensiveSafety
                var awayTeamAwayDefensiveSafety = awayTeamAwayOpponents.Select(x => x.AwayTeamDefensiveSafety).Sum();
                var awayTeamHomeDefensiveSafety = awayTeamHomeOpponents.Select(x => x.HomeTeamDefensiveSafety).Sum();

                var homeTeamAwayDefensiveSafety = homeTeamAwayOpponents.Select(x => x.AwayTeamDefensiveSafety).Sum();
                var homeTeamHomeDefensiveSafety = homeTeamHomeOpponents.Select(x => x.HomeTeamDefensiveSafety).Sum();

                var awayTeamDefensiveSafety = 0.0;
                var homeTeamDefensiveSafety = 0.0;

                try
                {
                    if (awayTeamAwayDefensiveSafety > 0.0 && awayTeamHomeDefensiveSafety > 0)
                    {
                        awayTeamDefensiveSafety = (awayTeamAwayDefensiveSafety + awayTeamHomeDefensiveSafety) / awayTeamPastGames.Count();
                    }
                    if (homeTeamAwayDefensiveSafety > 0.0 && homeTeamHomeDefensiveSafety > 0)
                    {
                        homeTeamDefensiveSafety = (homeTeamAwayDefensiveSafety + homeTeamHomeDefensiveSafety) / homeTeamPastGames.Count();
                    }
                }
                catch { }
                #endregion

                #region OpponentSpecialTeamsPointsAllowed
                var awayTeamAwaySpecialTeamsPointsAllowed = awayTeamAwayOpponents.Select(x => x.HomeTeamSpecialTeamsPoints).Sum();
                var awayTeamHomeSpecialTeamsPointsAllowed = awayTeamHomeOpponents.Select(x => x.AwayTeamSpecialTeamsPoints).Sum();
                var homeTeamAwaySpecialTeamsPointsAllowed = homeTeamAwayOpponents.Select(x => x.HomeTeamSpecialTeamsPoints).Sum();
                var homeTeamHomeSpecialTeamsPointsAllowed = homeTeamHomeOpponents.Select(x => x.AwayTeamSpecialTeamsPoints).Sum();

                var awayTeamSpecialTeamsPointsAllowed = 0.0;
                var homeTeamSpecialTeamsPointsAllowed = 0.0;

                var awayTeamAwaySpecialTeamsPoints = awayTeamAwayOpponents.Select(x => x.AwayTeamSpecialTeamsPoints).Sum();
                var awayTeamHomeSpecialTeamsPoints = awayTeamHomeOpponents.Select(x => x.HomeTeamSpecialTeamsPoints).Sum();
                var homeTeamAwaySpecialTeamsPoints = homeTeamAwayOpponents.Select(x => x.AwayTeamSpecialTeamsPoints).Sum();
                var homeTeamHomeSpecialTeamsPoints = homeTeamHomeOpponents.Select(x => x.HomeTeamSpecialTeamsPoints).Sum();

                var awayTeamSpecialTeamsPoints = 0.0;
                var homeTeamSpecialTeamsPoints = 0.0;

                try
                {
                    if (awayTeamAwaySpecialTeamsPointsAllowed > 0.0 && awayTeamHomeSpecialTeamsPointsAllowed > 0)
                    {
                        awayTeamSpecialTeamsPointsAllowed = (awayTeamAwaySpecialTeamsPointsAllowed + awayTeamHomeSpecialTeamsPointsAllowed) / awayTeamPastGames.Count();
                    }
                    if (homeTeamAwaySpecialTeamsPointsAllowed > 0.0 && homeTeamSpecialTeamsPointsAllowed > 0)
                    {
                        homeTeamSpecialTeamsPointsAllowed = (homeTeamAwaySpecialTeamsPointsAllowed + homeTeamSpecialTeamsPointsAllowed) / homeTeamPastGames.Count();
                    }
                    if (awayTeamAwaySpecialTeamsPoints > 0.0 && awayTeamHomeSpecialTeamsPoints > 0)
                    {
                        awayTeamSpecialTeamsPoints = (awayTeamAwaySpecialTeamsPoints + awayTeamHomeSpecialTeamsPoints) / awayTeamPastGames.Count();
                    }
                    if (homeTeamAwaySpecialTeamsPoints > 0.0 && homeTeamSpecialTeamsPoints > 0)
                    {
                        homeTeamSpecialTeamsPoints = (homeTeamAwaySpecialTeamsPoints + homeTeamSpecialTeamsPoints) / homeTeamPastGames.Count();
                    }
                }
                catch { }
                #endregion

                #region OpponentPointsPerPlay
                var awayTeamPointsPerPlay = 0.0;
                var homeTeamPointsPerPlay = 0.0;
                var awayTeamAwayPlays = awayTeamAwayOpponents.Select(x => x.AwayTeamNegativePlays + x.AwayTeamPositivePlays).Sum();
                var awayTeamHomePlays = awayTeamHomeOpponents.Select(x => x.HomeTeamNegativePlays + x.HomeTeamPositivePlays).Sum();
                var awayTeamAwayScore = awayTeamAwayOpponents.Select(x => x.AwayTeamScore).Sum();
                var awayTeamHomeScore = awayTeamHomeOpponents.Select(x => x.HomeTeamScore).Sum();
                var homeTeamAwayPlays = homeTeamAwayOpponents.Select(x => x.AwayTeamNegativePlays + x.AwayTeamPositivePlays).Sum();
                var homeTeamHomePlays = homeTeamHomeOpponents.Select(x => x.HomeTeamNegativePlays + x.HomeTeamPositivePlays).Sum();
                var homeTeamAwayScore = homeTeamAwayOpponents.Select(x => x.AwayTeamScore).Sum();
                var homeTeamHomeScore = homeTeamHomeOpponents.Select(x => x.HomeTeamScore).Sum();
                try
                {
                    awayTeamPointsPerPlay = (awayTeamHomeScore + awayTeamAwayScore) / (awayTeamAwayPlays + awayTeamHomePlays);
                    homeTeamPointsPerPlay = (homeTeamHomeScore + homeTeamAwayScore) / (homeTeamAwayPlays + homeTeamHomePlays);
                }
                catch { }
                #endregion

                #region OpponentTurnoverMargin
                var awayTeamTurnoverMargin = (htFumbles + htInterceptions) - (atFumbles + atInterceptions);
                var homeTeamTurnoverMargin = (atFumbles + atInterceptions) - (htFumbles + htInterceptions);

                #endregion

                #region Win Prediction
                var awayTeamWinPrediction = new WillTeamWin.ModelInput()
                {
                    IsConference = requestModel.IsConference,
                    IsNeutralSite = requestModel.IsNeutralSite,
                    Week = requestModel.Week,
                    Year = requestModel.Year,

                    TeamId = requestModel.AwayTeamId,
                    TeamConference = (int)awayTeamConference,
                    TeamWins = (float)awayTeamWins,
                    TeamLosses = (float)awayTeamLosses,
                    //      TeamScore = (float)awayTeamNoDefense.Score,
                    TeamChanceToWin = (float)awayTeamChanceToWin,
                    TeamFirstDowns = (float)atPastFirstDowns,
                    TeamTotalYards = (float)atRushingYards + (float)atNetPassingYards,
                    TeamNetPassingYards = (float)atNetPassingYards,
                    TeamYardsPerPass = (float)atYardsPerPass,
                    TeamPassingAttempts = (float)atPassingAttempts,
                    TeamRushingYards = (float)atRushingYards,
                    TeamRushingAttempts = (float)atRushingAttempts,
                    TeamYardsPerRushAttempt = (float)atYardsPerRushAttempt,
                    TeamTotalPenalties = (float)atTotalPenalties,
                    TeamTotalPenaltyYards = (float)atTotalPenaltyYards,
                    TeamFumblesLost = (float)atFumbles,
                    TeamInterceptions = (float)atInterceptions,
                    TeamPossessionTime = (float)atTotalPossessionTime,
                    TeamPositivePlays = (float)atTotalPositivePlays,
                    TeamNegativePlays = (float)atTotalNegativePlays,
                    TeamDefensiveTacklesForLoss = (float)awayTeamDefensiveTacklesForLoss,
                    TeamDefensiveSacks = (float)awayTeamDefensiveSacks,
                    TeamFieldGoals = (float)awayTeamFieldGoals,
                    TeamDefensiveSafety = (float)awayTeamDefensiveSafety,
                    TeamSpecialTeamsPoints = (float)awayTeamSpecialTeamsPoints,
                    TeamPointsPerPlay = (float)awayTeamPointsPerPlay,
                    TeamTurnoverMargin = (float)awayTeamTurnoverMargin,

                    OpponentTeamId = requestModel.HomeTeamId,
                    OpponentConference = (int)homeTeamConference,
                    OpponentWins = (float)homeTeamWins,
                    OpponentLosses = (float)homeTeamLosses,
                    //      OpponentScore = (float)homeTeamNoDefense.Score,
                    OpponentFirstDownsAllowed = (float)htFirstDownsAllowed,
                    OpponentTotalYardsAllowed = (float)htTotalYardsAllowed,
                    OpponentPassingYardsAllowed = (float)htPassingYardsAllowed,
                    OpponentRushingYardsAllowed = (float)htRushingYardsAllowed,
                    OpponentYardsPerPassAllowed = (float)htOpponentYardsPerPass,
                    OpponentYardsPerRushAttemptAllowed = (float)htOpponentYardsPerRushAttempt,
                    OpponentTotalPenalties = (float)htTotalPenalties,
                    OpponentTotalPenaltyYards = (float)htAwayTotalPenaltyYards,
                    OpponentFumblesForced = (float)htOpponentFumblesLost,
                    OpponentInterceptionsForced = (float)htOpponentInterceptionsLost,
                    OpponentPossessionTime = (float)htTotalPossessionTime,
                    OpponentPositivePlays = (float)htTotalPositivePlays,
                    OpponentNegativePlays = (float)htTotalNegativePlays,
                    OpponentDefensiveTacklesForLoss = (float)homeTeamDefensiveTacklesForLoss,
                    OpponentDefensiveSacks = (float)homeTeamDefensiveSacks,
                    OpponentFieldGoalsAllowed = (float)homeTeamFieldGoalsAllowed,
                    OpponentDefensiveSafety = (float)homeTeamDefensiveSafety,
                    OpponentSpecialTeamsPointsAllowed = (float)homeTeamSpecialTeamsPointsAllowed,
                    OpponentPointsPerPlay = (float)homeTeamPointsPerPlay,
                    OpponentTurnoverMargin = (float)homeTeamTurnoverMargin,
                };
                var homeTeamWinPrediction = new WillTeamWin.ModelInput()
                {
                    IsConference = requestModel.IsConference,
                    IsNeutralSite = requestModel.IsNeutralSite,
                    Week = requestModel.Week,
                    Year = requestModel.Year,

                    TeamId = requestModel.HomeTeamId,
                    TeamConference = (int)homeTeamConference,
                    TeamWins = (float)homeTeamWins,
                    TeamLosses = (float)homeTeamLosses,
                    //   TeamScore = (float)homeTeamNoDefense.Score,
                    TeamChanceToWin = (float)homeTeamChanceToWin,
                    TeamFirstDowns = (float)htPastFirstDowns,
                    TeamTotalYards = (float)htRushingYards + (float)htNetPassingYards,
                    TeamNetPassingYards = (float)htNetPassingYards,
                    TeamYardsPerPass = (float)htYardsPerPass,
                    TeamPassingAttempts = (float)htPassingAttempts,
                    TeamRushingYards = (float)htRushingYards,
                    TeamRushingAttempts = (float)htRushingAttempts,
                    TeamYardsPerRushAttempt = (float)htYardsPerRushAttempt,
                    TeamTotalPenalties = (float)htTotalPenalties,
                    TeamTotalPenaltyYards = (float)htTotalPenaltyYards,
                    TeamFumblesLost = (float)htFumbles,
                    TeamInterceptions = (float)htInterceptions,
                    TeamPossessionTime = (float)htTotalPossessionTime,
                    TeamPositivePlays = (float)htTotalPositivePlays,
                    TeamNegativePlays = (float)htTotalNegativePlays,
                    TeamDefensiveTacklesForLoss = (float)homeTeamDefensiveTacklesForLoss,
                    TeamDefensiveSacks = (float)homeTeamDefensiveSacks,
                    TeamFieldGoals = (float)homeTeamFieldGoals,
                    TeamDefensiveSafety = (float)homeTeamDefensiveSafety,
                    TeamSpecialTeamsPoints = (float)homeTeamSpecialTeamsPoints,
                    TeamPointsPerPlay = (float)homeTeamPointsPerPlay,
                    TeamTurnoverMargin = (float)homeTeamTurnoverMargin,

                    OpponentTeamId = requestModel.AwayTeamId,
                    OpponentConference = (int)awayTeamConference,
                    OpponentWins = (float)awayTeamWins,
                    OpponentLosses = (float)awayTeamLosses,
                    //  OpponentScore = (float)awayTeamNoDefense.Score,
                    OpponentFirstDownsAllowed = (float)atFirstDownsAllowed,
                    OpponentTotalYardsAllowed = (float)atTotalYardsAllowed,
                    OpponentPassingYardsAllowed = (float)atPassingYardsAllowed,
                    OpponentRushingYardsAllowed = (float)atRushingYardsAllowed,
                    OpponentYardsPerPassAllowed = (float)atOpponentYardsPerPass,
                    OpponentYardsPerRushAttemptAllowed = (float)atOpponentYardsPerRushAttempt,
                    OpponentTotalPenalties = (float)atTotalPenalties,
                    OpponentTotalPenaltyYards = (float)atAwayTotalPenaltyYards,
                    OpponentFumblesForced = (float)atOpponentFumblesLost,
                    OpponentInterceptionsForced = (float)atOpponentInterceptionsLost,
                    OpponentPossessionTime = (float)atTotalPossessionTime,
                    OpponentPositivePlays = (float)atTotalPositivePlays,
                    OpponentNegativePlays = (float)atTotalNegativePlays,
                    OpponentDefensiveTacklesForLoss = (float)awayTeamDefensiveTacklesForLoss,
                    OpponentDefensiveSacks = (float)awayTeamDefensiveSacks,
                    OpponentFieldGoalsAllowed = (float)awayTeamFieldGoalsAllowed,
                    OpponentDefensiveSafety = (float)awayTeamDefensiveSafety,
                    OpponentSpecialTeamsPointsAllowed = (float)awayTeamSpecialTeamsPointsAllowed,
                    OpponentPointsPerPlay = (float)awayTeamPointsPerPlay,
                    OpponentTurnoverMargin = (float)awayTeamTurnoverMargin,
                };
                var awayTeamWin = WillTeamWin.Predict(awayTeamWinPrediction);
                var homeTeamWin = WillTeamWin.Predict(homeTeamWinPrediction);
                #endregion

                #region Save Data
                var model = new YearlyWinDbo
                {
                    AwayTeamId = requestModel.AwayTeamId,
                    HomeTeamId = requestModel.HomeTeamId,
                    Year = requestModel.Year,
                    AwayTeamWin = awayTeamWin.Score,
                    HomeTeamWin = homeTeamWin.Score
                };
                //await db.YearlyWin.AddAsync(model);
                //await db.SaveChangesAsync();
                #endregion

                return model;
            }
        }
    }
}
