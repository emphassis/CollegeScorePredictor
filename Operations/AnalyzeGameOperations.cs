using CollegeScorePredictor.Constants;
using CollegeScorePredictor.Models.Database;
using CollegeScorePredictor.Models.EspnGameSummary;
using CollegeScorePredictor.Models.Record;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json;

namespace CollegeScorePredictor.Operations
{
    public static class AnalyzeGameOperations
    {
        public static async Task<GameResultDbo> AnalyzeGame(AppDbContext db, long eventId)
        {
            try
            {
                var client = new HttpClient();
                var apiUrl = Constants.System.EspnBaseUrl + Constants.System.EspnGameSummaryApi + eventId;
                var data = await client.GetStringAsync(apiUrl);

                JsonSerializerOptions options = new() { WriteIndented = true, UnknownTypeHandling = System.Text.Json.Serialization.JsonUnknownTypeHandling.JsonElement, NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString, DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull };
                EspnGameSummaryModel? model = JsonSerializer.Deserialize<EspnGameSummaryModel>(data, options);

                var isModelValid = IsGameModelValid(model!);
                if (!isModelValid)
                {
                    //Console.WriteLine("game model not valid");
                }


                var gameModel = model!;

                Teams awayTeam = gameModel.boxscore.teams.First();
                Teams homeTeam = gameModel.boxscore.teams.Last();
                ScoringPlays finalScore = gameModel.scoringPlays.Last();

                bool homeTeamWon = finalScore.homeScore > finalScore.awayScore;

                double? homeTeamWinProbabilityBeginningOfGame = null;
                double? homeTeamWinProbabilityEndOfGame = null;
                double? awayTeamWinProbabilityBeginningOfGame = null;
                double? awayTeamWinProbabilityEndOfGame = null;

                if (gameModel.winprobability.FirstOrDefault() != null)
                {
                    homeTeamWinProbabilityBeginningOfGame = (double)gameModel.winprobability.First().homeWinPercentage!;
                    homeTeamWinProbabilityEndOfGame = (double)gameModel.winprobability.Last().homeWinPercentage!;
                    awayTeamWinProbabilityBeginningOfGame = 1 - (double)gameModel.winprobability.First().homeWinPercentage!;
                    awayTeamWinProbabilityEndOfGame = 1 - (double)gameModel.winprobability.Last().homeWinPercentage!;
                }

                string homeTeamName = string.IsNullOrWhiteSpace(homeTeam.team.displayName) ? string.Empty : homeTeam.team.displayName;
                string homeTeamChanceToWin = string.IsNullOrEmpty(gameModel.predictor.homeTeam.gameProjection) ? "50" : gameModel.predictor.homeTeam.gameProjection;
                string homeTeamFirstDowns = string.IsNullOrEmpty(homeTeam.statistics[0].displayValue) ? "15" : homeTeam.statistics[0].displayValue!;
                string homeTeamThirdDownCompletions = string.IsNullOrEmpty(homeTeam.statistics[1].displayValue) ? "0" : homeTeam.statistics[1].displayValue!;
                string homeTeamThirdDownAttempts = string.IsNullOrEmpty(homeTeam.statistics[1].displayValue) ? "0" : homeTeam.statistics[1].displayValue!;
                string homeTeamFourthDownCompletions = string.IsNullOrEmpty(homeTeam.statistics[2].displayValue) ? "0" : homeTeam.statistics[2].displayValue!;
                string homeTeamFourthDownAttempts = string.IsNullOrEmpty(homeTeam.statistics[2].displayValue) ? "0" : homeTeam.statistics[2].displayValue!;
                string homeTeamTotalYards = string.IsNullOrEmpty(homeTeam.statistics[3].displayValue) ? "200" : homeTeam.statistics[3].displayValue!;
                string homeTeamNetPassingYards = string.IsNullOrEmpty(homeTeam.statistics[4].displayValue) ? "100" : homeTeam.statistics[4].displayValue!;
                string homeTeamPassingCompletions = string.IsNullOrEmpty(homeTeam.statistics[5].displayValue) ? "10" : homeTeam.statistics[5].displayValue!;
                string homeTeamPassingAttempts = string.IsNullOrEmpty(homeTeam.statistics[5].displayValue) ? "20" : homeTeam.statistics[5].displayValue!;
                string homeTeamYardsPerPass = string.IsNullOrEmpty(homeTeam.statistics[6].displayValue) ? "4" : homeTeam.statistics[6].displayValue!;
                string homeTeamRushingYards = string.IsNullOrEmpty(homeTeam.statistics[7].displayValue) ? "100" : homeTeam.statistics[7].displayValue!;
                string homeTeamRushingAttempts = string.IsNullOrEmpty(homeTeam.statistics[8].displayValue) ? "20" : homeTeam.statistics[8].displayValue!;
                string homeTeamYardsPerRushAttempt = string.IsNullOrEmpty(homeTeam.statistics[9].displayValue) ? "4" : homeTeam.statistics[9].displayValue!;
                string homeTeamTotalPenalties = string.IsNullOrEmpty(homeTeam.statistics[10].displayValue) ? "8" : homeTeam.statistics[10].displayValue!;
                string homeTeamTotalPenaltyYards = string.IsNullOrEmpty(homeTeam.statistics[10].displayValue) ? "50" : homeTeam.statistics[10].displayValue!;
                string homeTeamFumblesLost = string.IsNullOrEmpty(homeTeam.statistics[12].displayValue) ? "1" : homeTeam.statistics[12].displayValue!;
                string homeTeamInterceptions = string.IsNullOrEmpty(homeTeam.statistics[13].displayValue) ? "1" : homeTeam.statistics[13].displayValue!;
                string homeTeamPossessionTime = string.IsNullOrEmpty(homeTeam.statistics[14].displayValue) ? "30" : homeTeam.statistics[14].displayValue!;

                string awayTeamName = string.IsNullOrWhiteSpace(awayTeam.team.displayName) ? string.Empty : awayTeam.team.displayName;
                string awayTeamChanceToWin = string.IsNullOrEmpty(gameModel.predictor.awayTeam.gameProjection) ? "50" : gameModel.predictor.awayTeam.gameProjection;
                string awayTeamFirstDowns = string.IsNullOrEmpty(awayTeam.statistics[0].displayValue) ? "15" : awayTeam.statistics[0].displayValue!;
                string awayTeamThirdDownCompletions = string.IsNullOrEmpty(awayTeam.statistics[1].displayValue) ? "0" : awayTeam.statistics[1].displayValue!;
                string awayTeamThirdDownAttempts = string.IsNullOrEmpty(awayTeam.statistics[1].displayValue) ? "0" : awayTeam.statistics[1].displayValue!;
                string awayTeamFourthDownCompletions = string.IsNullOrEmpty(awayTeam.statistics[2].displayValue) ? "0" : awayTeam.statistics[2].displayValue!;
                string awayTeamFourthDownAttempts = string.IsNullOrEmpty(awayTeam.statistics[2].displayValue) ? "0" : awayTeam.statistics[2].displayValue!;
                string awayTeamTotalYards = string.IsNullOrEmpty(awayTeam.statistics[3].displayValue) ? "200" : awayTeam.statistics[3].displayValue!;
                string awayTeamNetPassingYards = string.IsNullOrEmpty(awayTeam.statistics[4].displayValue) ? "100" : awayTeam.statistics[4].displayValue!;
                string awayTeamPassingCompletions = string.IsNullOrEmpty(awayTeam.statistics[5].displayValue) ? "10" : awayTeam.statistics[5].displayValue!;
                string awayTeamPassingAttempts = string.IsNullOrEmpty(awayTeam.statistics[5].displayValue) ? "20" : awayTeam.statistics[5].displayValue!;
                string awayTeamYardsPerPass = string.IsNullOrEmpty(awayTeam.statistics[6].displayValue) ? "4" : awayTeam.statistics[6].displayValue!;
                string awayTeamRushingYards = string.IsNullOrEmpty(awayTeam.statistics[7].displayValue) ? "100" : awayTeam.statistics[7].displayValue!;
                string awayTeamRushingAttempts = string.IsNullOrEmpty(awayTeam.statistics[8].displayValue) ? "20" : awayTeam.statistics[8].displayValue!;
                string awayTeamYardsPerRushAttempt = string.IsNullOrEmpty(awayTeam.statistics[9].displayValue) ? "4" : awayTeam.statistics[9].displayValue!;
                string awayTeamTotalPenalties = string.IsNullOrEmpty(awayTeam.statistics[10].displayValue) ? "8" : awayTeam.statistics[10].displayValue!;
                string awayTeamTotalPenaltyYards = string.IsNullOrEmpty(awayTeam.statistics[10].displayValue) ? "50" : awayTeam.statistics[10].displayValue!;
                string awayTeamFumblesLost = string.IsNullOrEmpty(awayTeam.statistics[12].displayValue) ? "1" : awayTeam.statistics[12].displayValue!;
                string awayTeamInterceptions = string.IsNullOrEmpty(awayTeam.statistics[13].displayValue) ? "1" : awayTeam.statistics[13].displayValue!;
                string awayTeamPossessionTime = string.IsNullOrEmpty(awayTeam.statistics[14].displayValue) ? "30" : awayTeam.statistics[14].displayValue!;

                var drivesModel = GetDrivesModel(gameModel.drives, homeTeamName, awayTeamName);

                var conferenceTeams = await (from c in db.ConferenceTeam
                                             select c).ToListAsync();
                var homeTeamId = TeamOperations.GetTeamId(homeTeam.team.id);
                var awayTeamId = TeamOperations.GetTeamId(awayTeam.team.id);
                var homeTeamConference = (from h in conferenceTeams
                                          where h.ConferenceTeamId == homeTeamId
                                          select h.TeamConference).FirstOrDefault();
                var awayTeamConference = (from h in conferenceTeams
                                          where h.ConferenceTeamId == awayTeamId
                                          select h.TeamConference).FirstOrDefault();

                GameResultDbo gameResult = new GameResultDbo
                {
                    EventId = eventId,
                    Week = gameModel.header.week,
                    Year = gameModel.header.season.year,
                    IsConference = gameModel.header.competitions.First().conferenceCompetition,
                    IsNeutralSite = gameModel.header.competitions.First().neutralSite,
                    HomeTeamId = homeTeamId,
                    HomeTeamConference = homeTeamConference,
                    HomeTeamName = homeTeamName,
                    HomeTeamScore = finalScore.homeScore,
                    HomeTeamChanceToWin = double.Parse(homeTeamChanceToWin),
                    HomeTeamWon = homeTeamWon,
                    HomeTeamFirstDowns = double.Parse(homeTeamFirstDowns),
                    HomeTeamThirdDownCompletions = GetSplitStatistic(homeTeamThirdDownCompletions, true),
                    HomeTeamThirdDownAttempts = GetSplitStatistic(homeTeamThirdDownAttempts, false),
                    HomeTeamFourthDownCompletions = GetSplitStatistic(homeTeamFourthDownCompletions, true),
                    HomeTeamFourthDownAttempts = GetSplitStatistic(homeTeamFourthDownAttempts, false),
                    HomeTeamTotalYards = double.Parse(homeTeamTotalYards),
                    HomeTeamNetPassingYards = double.Parse(homeTeamNetPassingYards),
                    HomeTeamPassingCompletions = GetSplitStatistic(homeTeamPassingCompletions, true),
                    HomeTeamPassingAttempts = GetSplitStatistic(homeTeamPassingAttempts, false),
                    HomeTeamYardsPerPass = double.Parse(homeTeamYardsPerPass),
                    HomeTeamRushingYards = double.Parse(homeTeamRushingYards),
                    HomeTeamRushingAttempts = double.Parse(homeTeamRushingAttempts),
                    HomeTeamYardsPerRushAttempt = double.Parse(homeTeamYardsPerRushAttempt),
                    HomeTeamTotalPenalties = GetSplitStatistic(homeTeamTotalPenalties, true),
                    HomeTeamTotalPenaltyYards = GetSplitStatistic(homeTeamTotalPenaltyYards, false),
                    HomeTeamFumblesLost = double.Parse(homeTeamFumblesLost),
                    HomeTeamInterceptions = double.Parse(homeTeamInterceptions),
                    HomeTeamPossessionTime = GetPossessionTime(homeTeamPossessionTime),
                    HomeTeamWinProbabilityBeginningOfGame = homeTeamWinProbabilityBeginningOfGame,
                    HomeTeamWinProbabilityEndOfGame = homeTeamWinProbabilityEndOfGame,
                    HomeTeamPositivePlays = drivesModel.HomeTeamPositivePlays,
                    HomeTeamNegativePlays = drivesModel.HomeTeamNegativePlays,
                    HomeTeamNeutralPlays = drivesModel.HomeTeamNeutralPlays,
                    HomeTeamDefensiveSacks = drivesModel.HomeTeamDefensiveSacks,
                    HomeTeamDefensiveSafety = drivesModel.HomeTeamDefensiveSafety,
                    HomeTeamDefensiveTacklesForLoss = drivesModel.HomeTeamDefensiveTacklesForLoss,
                    HomeTeamFieldGoals = drivesModel.HomeTeamFieldGoals,
                    HomeTeamSpecialTeamsPoints = drivesModel.HomeTeamSpecialTeamsPoints,

                    AwayTeamId = awayTeamId,
                    AwayTeamConference = awayTeamConference,
                    AwayTeamName = awayTeamName,
                    AwayTeamScore = finalScore.awayScore,
                    AwayTeamChanceToWin = double.Parse(awayTeamChanceToWin),
                    AwayTeamWon = !homeTeamWon,
                    AwayTeamFirstDowns = double.Parse(awayTeamFirstDowns),
                    AwayTeamThirdDownCompletions = GetSplitStatistic(awayTeamThirdDownCompletions, true),
                    AwayTeamThirdDownAttempts = GetSplitStatistic(awayTeamThirdDownAttempts, false),
                    AwayTeamFourthDownCompletions = GetSplitStatistic(awayTeamFourthDownCompletions, true),
                    AwayTeamFourthDownAttempts = GetSplitStatistic(awayTeamFourthDownAttempts, false),
                    AwayTeamTotalYards = double.Parse(awayTeamTotalYards),
                    AwayTeamNetPassingYards = double.Parse(awayTeamNetPassingYards),
                    AwayTeamPassingCompletions = GetSplitStatistic(awayTeamPassingCompletions, true),
                    AwayTeamPassingAttempts = GetSplitStatistic(awayTeamPassingAttempts, false),
                    AwayTeamYardsPerPass = double.Parse(awayTeamYardsPerPass),
                    AwayTeamRushingYards = double.Parse(awayTeamRushingYards),
                    AwayTeamRushingAttempts = double.Parse(awayTeamRushingAttempts),
                    AwayTeamYardsPerRushAttempt = double.Parse(awayTeamYardsPerRushAttempt),
                    AwayTeamTotalPenalties = GetSplitStatistic(awayTeamTotalPenalties, true),
                    AwayTeamTotalPenaltyYards = GetSplitStatistic(awayTeamTotalPenaltyYards, false),
                    AwayTeamFumblesLost = double.Parse(awayTeamFumblesLost),
                    AwayTeamInterceptions = double.Parse(awayTeamInterceptions),
                    AwayTeamPossessionTime = GetPossessionTime(awayTeamPossessionTime),
                    AwayTeamWinProbabilityBeginningOfGame = awayTeamWinProbabilityBeginningOfGame,
                    AwayTeamWinProbabilityEndOfGame = awayTeamWinProbabilityEndOfGame,
                    AwayTeamPositivePlays = drivesModel.AwayTeamPositivePlays,
                    AwayTeamNegativePlays = drivesModel.AwayTeamNegativePlays,
                    AwayTeamNeutralPlays = drivesModel.AwayTeamNeutralPlays,
                    AwayTeamDefensiveSacks = drivesModel.AwayTeamDefensiveSacks,
                    AwayTeamDefensiveSafety = drivesModel.AwayTeamDefensiveSafety,
                    AwayTeamDefensiveTacklesForLoss = drivesModel.AwayTeamDefensiveTacklesForLoss,
                    AwayTeamFieldGoals = drivesModel.AwayTeamFieldGoals,
                    AwayTeamSpecialTeamsPoints = drivesModel.AwayTeamSpecialTeamsPoints,
                };

                return gameResult;
            }
            catch (Exception exception)
            {
                Debug.WriteLine("skipped event: " + eventId);
                Debug.WriteLine(exception.Message);
                Debug.WriteLine(exception.StackTrace);
                return null!;
                //for whatever reason, the null checks aren't working and boxscore is ALWAYS available when I know for a fucking fact it isn't available on https://site.api.espn.com/apis/site/v2/sports/football/college-football/summary?event=222340258
            }
        }

        private static GetGameDrivesModel GetDrivesModel(Drives drives, string homeTeamName, string awayTeamName)
        {
            GetGameDrivesModel model = new GetGameDrivesModel();
            foreach (var drive in drives.previous)
            {
                foreach (var play in drive.plays)
                {
                    try
                    {
                        Enums.PlayType playType = (Enums.PlayType)int.Parse(play.type.id!);

                        var isHomeTeamOnOffense = homeTeamName == drive.team.displayName;

                        //positive offensive play
                        if (playType == Enums.PlayType.TwoPointPass ||
                                playType == Enums.PlayType.TwoPointRush ||
                                playType == Enums.PlayType.Kickoff ||
                                playType == Enums.PlayType.FieldGoalGood ||
                                playType == Enums.PlayType.PassingTouchdown ||
                                playType == Enums.PlayType.RushingTouchdown ||
                                playType == Enums.PlayType.PassReception ||
                                playType == Enums.PlayType.Rush)
                        {
                            if (isHomeTeamOnOffense)
                            {
                                if (playType == Enums.PlayType.Rush ||
                                    playType == Enums.PlayType.RushingTouchdown ||
                                        playType == Enums.PlayType.TwoPointRush)
                                {
                                    model.HomeTeamRushingPlays += 1;
                                    if (play.statYardage < 0)
                                    {
                                        model.AwayTeamDefensiveTacklesForLoss += 1;
                                        model.AwayTeamPositivePlays++;
                                        model.HomeTeamNegativePlays++;
                                    }
                                    else
                                    {
                                        model.HomeTeamPositivePlays++;
                                        model.AwayTeamNegativePlays++;
                                    }
                                }
                                else if (playType == Enums.PlayType.PassingTouchdown ||
                                    playType == Enums.PlayType.PassIncompletion ||
                                    playType == Enums.PlayType.PassReception)
                                {
                                    model.HomeTeamPassingPlays += 1;
                                }
                                else
                                {
                                    if (playType == Enums.PlayType.FieldGoalGood)
                                    {
                                        model.HomeTeamFieldGoals += 1;
                                        model.HomeTeamSpecialTeamsPoints += 3;
                                    }
                                    if (playType == Enums.PlayType.TwoPointPass ||
                                        playType == Enums.PlayType.TwoPointRush)
                                    {
                                        model.HomeTeamSpecialTeamsPoints += 2;
                                    }
                                    model.HomeTeamPositivePlays++;
                                    model.AwayTeamNegativePlays++;
                                }
                            }
                            else
                            {
                                if (playType == Enums.PlayType.Rush ||
                                    playType == Enums.PlayType.RushingTouchdown ||
                                        playType == Enums.PlayType.TwoPointRush)
                                {
                                    model.AwayTeamRushingPlays += 1;
                                    if (play.statYardage < 0)
                                    {
                                        model.HomeTeamDefensiveTacklesForLoss += 1;
                                        model.HomeTeamPositivePlays++;
                                        model.AwayTeamNegativePlays++;
                                    }
                                    else
                                    {
                                        model.AwayTeamPositivePlays++;
                                        model.HomeTeamNegativePlays++;
                                    }
                                }
                                else if (playType == Enums.PlayType.PassingTouchdown ||
                                    playType == Enums.PlayType.PassIncompletion ||
                                    playType == Enums.PlayType.PassReception)
                                {
                                    model.AwayTeamPassingPlays += 1;
                                }
                                else
                                {
                                    if (playType == Enums.PlayType.FieldGoalGood)
                                    {
                                        model.AwayTeamFieldGoals += 1;
                                        model.AwayTeamSpecialTeamsPoints += 3;
                                    }
                                    if (playType == Enums.PlayType.TwoPointPass ||
                                        playType == Enums.PlayType.TwoPointRush)
                                    {
                                        model.AwayTeamSpecialTeamsPoints += 2;
                                    }
                                    model.AwayTeamPositivePlays++;
                                    model.HomeTeamNegativePlays++;
                                }
                            }
                        }

                        //negative offensive play
                        else if (playType == Enums.PlayType.PassIncompletion ||
                                playType == Enums.PlayType.Sack ||
                                playType == Enums.PlayType.BlockedPunt ||
                                playType == Enums.PlayType.Punt ||
                                playType == Enums.PlayType.Safety ||
                                playType == Enums.PlayType.PassInterceptionReturn ||
                                playType == Enums.PlayType.FumbleRecoveryOpponent ||
                                playType == Enums.PlayType.PuntReturnTouchdown ||
                                playType == Enums.PlayType.InterceptionReturnTouchdown ||
                                playType == Enums.PlayType.BlockedPuntTouchdown ||
                                playType == Enums.PlayType.Defensive2PtConversion ||
                                playType == Enums.PlayType.FumbleReturnTouchdown ||
                                playType == Enums.PlayType.FieldGoalMissed ||
                                playType == Enums.PlayType.Interception ||
                                playType == Enums.PlayType.FieldGoalReturnTouchdown ||
                                playType == Enums.PlayType.FieldGoalReturn ||
                                playType == Enums.PlayType.BlockedFieldGoalTouchdown ||
                                playType == Enums.PlayType.KickoffReturnTouchdown ||//I think this is from the pov of offense
                                playType == Enums.PlayType.BlockedFieldGoal ||
                                playType == Enums.PlayType.FumbleRecoveryOwn)
                        {
                            if (isHomeTeamOnOffense)
                            {
                                if (playType == Enums.PlayType.FieldGoalReturnTouchdown ||
                                    playType == Enums.PlayType.BlockedFieldGoalTouchdown ||
                                    playType == Enums.PlayType.KickoffReturnTouchdown ||
                                    playType == Enums.PlayType.BlockedPuntTouchdown ||
                                    playType == Enums.PlayType.PuntReturnTouchdown ||
                                    playType == Enums.PlayType.Defensive2PtConversion)
                                {
                                    if (playType == Enums.PlayType.FieldGoalReturnTouchdown ||
                                    playType == Enums.PlayType.BlockedFieldGoalTouchdown ||
                                    playType == Enums.PlayType.KickoffReturnTouchdown ||
                                    playType == Enums.PlayType.BlockedPuntTouchdown)
                                    {
                                        model.AwayTeamSpecialTeamsPoints += 7;
                                    }
                                    if (playType == Enums.PlayType.Defensive2PtConversion)
                                    {
                                        model.AwayTeamSpecialTeamsPoints += 2;
                                    }
                                    if (playType == Enums.PlayType.Safety)
                                    {
                                        model.AwayTeamSpecialTeamsPoints += 2;
                                        model.AwayTeamDefensiveSafety += 1;
                                    }
                                }
                                if (playType == Enums.PlayType.Sack)
                                {
                                    model.AwayTeamDefensiveSacks += 1;
                                }
                                model.HomeTeamNegativePlays++;
                                model.AwayTeamPositivePlays++;
                            }
                            else
                            {
                                if (playType == Enums.PlayType.FieldGoalReturnTouchdown ||
                                    playType == Enums.PlayType.BlockedFieldGoalTouchdown ||
                                    playType == Enums.PlayType.KickoffReturnTouchdown ||
                                    playType == Enums.PlayType.BlockedPuntTouchdown ||
                                    playType == Enums.PlayType.PuntReturnTouchdown ||
                                    playType == Enums.PlayType.Defensive2PtConversion)
                                {
                                    if (playType == Enums.PlayType.FieldGoalReturnTouchdown ||
                                    playType == Enums.PlayType.BlockedFieldGoalTouchdown ||
                                    playType == Enums.PlayType.KickoffReturnTouchdown ||
                                    playType == Enums.PlayType.BlockedPuntTouchdown)
                                    {
                                        model.HomeTeamSpecialTeamsPoints += 7;
                                    }
                                    if (playType == Enums.PlayType.Defensive2PtConversion)
                                    {
                                        model.HomeTeamSpecialTeamsPoints += 2;
                                    }
                                    if (playType == Enums.PlayType.Safety)
                                    {
                                        model.HomeTeamSpecialTeamsPoints += 2;
                                        model.HomeTeamDefensiveSafety += 1;
                                    }
                                }
                                if (playType == Enums.PlayType.Sack)
                                {
                                    model.HomeTeamDefensiveSacks += 1;
                                }
                                model.HomeTeamPositivePlays++;
                                model.AwayTeamNegativePlays++;
                            }
                        }
                        else if (playType == Enums.PlayType.EndofGame ||
                                playType == Enums.PlayType.EndofHalf ||
                                playType == Enums.PlayType.EndOfRegulation ||
                                playType == Enums.PlayType.EndPeriod ||
                                playType == Enums.PlayType.KickoffReturn ||
                                playType == Enums.PlayType.CoinToss ||
                                playType == Enums.PlayType.Timeout ||
                                playType == Enums.PlayType.Penalty)
                        //I need to parse string to figure out who made the penalty
                        {
                            model.HomeTeamNeutralPlays++;
                            model.AwayTeamNeutralPlays++;
                        }

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("drive missing?");
                        Debug.WriteLine("playtype: " + play.type.id);
                        //Console.WriteLine("Error getting drives from " + awayTeamName + " at " + homeTeamName);
                        //blech
                    }
                }
            }
            return model;
        }

        private static double GetStatisticPercentage(string stat)
        {
            var numbers = stat.Split("-");

            var successes = int.Parse(numbers[0]);
            var attempts = int.Parse(numbers[1]);

            if (attempts > 0 && successes > 0)
            {
                return successes / attempts;
            }

            return 100;
        }

        private static double GetSplitStatistic(string stat, bool firstIndex)
        {
            var numbers = stat.Split("-");
            if (firstIndex)
            {
                return double.Parse(numbers[0]);
            }
            else
            {
                return double.Parse(numbers[1]);
            }
        }

        private static double GetPossessionTime(string stat)
        {
            var time = stat.Split(":");
            var minutes = Int32.Parse(time[0]);
            var seconds = Int32.Parse(time[1]);
            var secondPercentage = seconds == 0 ? 0 : seconds / 60;

            return Math.Round((double)minutes + secondPercentage, 2);
        }

        private static bool IsGameModelValid(EspnGameSummaryModel model)
        {
            if (model == null) return false;
            if (model.header.competitions.FirstOrDefault() == null) return false;
            if (model.header.competitions.First().boxscoreAvailable == false) return false;
            if (model.boxscore.teams.FirstOrDefault() == null) return false;
            if (model.scoringPlays.FirstOrDefault() == null) return false;
            if (model.boxscore.teams.First().statistics.Count == 0) return false;
            return true;
        }
    }
}
