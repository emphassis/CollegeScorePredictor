﻿using CollegeScorePredictor.Models.Bet;
using CollegeScorePredictor.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace CollegeScorePredictor.Services
{
    public class BetHistoryService
    {
        private IDbContextFactory<AppDbContext> factory;
        public BetHistoryService(IDbContextFactory<AppDbContext> _factory)
        {
            factory = _factory;
        }

        public async Task CreateHistory(int week, int year)
        {
            using (var db = await factory.CreateDbContextAsync())
            {
                var bets = await (from b in db.Bet
                                  where b.Week == week
                                  && b.Year == year
                                  select b).ToListAsync();

                foreach (var bet in bets)
                {
                    var game = await (from g in db.GameResult
                                      where g.AwayTeamId == bet.AwayTeamId && g.HomeTeamId == bet.HomeTeamId
                                      && g.Week == week && g.Year == year
                                      select g).FirstOrDefaultAsync();

                    if (game == null)
                    {
                        Console.WriteLine("bet history game is null");
                        continue;
                    }

                    var wonBet = false;

                    if(bet.BetType == (int)BetTypes.HomeSpread)
                    {
                        if(game.HomeTeamScore + bet.Odd > game.AwayTeamScore)
                        {
                            wonBet = true;
                        }
                    }

                    if (bet.BetType == (int)BetTypes.AwaySpread)
                    {
                        if (game.AwayTeamScore + bet.Odd > game.HomeTeamScore)
                        {
                            wonBet = true;
                        }
                    }

                    if (bet.BetType == (int)BetTypes.HomeMoneyLine)
                    {
                        if (game.HomeTeamScore > game.AwayTeamScore)
                        {
                            wonBet = true;
                        }
                    }

                    if (bet.BetType == (int)BetTypes.AwayMoneyLine)
                    {
                        if (game.AwayTeamScore > game.HomeTeamScore)
                        {
                            wonBet = true;
                        }
                    }

                    if (bet.BetType == (int)BetTypes.Over)
                    {
                        if ((game.AwayTeamScore + game.HomeTeamScore) > bet.Odd)
                        {
                            wonBet = true;
                        }
                    }

                    if (bet.BetType == (int)BetTypes.Under)
                    {
                        if ((game.AwayTeamScore + game.HomeTeamScore) < bet.Odd)
                        {
                            wonBet = true;
                        }
                    }

                    var betHistory = new BetHistoryDbo
                    {
                        BetId = bet.BetId,
                        Week = week,
                        Year = year,
                        Won = wonBet
                    };
                    await db.BetHistory.AddAsync(betHistory);

                    if (wonBet)
                    {
                        Console.WriteLine("Won Bet!");
                    }
                    else
                    {
                        Console.WriteLine("Lost Bet... :(");
                    }
                }

                await db.SaveChangesAsync();
            }
        }

        public async Task<GetBetHistoryResponseModel> GetHistory(int week, int year, bool onlyWon = false, bool onlyQualified = true)
        {
            using(var db = await factory.CreateDbContextAsync())
            {
                var conferenceTeams = await (from c in db.ConferenceTeam
                                             select c).ToListAsync();

                var betHistory = db.BetHistory.Where(x => x.Week == week && x.Year == year);

                if(onlyWon)
                {
                    betHistory = betHistory.Where(x=>x.Won == onlyWon);
                }

                var history = await (from bh in betHistory
                                     join b in db.Bet on bh.BetId equals b.BetId
                                     //orderby bh.Won
                                     orderby b.Variance
                                     select new
                                     {
                                         bh.BetId,
                                         bh.Won,
                                         b.AwayTeamId,
                                         b.HomeTeamId,
                                         b.BetType,
                                         b.Variance,
                                         b.Odd,
                                         b.Bet
                                     }).Take(20).ToListAsync();

                if (onlyQualified)
                {
                    history = history.Where(x => x.Bet).ToList();
                }

                var betHistoryList = new List<BetHistoryModel>();
                var betsLost = history.Where(x => !x.Won).Count();
                var betsWon = history.Where(x => x.Won).Count();
                double profit = 0;

                #region Calculate Profit Percentage
                var wonBetWinnings = 9.1;
                var betAmount = 10;

                var betWinnings = betsWon * wonBetWinnings;
                var betsWonAnte = betsWon * betAmount;

                var totalProfit = betWinnings + betsWonAnte;

                var totalAnte = history.Count() * betAmount;

                profit = ((totalProfit / totalAnte) - 1) * 100;
                #endregion

                foreach (var bet in history)
                {
                    var betModel = new BetHistoryModel
                    {
                        BetId = bet.BetId,
                        Won = bet.Won,
                        HomeTeamName = (from t in conferenceTeams
                                        where t.TeamId == bet.HomeTeamId
                                        select t.TeamName).First(),
                        AwayTeamName = (from t in conferenceTeams
                                        where t.TeamId == bet.AwayTeamId
                                        select t.TeamName).First(),
                        BetTypeName = ((BetTypes)bet.BetType).ToString(),
                        Variance = bet.Variance,
                        Odd = bet.Odd
                    };
                    betHistoryList.Add(betModel);
                }

                var response = new GetBetHistoryResponseModel
                {
                    BetHistory = betHistoryList,
                    BetsLost = betsLost,
                    BetsWon = betsWon,
                    Profit = profit
                };

                return response;
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
