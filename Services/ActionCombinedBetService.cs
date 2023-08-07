using CollegeScorePredictor.Models.Bet;
using CollegeScorePredictor.Models.Database;
using CollegeScorePredictor.Operations;
using Microsoft.EntityFrameworkCore;

namespace CollegeScorePredictor.Services
{
    public class ActionCombinedBetService
    {
        private IDbContextFactory<AppDbContext> factory;
        public ActionCombinedBetService(IDbContextFactory<AppDbContext> _factory)
        {
            factory = _factory;
        }

        public async Task CreateBets(int week, int year)
        {
            var odds = await ActionNetworkApiOperations.GetOdds(week, year);

            if (odds == null)
            {
                Console.WriteLine("no odds");
                return;
            }

            using (var db = await factory.CreateDbContextAsync())
            {
                var conferenceTeams = await (from t in db.ConferenceTeam
                                             select t).ToListAsync();

                var teamGames = await (from g in db.GamePrediction
                                       where g.Week == week && g.Year == year
                                       select g).ToListAsync();

                foreach (var odd in odds.games)
                {

                    var caesars = odd.odds.FirstOrDefault();

                    if (caesars == null)
                    {
                        Console.WriteLine("no caesars");
                        continue;
                    }

                    var homeTeamId = conferenceTeams.Where(t => t.ActionNetworkId == odd.home_team_id).FirstOrDefault();
                    var awayTeamId = conferenceTeams.Where(t => t.ActionNetworkId == odd.away_team_id).FirstOrDefault();

                    if (homeTeamId == null || awayTeamId == null)
                    {
                        Console.WriteLine("no ids");
                        continue;
                    }

                    var gamePrediction = (from t in teamGames
                                          where t.HomeTeamId == homeTeamId.TeamId
                                          && t.AwayTeamId == awayTeamId.TeamId
                                          select t).FirstOrDefault();

                    var gamePredictionConferenceId = (from t in db.GamePredictionConference
                                          where t.HomeTeamId == homeTeamId.TeamId
                                          && t.AwayTeamId == awayTeamId.TeamId
                                          && t.Week == week && t.Year == year
                                          select t).FirstOrDefault();

                    if (gamePrediction == null || gamePredictionConferenceId == null)
                    {
                        Console.WriteLine("no predictions");
                        continue;
                    }

                    var gameScores = await (from h in db.ScorePrediction
                                            where h.GamePredictionId == gamePrediction.GamePredictionId
                                            select h).ToListAsync();

                    var gameConferenceScores = await (from h in db.ScorePredictionConference
                                                      where h.GamePredictionConferenceId == gamePredictionConferenceId.GamePredictionConferenceId
                                                      select new ScorePredictionDbo
                                                      {
                                                          ScorePredictionId = h.ScorePredictionConferenceId,
                                                          GamePredictionId = h.GamePredictionConferenceId,
                                                          ModelName = h.ModelName,
                                                          HomeTeamScore = h.HomeTeamScore,
                                                          AwayTeamScore = h.AwayTeamScore
                                                      }).ToListAsync();

                    gameScores.AddRange(gameConferenceScores);

                    if (gameScores.Count < 3)
                    {
                        Console.WriteLine("no scores");
                        continue;
                    }

                    if(gameScores.Count < 6)
                    {
                        Console.WriteLine("There are only " + gameScores.Count + " game scores.");
                    }

                    var homeSpread = caesars.spread_home;
                    var awaySpread = caesars.spread_away;
                    //var homeMoneyLine = caesars.ml_home;
                    //var awayMoneyLine = caesars.ml_away;
                    var overUnder = caesars.total;

                    #region Public Bets
                    var publicBetOverCount = caesars.total_over_public.HasValue ? caesars.total_over_public.Value : 0;
                    var publicBetUnderCount = caesars.total_under_public.HasValue ? caesars.total_under_public.Value : 0;

                    var publicBetOver = publicBetOverCount > publicBetUnderCount;
                    var publicBetUnder = publicBetUnderCount > publicBetOverCount;

                    var publicMoneylineHomeCount = caesars.ml_home_public.HasValue ? caesars.ml_home_public.Value : 0;
                    var publicMoneylineAwayCount = caesars.ml_away_public.HasValue ? caesars.ml_away_public.Value : 0;

                    var publicBetMoneylineHome = publicMoneylineHomeCount > publicMoneylineAwayCount;
                    var publicBetMoneylineAway = publicMoneylineAwayCount > publicMoneylineHomeCount;

                    var publicSpreadHomeCount = caesars.spread_home_public.HasValue ? caesars.spread_home_public.Value : 0;
                    var publicSpreadAwayCount = caesars.spread_away_public.HasValue ? caesars.spread_away_public.Value : 0;

                    var publicBetSpreadHome = publicSpreadHomeCount > publicSpreadAwayCount;
                    var publicBetSpreadAway = publicSpreadAwayCount > publicSpreadHomeCount;
                    #endregion

                    var noSpread = (homeSpread == null || awaySpread == null);

                    if (homeSpread == null || awaySpread == null)
                    {
                        homeSpread = 0;
                        awaySpread = 0;
                        Console.WriteLine("no spread");
                    }

                    var noOverUnder = overUnder == null;

                    if (overUnder == null)
                    {
                        overUnder = 0;
                        Console.WriteLine("no over/under");
                    }

                    overUnder = Math.Abs((double)overUnder);

                    var homeFavorite = awaySpread > homeSpread;

                    var betGame = false;

                    var homeTeamGameScores = (from g in gameScores
                                              select g.HomeTeamScore).OrderByDescending(x => x).ToList();

                    var awayTeamGameScores = (from g in gameScores
                                              select g.AwayTeamScore).OrderByDescending(x => x).ToList();

                    var gameOne = gameScores[0];
                    var gameTwo = gameScores[1];
                    var gameThree = gameScores[2];
                    double homeGameOneWinMargin = gameOne.HomeTeamScore - gameOne.AwayTeamScore;
                    double homeGameTwoWinMargin = gameTwo.HomeTeamScore - gameTwo.AwayTeamScore;
                    double homeGameThreeWinMargin = gameThree.HomeTeamScore - gameThree.AwayTeamScore;
                    double awayGameOneWinMargin = gameOne.AwayTeamScore - gameOne.HomeTeamScore;
                    double awayGameTwoWinMargin = gameTwo.AwayTeamScore - gameTwo.HomeTeamScore;
                    double awayGameThreeWinMargin = gameThree.AwayTeamScore - gameThree.HomeTeamScore;
                    double gameOneOverUnder = gameOne.HomeTeamScore + gameOne.AwayTeamScore;
                    double gameTwoOverUnder = gameTwo.HomeTeamScore + gameTwo.AwayTeamScore;
                    double gameThreeOverUnder = gameThree.HomeTeamScore + gameThree.AwayTeamScore;
                    bool betHomeSpread = false;
                    bool betAwaySpread = false;
                    bool betHomeMoneyline = false;
                    bool betAwayMoneyline = false;
                    bool betOver = false;
                    bool betUnder = false;
                    bool rule4or5 = false;
                    int variance = 100;
                    var homeTeamFavorite = homeSpread < awaySpread;

                    var posHomeSpread = Math.Abs((double)homeSpread);
                    var posAwaySpread = Math.Abs((double)awaySpread);

                    if (!noOverUnder)
                    {
                        if (overUnder > 0 &&
                            gameOneOverUnder > overUnder + 3 &&
                            gameTwoOverUnder > overUnder + 3 &&
                            gameThreeOverUnder > overUnder + 3)
                        {
                            betOver = true;
                        }
                        if (overUnder > 0 &&
                            gameOneOverUnder + 3 < overUnder &&
                            gameTwoOverUnder + 3 < overUnder &&
                            gameThreeOverUnder + 3 < overUnder)
                        {
                            betUnder = true;
                        }
                    }

                    if (!noSpread)
                    {
                        #region Rule 0
                        /*
                         rule 0: if a model variance, i.e., the point difference between the home team's highest score and lowest score
                                    plus the point difference between the away team's highest score and lowest score
                                    is more than 12, don't bet
                         */
                        var homeTeamVariance = homeTeamGameScores.First() - homeTeamGameScores.Last();
                        var awayTeamVariance = awayTeamGameScores.First() - awayTeamGameScores.Last();

                        variance = homeTeamVariance + awayTeamVariance;

                        if (variance <= 8)
                        {
                            betGame = true;
                        }

                        if (homeTeamVariance > 8)
                        {
                            betGame = false;
                        }

                        if (awayTeamVariance > 8)
                        {
                            betGame = false;
                        }

                        #endregion
                        #region Rule 1
                        //rule 1: a favorite must cover the spread +3 in order to be an eligible bet
                        if (homeTeamFavorite)
                        {
                            if (homeGameOneWinMargin > (awaySpread + 3) &&
                                homeGameTwoWinMargin > (awaySpread + 3) &&
                                homeGameThreeWinMargin > (awaySpread + 3))
                            {
                                betHomeSpread = true;
                            }
                        }
                        else
                        {
                            if (awayGameOneWinMargin > (homeSpread + 3) &&
                                awayGameTwoWinMargin > (homeSpread + 3) &&
                                awayGameThreeWinMargin > (homeSpread + 3))
                            {
                                betAwaySpread = true;
                            }
                        }
                        #endregion
                        #region Rule 2
                        //rule 2: an underdog will only be bet on the point spread if they're picked to beat the favorite
                        if (homeTeamFavorite)
                        {
                            if (awayGameOneWinMargin > 4 &&
                                awayGameTwoWinMargin > 4 &&
                                awayGameThreeWinMargin > 4)
                            {
                                betAwayMoneyline = true;
                            }
                        }
                        else
                        {
                            if (homeGameOneWinMargin > 4 &&
                                homeGameTwoWinMargin > 4 &&
                                homeGameThreeWinMargin > 4)
                            {
                                betHomeMoneyline = true;
                            }
                        }
                        #endregion
                        #region Rule 3
                        //rule 3: if the spread of a game +3 is half the under/over or more, don't bet the under
                        if (posHomeSpread > ((overUnder * .5) + 3))
                        {
                            betGame = false;
                        }
                        #endregion
                        #region Rule 4
                        //rule 4: if the spread is more than -15 don't bet the favorite
                        if (homeFavorite)
                        {
                            if (posHomeSpread > 15)
                            {
                                betGame = false;
                                rule4or5 = true;
                            }
                        }
                        else
                        {
                            if (posAwaySpread > 15)
                            {
                                betGame = false;
                                rule4or5 = true;
                            }
                        }
                        #endregion
                        #region Rule 5
                        //rule 5: if the highest model of the underdog margin of victory plus point spread is greater than 15 don't bet
                        if (betHomeMoneyline)
                        {
                            var homeMargins = new List<double>
                            {
                                homeGameOneWinMargin,
                                homeGameTwoWinMargin,
                                homeGameThreeWinMargin
                            };
                            homeMargins = homeMargins.OrderBy(x => x).ToList();
                            if (homeMargins.Last() + posHomeSpread > 15)
                            {
                                betGame = false;
                                rule4or5 = true;
                            }
                        }
                        if (betAwayMoneyline)
                        {
                            var awayMargins = new List<double>
                            {
                                awayGameOneWinMargin,
                                awayGameTwoWinMargin,
                                awayGameThreeWinMargin
                            };
                            awayMargins = awayMargins.OrderBy(x => x).ToList();
                            if (awayMargins.First() + posAwaySpread > 15)
                            {
                                betGame = false;
                                rule4or5 = true;
                            }
                        }
                        #endregion
                        #region Rule 6
                        //rule 6: don't bet overs or unders on games affected by rule 4 or 5
                        if (rule4or5)
                        {
                            betGame = false;
                            betGame = false;
                        }
                        #endregion

                        //#region Rule 7
                        ///*
                        //    rule 7: if a team loses a specific bet, e.g., covering the point spread, then for that (year?) you can't
                        //            pick the team to cover the point spread. but you could bet the team on an over or under
                        // */

                        //if (betHomeSpread)
                        //{
                        //    var didLoseBeforeAway = await (from bh in db.BetHistory
                        //                                   join b in db.Bet on bh.BetId equals b.BetId
                        //                                   where bh.Year == year && bh.Week > 2 &&
                        //                                   bh.Won == false
                        //                                   && b.BetType == (int)BetTypes.HomeSpread
                        //                                   && b.AwayTeamId == awayTeamId.TeamId
                        //                                   select bh).AnyAsync();

                        //    var didLoseBeforeHome = await (from bh in db.BetHistory
                        //                                   join b in db.Bet on bh.BetId equals b.BetId
                        //                                   where bh.Year == year && bh.Week > 2 &&
                        //                                   bh.Won == false
                        //                                   && b.BetType == (int)BetTypes.HomeSpread
                        //                                   && b.HomeTeamId == awayTeamId.TeamId
                        //                                   select bh).AnyAsync();

                        //    if (didLoseBeforeHome || didLoseBeforeAway)
                        //    {
                        //        betGame = false;
                        //    }
                        //}
                        //if (betAwaySpread)
                        //{
                        //    var didLoseBeforeAway = await (from bh in db.BetHistory
                        //                                   join b in db.Bet on bh.BetId equals b.BetId
                        //                                   where bh.Year == year && bh.Week > 2 &&
                        //                                   bh.Won == false
                        //                                   && b.BetType == (int)BetTypes.AwaySpread
                        //                                   && b.AwayTeamId == homeTeamId.TeamId
                        //                                   select bh).AnyAsync();

                        //    var didLoseBeforeHome = await (from bh in db.BetHistory
                        //                                   join b in db.Bet on bh.BetId equals b.BetId
                        //                                   where bh.Year == year && bh.Week > 2 &&
                        //                                   bh.Won == false
                        //                                   && b.BetType == (int)BetTypes.AwaySpread
                        //                                   && b.HomeTeamId == homeTeamId.TeamId
                        //                                   select bh).AnyAsync();

                        //    if (didLoseBeforeHome || didLoseBeforeAway)
                        //    {
                        //        betGame = false;
                        //    }
                        //}
                        //if (betHomeMoneyline)
                        //{
                        //    var didLoseBeforeAway = await (from bh in db.BetHistory
                        //                                   join b in db.Bet on bh.BetId equals b.BetId
                        //                                   where bh.Year == year && bh.Week > 2 &&
                        //                                   bh.Won == false
                        //                                   && b.BetType == (int)BetTypes.HomeMoneyLine
                        //                                   && b.AwayTeamId == awayTeamId.TeamId
                        //                                   select bh).AnyAsync();

                        //    var didLoseBeforeHome = await (from bh in db.BetHistory
                        //                                   join b in db.Bet on bh.BetId equals b.BetId
                        //                                   where bh.Year == year && bh.Week > 2 &&
                        //                                   bh.Won == false
                        //                                   && b.BetType == (int)BetTypes.HomeMoneyLine
                        //                                   && b.HomeTeamId == awayTeamId.TeamId
                        //                                   select bh).AnyAsync();

                        //    if (didLoseBeforeHome || didLoseBeforeAway)
                        //    {
                        //        betGame = false;
                        //    }
                        //}
                        //if (betAwayMoneyline)
                        //{
                        //    var didLoseBeforeAway = await (from bh in db.BetHistory
                        //                                   join b in db.Bet on bh.BetId equals b.BetId
                        //                                   where bh.Year == year && bh.Week > 2 &&
                        //                                   bh.Won == false
                        //                                   && b.BetType == (int)BetTypes.AwayMoneyLine
                        //                                   && b.AwayTeamId == homeTeamId.TeamId
                        //                                   select bh).AnyAsync();

                        //    var didLoseBeforeHome = await (from bh in db.BetHistory
                        //                                   join b in db.Bet on bh.BetId equals b.BetId
                        //                                   where bh.Year == year && bh.Week > 2 &&
                        //                                   bh.Won == false
                        //                                   && b.BetType == (int)BetTypes.AwayMoneyLine
                        //                                   && b.HomeTeamId == homeTeamId.TeamId
                        //                                   select bh).AnyAsync();

                        //    if (didLoseBeforeHome || didLoseBeforeAway)
                        //    {
                        //        betGame = false;
                        //    }
                        //}
                        //if (betOver)
                        //{
                        //    var didLoseBefore = await (from bh in db.BetHistory
                        //                               join b in db.Bet on bh.BetId equals b.BetId
                        //                               where bh.Year == year && bh.Week > 2 &&
                        //                               bh.Won == false
                        //                               && b.BetType == (int)BetTypes.Over
                        //                               && (b.AwayTeamId == homeTeamId.TeamId ||
                        //                               b.HomeTeamId == homeTeamId.TeamId ||
                        //                               b.AwayTeamId == awayTeamId.TeamId ||
                        //                               b.HomeTeamId == awayTeamId.TeamId)
                        //                               select bh).AnyAsync();

                        //    if (didLoseBefore)
                        //    {
                        //        betGame = false;
                        //    }
                        //}
                        //if (betUnder)
                        //{
                        //    var didLoseBefore = await (from bh in db.BetHistory
                        //                               join b in db.Bet on bh.BetId equals b.BetId
                        //                               where bh.Year == year && bh.Week > 2 &&
                        //                               bh.Won == false
                        //                               && b.BetType == (int)BetTypes.Under
                        //                               && (b.AwayTeamId == homeTeamId.TeamId ||
                        //                               b.HomeTeamId == homeTeamId.TeamId ||
                        //                               b.AwayTeamId == awayTeamId.TeamId ||
                        //                               b.HomeTeamId == awayTeamId.TeamId)
                        //                               select bh).AnyAsync();

                        //    if (didLoseBefore)
                        //    {
                        //        betGame = false;
                        //    }
                        //}

                        //#endregion

                        #region Rule 7
                        ///*
                        //    rule 7: if a team loses a specific bet, e.g., covering the point spread, then for that (year?) you can't
                        //            pick the team to cover the point spread. but you could bet the team on an over or under
                        // */
                        var weekBegin = week - 3;

                        if (betHomeSpread)
                        {
                            var didLoseBeforeAway = await (from bh in db.CombinedBetHistory
                                                           join b in db.CombinedBet on bh.CombinedBetId equals b.CombinedBetId
                                                           where bh.Year == year && bh.Week > weekBegin &&
                                                           bh.Won == false
                                                           && b.BetType == (int)BetTypes.HomeSpread
                                                           && b.AwayTeamId == awayTeamId.TeamId
                                                           select bh).AnyAsync();

                            var didLoseBeforeHome = await (from bh in db.CombinedBetHistory
                                                           join b in db.CombinedBet on bh.CombinedBetId equals b.CombinedBetId
                                                           where bh.Year == year && bh.Week > weekBegin &&
                                                           bh.Won == false
                                                           && b.BetType == (int)BetTypes.HomeSpread
                                                           && b.HomeTeamId == awayTeamId.TeamId
                                                           select bh).AnyAsync();

                            if (didLoseBeforeHome || didLoseBeforeAway)
                            {
                                betGame = false;
                            }
                        }
                        if (betAwaySpread)
                        {
                            var didLoseBeforeAway = await (from bh in db.CombinedBetHistory
                                                           join b in db.CombinedBet on bh.CombinedBetId equals b.CombinedBetId
                                                           where bh.Year == year && bh.Week > weekBegin &&
                                                           bh.Won == false
                                                           && b.BetType == (int)BetTypes.AwaySpread
                                                           && b.AwayTeamId == homeTeamId.TeamId
                                                           select bh).AnyAsync();

                            var didLoseBeforeHome = await (from bh in db.CombinedBetHistory
                                                           join b in db.CombinedBet on bh.CombinedBetId equals b.CombinedBetId
                                                           where bh.Year == year && bh.Week > weekBegin &&
                                                           bh.Won == false
                                                           && b.BetType == (int)BetTypes.AwaySpread
                                                           && b.HomeTeamId == homeTeamId.TeamId
                                                           select bh).AnyAsync();

                            if (didLoseBeforeHome || didLoseBeforeAway)
                            {
                                betGame = false;
                            }
                        }
                        if (betHomeMoneyline)
                        {
                            var didLoseBeforeAway = await (from bh in db.CombinedBetHistory
                                                           join b in db.CombinedBet on bh.CombinedBetId equals b.CombinedBetId
                                                           where bh.Year == year && bh.Week > weekBegin &&
                                                           bh.Won == false
                                                           && b.BetType == (int)BetTypes.HomeMoneyLine
                                                           && b.AwayTeamId == awayTeamId.TeamId
                                                           select bh).AnyAsync();

                            var didLoseBeforeHome = await (from bh in db.CombinedBetHistory
                                                           join b in db.CombinedBet on bh.CombinedBetId equals b.CombinedBetId
                                                           where bh.Year == year && bh.Week > weekBegin &&
                                                           bh.Won == false
                                                           && b.BetType == (int)BetTypes.HomeMoneyLine
                                                           && b.HomeTeamId == awayTeamId.TeamId
                                                           select bh).AnyAsync();

                            if (didLoseBeforeHome || didLoseBeforeAway)
                            {
                                betGame = false;
                            }
                        }
                        if (betAwayMoneyline)
                        {
                            var didLoseBeforeAway = await (from bh in db.CombinedBetHistory
                                                           join b in db.CombinedBet on bh.CombinedBetId equals b.CombinedBetId
                                                           where bh.Year == year && bh.Week > weekBegin &&
                                                           bh.Won == false
                                                           && b.BetType == (int)BetTypes.AwayMoneyLine
                                                           && b.AwayTeamId == homeTeamId.TeamId
                                                           select bh).AnyAsync();

                            var didLoseBeforeHome = await (from bh in db.CombinedBetHistory
                                                           join b in db.CombinedBet on bh.CombinedBetId equals b.CombinedBetId
                                                           where bh.Year == year && bh.Week > weekBegin &&
                                                           bh.Won == false
                                                           && b.BetType == (int)BetTypes.AwayMoneyLine
                                                           && b.HomeTeamId == homeTeamId.TeamId
                                                           select bh).AnyAsync();

                            if (didLoseBeforeHome || didLoseBeforeAway)
                            {
                                betGame = false;
                            }
                        }
                        if (betOver)
                        {
                            var didLoseBefore = await (from bh in db.CombinedBetHistory
                                                       join b in db.CombinedBet on bh.CombinedBetId equals b.CombinedBetId
                                                       where bh.Year == year && bh.Week > weekBegin &&
                                                       bh.Won == false
                                                       && b.BetType == (int)BetTypes.Over
                                                       && (b.AwayTeamId == homeTeamId.TeamId ||
                                                       b.HomeTeamId == homeTeamId.TeamId ||
                                                       b.AwayTeamId == awayTeamId.TeamId ||
                                                       b.HomeTeamId == awayTeamId.TeamId)
                                                       select bh).AnyAsync();

                            if (didLoseBefore)
                            {
                                betGame = false;
                            }
                        }
                        if (betUnder)
                        {
                            var didLoseBefore = await (from bh in db.CombinedBetHistory
                                                       join b in db.CombinedBet on bh.CombinedBetId equals b.CombinedBetId
                                                       where bh.Year == year && bh.Week > weekBegin &&
                                                       bh.Won == false
                                                       && b.BetType == (int)BetTypes.Under
                                                       && (b.AwayTeamId == homeTeamId.TeamId ||
                                                       b.HomeTeamId == homeTeamId.TeamId ||
                                                       b.AwayTeamId == awayTeamId.TeamId ||
                                                       b.HomeTeamId == awayTeamId.TeamId)
                                                       select bh).AnyAsync();

                            if (didLoseBefore)
                            {
                                betGame = false;
                            }
                        }

                        #endregion

                    }

                    if (betHomeSpread)
                    {
                        //specific to this model
                        //amazing odds 15-4
                        if (variance > 9)
                        {
                            betGame = false;
                        }

                        //betGame = GetBetBool(betGame, BetTypes.HomeSpread, publicBetMoneylineAway, publicBetMoneylineHome, publicBetSpreadAway, publicBetSpreadHome, publicBetOver, publicBetUnder);

                        var bet = new CombinedBetDbo
                        {
                            GamePredictionId = gamePrediction.GamePredictionId,
                            HomeTeamId = homeTeamId.TeamId,
                            AwayTeamId = awayTeamId.TeamId,
                            Bet = betGame,
                            BetType = (int)BetTypes.HomeSpread,
                            Odd = homeSpread.Value,
                            PublicBetOver = publicBetOver,
                            PublicBetUnder = publicBetUnder,
                            PublicBetHomeMoneyline = publicBetMoneylineHome,
                            PublicBetAwayMoneyline = publicBetMoneylineAway,
                            PublicBetHomeSpread = publicBetSpreadHome,
                            PublicBetAwaySpread = publicBetSpreadAway,
                            Variance = variance,
                            Week = week,
                            Year = year

                        };
                        await db.CombinedBet.AddAsync(bet);
                    }
                    if (betAwaySpread)
                    {
                        //specific to this model
                        //odds 7-2
                        if (variance > 4)
                        {
                            betGame = false;
                        }

                        //betGame = GetBetBool(betGame, BetTypes.AwaySpread, publicBetMoneylineAway, publicBetMoneylineHome, publicBetSpreadAway, publicBetSpreadHome, publicBetOver, publicBetUnder);

                        var bet = new CombinedBetDbo
                        {
                            GamePredictionId = gamePrediction.GamePredictionId,
                            HomeTeamId = homeTeamId.TeamId,
                            AwayTeamId = awayTeamId.TeamId,
                            Bet = betGame,
                            BetType = (int)BetTypes.AwaySpread,
                            Odd = awaySpread.Value,
                            PublicBetOver = publicBetOver,
                            PublicBetUnder = publicBetUnder,
                            PublicBetHomeMoneyline = publicBetMoneylineHome,
                            PublicBetAwayMoneyline = publicBetMoneylineAway,
                            PublicBetHomeSpread = publicBetSpreadHome,
                            PublicBetAwaySpread = publicBetSpreadAway,
                            Variance = variance,
                            Week = week,
                            Year = year

                        };
                        await db.CombinedBet.AddAsync(bet);
                    }
                    if (betHomeMoneyline)
                    {
                        //specific to this model
                        //rare to hit this model, but this variance is 2-1
                        if (variance > 3)
                        {
                            betGame = false;
                        }

                        var bet = new CombinedBetDbo
                        {
                            GamePredictionId = gamePrediction.GamePredictionId,
                            HomeTeamId = homeTeamId.TeamId,
                            AwayTeamId = awayTeamId.TeamId,
                            Bet = betGame,
                            BetType = (int)BetTypes.HomeMoneyLine,
                            Odd = 0,
                            PublicBetOver = publicBetOver,
                            PublicBetUnder = publicBetUnder,
                            PublicBetHomeMoneyline = publicBetMoneylineHome,
                            PublicBetAwayMoneyline = publicBetMoneylineAway,
                            PublicBetHomeSpread = publicBetSpreadHome,
                            PublicBetAwaySpread = publicBetSpreadAway,
                            Variance = variance,
                            Week = week,
                            Year = year

                        };
                        await db.CombinedBet.AddAsync(bet);
                    }
                    if (betAwayMoneyline)
                    {
                        //specific to this model
                        //the odds suck 4-4, 5-5, 2-4... no winning combo
                        betGame = false;

                        var bet = new CombinedBetDbo
                        {
                            GamePredictionId = gamePrediction.GamePredictionId,
                            HomeTeamId = homeTeamId.TeamId,
                            AwayTeamId = awayTeamId.TeamId,
                            Bet = betGame,
                            BetType = (int)BetTypes.AwayMoneyLine,
                            Odd = 0,
                            PublicBetOver = publicBetOver,
                            PublicBetUnder = publicBetUnder,
                            PublicBetHomeMoneyline = publicBetMoneylineHome,
                            PublicBetAwayMoneyline = publicBetMoneylineAway,
                            PublicBetHomeSpread = publicBetSpreadHome,
                            PublicBetAwaySpread = publicBetSpreadAway,
                            Variance = variance,
                            Week = week,
                            Year = year

                        };
                        await db.CombinedBet.AddAsync(bet);
                    }
                    if (betOver)
                    {
                        //specific to this model
                        //rare to hit this model, but this variance is 10-6
                        if (variance != 3 || variance != 4)
                        {
                            betGame = false;
                        }
                        //betGame = GetBetBool(betGame, BetTypes.Over, publicBetMoneylineAway, publicBetMoneylineHome, publicBetSpreadAway, publicBetSpreadHome, publicBetOver, publicBetUnder);

                        var bet = new CombinedBetDbo
                        {
                            GamePredictionId = gamePrediction.GamePredictionId,
                            HomeTeamId = homeTeamId.TeamId,
                            AwayTeamId = awayTeamId.TeamId,
                            Bet = betGame,
                            BetType = (int)BetTypes.Over,
                            Odd = overUnder.Value,
                            PublicBetOver = publicBetOver,
                            PublicBetUnder = publicBetUnder,
                            PublicBetHomeMoneyline = publicBetMoneylineHome,
                            PublicBetAwayMoneyline = publicBetMoneylineAway,
                            PublicBetHomeSpread = publicBetSpreadHome,
                            PublicBetAwaySpread = publicBetSpreadAway,
                            Variance = variance,
                            Week = week,
                            Year = year

                        };
                        await db.CombinedBet.AddAsync(bet);
                    }
                    if (betUnder)
                    {
                        //specific to this model
                        if (variance >= 8 || variance <= 4)
                        {
                            betGame = false;
                        }
                        //betGame = GetBetBool(betGame, BetTypes.Under, publicBetMoneylineAway, publicBetMoneylineHome, publicBetSpreadAway, publicBetSpreadHome, publicBetOver, publicBetUnder);

                        var bet = new CombinedBetDbo
                        {
                            GamePredictionId = gamePrediction.GamePredictionId,
                            HomeTeamId = homeTeamId.TeamId,
                            AwayTeamId = awayTeamId.TeamId,
                            Bet = betGame,
                            BetType = (int)BetTypes.Under,
                            Odd = overUnder.Value,
                            PublicBetOver = publicBetOver,
                            PublicBetUnder = publicBetUnder,
                            PublicBetHomeMoneyline = publicBetMoneylineHome,
                            PublicBetAwayMoneyline = publicBetMoneylineAway,
                            PublicBetHomeSpread = publicBetSpreadHome,
                            PublicBetAwaySpread = publicBetSpreadAway,
                            Variance = variance,
                            Week = week,
                            Year = year

                        };
                        await db.CombinedBet.AddAsync(bet);
                    }

                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task<List<GetBetResponseModel>> GetBets(int week, int year, bool includeBadBets, int? variance)
        {
            using (var db = await factory.CreateDbContextAsync())
            {
                var bets = db.Bet.Where(x => x.Week == week && x.Year == year);

                if (!includeBadBets)
                {
                    bets = bets.Where(x => x.Bet == true);
                }

                if (variance.HasValue)
                {
                    bets = bets.Where(x => x.Variance <= variance);
                }

                var rawBets = await (from b in bets
                                     select new
                                     {
                                         b.BetId,
                                         b.Bet,
                                         b.HomeTeamId,
                                         b.AwayTeamId,
                                         b.BetType,
                                         b.Odd,
                                         b.PublicBetAwayMoneyline,
                                         b.PublicBetHomeMoneyline,
                                         b.PublicBetAwaySpread,
                                         b.PublicBetHomeSpread,
                                         b.PublicBetOver,
                                         b.PublicBetUnder,
                                         b.Variance
                                     }).ToListAsync();

                var conferenceTeams = await (from t in db.ConferenceTeam
                                             select t).ToListAsync();

                var response = (from b in rawBets
                                    // orderby b.Variance
                                select new GetBetResponseModel
                                {
                                    BetId = b.BetId,
                                    Bet = b.Bet,
                                    HomeTeamName = (from t in conferenceTeams
                                                    where t.TeamId == b.HomeTeamId
                                                    select t.TeamName).First(),
                                    AwayTeamName = (from t in conferenceTeams
                                                    where t.TeamId == b.AwayTeamId
                                                    select t.TeamName).First(),
                                    BetTypeName = ((BetTypes)b.BetType).ToString(),
                                    Odd = b.Odd,
                                    Variance = b.Variance
                                }).ToList();

                return response;
            }
        }

        private string GetEspnTeamName(string teamName)
        {
            switch (teamName)
            {
                case "Hawaii Rainbow Warriors":
                    return "Hawai'i Rainbow Warriors";
                case "San Jose State Spartans":
                    return "San José State Spartans";
                default:
                    return teamName;
            }
        }

        private bool GetBetBool(bool bet, BetTypes betType, bool awayMl, bool homeMl, bool awaySpread, bool homeSpread, bool over, bool under)
        {
            if (!bet)
            {
                return false;
            }
            switch (betType)
            {
                case BetTypes.HomeSpread:
                    if (homeSpread)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case BetTypes.AwaySpread:
                    if (awaySpread)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case BetTypes.HomeMoneyLine:
                    if (homeMl)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case BetTypes.AwayMoneyLine:
                    if (awayMl)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case BetTypes.Over:
                    if (over)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case BetTypes.Under:
                    if (under)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                default:
                    return false;
            }
        }

        private enum BetTypes : int
        {
            None = 0,
            HomeSpread = 1,
            AwaySpread = 2,
            HomeMoneyLine = 3,
            AwayMoneyLine = 4,
            Over = 5,
            Under = 6
        }
    }
}
