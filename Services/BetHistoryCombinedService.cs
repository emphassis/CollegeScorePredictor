using CollegeScorePredictor.Models.Bet;
using Microsoft.EntityFrameworkCore;

namespace CollegeScorePredictor.Services
{
    public class BetHistoryCombinedService
    {
        private IDbContextFactory<AppDbContext> factory;
        public BetHistoryCombinedService(IDbContextFactory<AppDbContext> _factory)
        {
            factory = _factory;
        }

        public async Task<GetBetHistoryResponseModel> CombineResultsAsync(int week, int year, bool onlyQualified)
        {
            using (var db = await factory.CreateDbContextAsync())
            {
                var conferenceTeams = await (from c in db.ConferenceTeam
                                             select c).ToListAsync();


                //var combinedBets = await (from bhc in db.CombinedBetHistory
                //                            join bc in db.CombinedBet on bhc.CombinedBetId equals bc.CombinedBetId
                //                            where bc.Week == week && bc.Year == year
                //                            select new
                //                            {
                //                                bhc.CombinedBetId,
                //                                bhc.Won,
                //                                bc.AwayTeamId,
                //                                bc.HomeTeamId,
                //                                bc.BetType,
                //                                bc.Odd,
                //                                bc.Variance,
                //                                bc.Bet
                //                            }).ToListAsync();

                var conferenceBets = await (from bhc in db.BetHistoryConference
                                            join bc in db.BetConference on bhc.BetConferenceId equals bc.BetConferenceId
                                            where bc.Week == week && bc.Year == year
                                            select new
                                            {
                                                bhc.BetConferenceId,
                                                bhc.Won,
                                                bc.AwayTeamId,
                                                bc.HomeTeamId,
                                                bc.BetType,
                                                bc.Odd,
                                                bc.Variance,
                                                bc.Bet
                                            }).ToListAsync();


                var bets = await (from bh in db.BetHistory
                                  join b in db.Bet on bh.BetId equals b.BetId
                                  where b.Week == week && b.Year == year
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
                                  }).ToListAsync();

                if (onlyQualified)
                {
                    conferenceBets = conferenceBets.Where(x => x.Bet).ToList();
                    bets = bets.Where(x => x.Bet).ToList();
                }

                var history = new List<BetHistorySearchModel>();

                foreach (var b in conferenceBets)
                {
                    if (b.BetType == (int)BetTypes.HomeSpread)
                    {
                        var badSpread = bets.Where(x => x.AwayTeamId == b.AwayTeamId && x.BetType == (int)BetTypes.HomeSpread).FirstOrDefault();
                        if (badSpread != null)
                        {
                            history.Add(new BetHistorySearchModel
                            {
                                BetId = b.BetConferenceId,
                                Won = b.Won,
                                AwayTeamId = b.AwayTeamId,
                                HomeTeamId = b.HomeTeamId,
                                BetType = b.BetType,
                                Variance = b.Variance,
                                Odd = b.Odd,
                                Bet = b.Bet
                            });
                        }
                    }
                    if (b.BetType == (int)BetTypes.AwaySpread)
                    {
                        var badSpread = bets.Where(x => x.AwayTeamId == b.AwayTeamId && x.BetType == (int)BetTypes.AwaySpread).FirstOrDefault();
                        if (badSpread != null)
                        {
                            history.Add(new BetHistorySearchModel
                            {
                                BetId = b.BetConferenceId,
                                Won = b.Won,
                                AwayTeamId = b.AwayTeamId,
                                HomeTeamId = b.HomeTeamId,
                                BetType = b.BetType,
                                Variance = b.Variance,
                                Odd = b.Odd,
                                Bet = b.Bet
                            });
                        }
                    }
                    if (b.BetType == (int)BetTypes.HomeMoneyLine)
                    {
                        var badMoneyline = bets.Where(x => x.AwayTeamId == b.AwayTeamId && x.BetType == (int)BetTypes.HomeMoneyLine).FirstOrDefault();
                        if (badMoneyline != null)
                        {
                            history.Add(new BetHistorySearchModel
                            {
                                BetId = b.BetConferenceId,
                                Won = b.Won,
                                AwayTeamId = b.AwayTeamId,
                                HomeTeamId = b.HomeTeamId,
                                BetType = b.BetType,
                                Variance = b.Variance,
                                Odd = b.Odd,
                                Bet = b.Bet
                            });
                        }
                    }
                    if (b.BetType == (int)BetTypes.AwayMoneyLine)
                    {
                        var badMoneyline = bets.Where(x => x.AwayTeamId == b.AwayTeamId && x.BetType == (int)BetTypes.AwayMoneyLine).FirstOrDefault();
                        if (badMoneyline != null)
                        {
                            history.Add(new BetHistorySearchModel
                            {
                                BetId = b.BetConferenceId,
                                Won = b.Won,
                                AwayTeamId = b.AwayTeamId,
                                HomeTeamId = b.HomeTeamId,
                                BetType = b.BetType,
                                Variance = b.Variance,
                                Odd = b.Odd,
                                Bet = b.Bet
                            });
                        }
                    }
                    if (b.BetType == (int)BetTypes.Over)
                    {
                        var badOvers = bets.Where(x => x.AwayTeamId == b.AwayTeamId && x.BetType == (int)BetTypes.Under).FirstOrDefault();
                        if (badOvers == null)
                        {
                            history.Add(new BetHistorySearchModel
                            {
                                BetId = b.BetConferenceId,
                                Won = b.Won,
                                AwayTeamId = b.AwayTeamId,
                                HomeTeamId = b.HomeTeamId,
                                BetType = b.BetType,
                                Variance = b.Variance,
                                Odd = b.Odd,
                                Bet = b.Bet
                            });
                        }
                    }
                    if (b.BetType == (int)BetTypes.Under)
                    {
                        var badUnders = bets.Where(x => x.AwayTeamId == b.AwayTeamId && x.BetType == (int)BetTypes.Over).FirstOrDefault();
                        if (badUnders == null)
                        {
                            history.Add(new BetHistorySearchModel
                            {
                                BetId = b.BetConferenceId,
                                Won = b.Won,
                                AwayTeamId = b.AwayTeamId,
                                HomeTeamId = b.HomeTeamId,
                                BetType = b.BetType,
                                Variance = b.Variance,
                                Odd = b.Odd,
                                Bet = b.Bet
                            });
                        }
                    }
                }

                foreach (var b in bets)
                {
                    if(history.Where(x=>x.HomeTeamId == b.HomeTeamId && x.BetType == b.BetType).Any())
                    {
                        continue;
                    }

                    if (b.BetType == (int)BetTypes.HomeSpread)
                    {
                        var badSpread = conferenceBets.Where(x => x.AwayTeamId == b.AwayTeamId && x.BetType == (int)BetTypes.HomeSpread).FirstOrDefault();
                        if (badSpread != null)
                        {
                            history.Add(new BetHistorySearchModel
                            {
                                BetId = b.BetId,
                                Won = b.Won,
                                AwayTeamId = b.AwayTeamId,
                                HomeTeamId = b.HomeTeamId,
                                BetType = b.BetType,
                                Variance = b.Variance,
                                Odd = b.Odd,
                                Bet = b.Bet
                            });
                        }
                    }
                    if (b.BetType == (int)BetTypes.AwaySpread)
                    {
                        var badSpread = conferenceBets.Where(x => x.AwayTeamId == b.AwayTeamId && x.BetType == (int)BetTypes.AwaySpread).FirstOrDefault();
                        if (badSpread != null)
                        {
                            history.Add(new BetHistorySearchModel
                            {
                                BetId = b.BetId,
                                Won = b.Won,
                                AwayTeamId = b.AwayTeamId,
                                HomeTeamId = b.HomeTeamId,
                                BetType = b.BetType,
                                Variance = b.Variance,
                                Odd = b.Odd,
                                Bet = b.Bet
                            });
                        }
                    }
                    if (b.BetType == (int)BetTypes.HomeMoneyLine)
                    {
                        var badMoneyline = conferenceBets.Where(x => x.AwayTeamId == b.AwayTeamId && x.BetType == (int)BetTypes.HomeMoneyLine).FirstOrDefault();
                        if (badMoneyline != null)
                        {
                            history.Add(new BetHistorySearchModel
                            {
                                BetId = b.BetId,
                                Won = b.Won,
                                AwayTeamId = b.AwayTeamId,
                                HomeTeamId = b.HomeTeamId,
                                BetType = b.BetType,
                                Variance = b.Variance,
                                Odd = b.Odd,
                                Bet = b.Bet
                            });
                        }
                    }
                    if (b.BetType == (int)BetTypes.AwayMoneyLine)
                    {
                        var badMoneyline = conferenceBets.Where(x => x.AwayTeamId == b.AwayTeamId && x.BetType == (int)BetTypes.AwayMoneyLine).FirstOrDefault();
                        if (badMoneyline != null)
                        {
                            history.Add(new BetHistorySearchModel
                            {
                                BetId = b.BetId,
                                Won = b.Won,
                                AwayTeamId = b.AwayTeamId,
                                HomeTeamId = b.HomeTeamId,
                                BetType = b.BetType,
                                Variance = b.Variance,
                                Odd = b.Odd,
                                Bet = b.Bet
                            });
                        }
                    }
                    if (b.BetType == (int)BetTypes.Over)
                    {
                        var badOvers = conferenceBets.Where(x => x.AwayTeamId == b.AwayTeamId && x.BetType == (int)BetTypes.Under).FirstOrDefault();
                        if (badOvers == null)
                        {
                            history.Add(new BetHistorySearchModel
                            {
                                BetId = b.BetId,
                                Won = b.Won,
                                AwayTeamId = b.AwayTeamId,
                                HomeTeamId = b.HomeTeamId,
                                BetType = b.BetType,
                                Variance = b.Variance,
                                Odd = b.Odd,
                                Bet = b.Bet
                            });
                        }
                    }
                    if (b.BetType == (int)BetTypes.Under)
                    {
                        var badUnders = conferenceBets.Where(x => x.AwayTeamId == b.AwayTeamId && x.BetType == (int)BetTypes.Over).FirstOrDefault();
                        if (badUnders == null)
                        {
                            history.Add(new BetHistorySearchModel
                            {
                                BetId = b.BetId,
                                Won = b.Won,
                                AwayTeamId = b.AwayTeamId,
                                HomeTeamId = b.HomeTeamId,
                                BetType = b.BetType,
                                Variance = b.Variance,
                                Odd = b.Odd,
                                Bet = b.Bet
                            });
                        }
                    }
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
        private class BetHistorySearchModel
        {
            public long BetId { get; set; }
            public bool Won { get; set; }
            public long AwayTeamId { get; set; }
            public long HomeTeamId { get; set; }
            public int BetType { get; set; }
            public int Variance { get; set; }
            public double Odd { get; set; }
            public bool Bet { get; set; }
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
