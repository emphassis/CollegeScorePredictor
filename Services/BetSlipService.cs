using CollegeScorePredictor.Models.Bet;
using Microsoft.EntityFrameworkCore;
using static CollegeScorePredictor.Constants.Enums;

namespace CollegeScorePredictor.Services
{
    public class BetSlipService
    {
        private IDbContextFactory<AppDbContext> factory;
        public BetSlipService(IDbContextFactory<AppDbContext> _factory)
        {
            factory = _factory;
        }


        public async Task<List<BetSlip>> GetBetSlips(int week, int year, bool includeBadBets, int? variance)
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

                var response = new List<BetSlip>();

                foreach (var bet in rawBets)
                {
                    var homeTeamName = (from t in conferenceTeams
                                        where t.TeamId == bet.HomeTeamId
                                        select t.TeamName).First();
                    var awayTeamName = (from t in conferenceTeams
                                        where t.TeamId == bet.AwayTeamId
                                        select t.TeamName).First();

                    var betSlip = new BetSlip
                    {
                        Bet = GetBetString((BetTypes)bet.BetType, homeTeamName, awayTeamName, bet.Odd),
                        HomeTeamName = (from t in conferenceTeams
                                        where t.TeamId == bet.HomeTeamId
                                        select t.TeamName).First(),
                        AwayTeamName = (from t in conferenceTeams
                                        where t.TeamId == bet.AwayTeamId
                                        select t.TeamName).First(),
                        BetName = GetBetNameString((BetTypes)bet.BetType)
                    };
                    response.Add(betSlip);
                }

                return response;
            }
        }

        private string GetBetNameString(BetTypes betType)
        {
            switch (betType)
            {
                case BetTypes.HomeSpread:
                    return "Spread";
                case BetTypes.AwaySpread:
                    return "Spread";
                case BetTypes.HomeMoneyLine:
                    return "Spread";
                case BetTypes.AwayMoneyLine:
                    return "Spread";
                case BetTypes.Over:
                    return "Total Points";
                case BetTypes.Under:
                    return "Total Points";
                default:
                    return "GetBetNameString Failed, lo siento!";
            }
        }

        private string GetBetString(BetTypes betType, string homeTeam, string awayTeam, double odd)
        {
            switch (betType)
            {
                case BetTypes.HomeSpread:
                    return homeTeam + " " + odd;
                case BetTypes.AwaySpread:
                    return awayTeam + " " + odd;
                case BetTypes.HomeMoneyLine:
                    return homeTeam + " " + odd;
                case BetTypes.AwayMoneyLine:
                    return awayTeam + " " + odd;
                case BetTypes.Over:
                    return "Over " + odd;
                case BetTypes.Under:
                    return "Under " + odd;
                default:
                    return "GetBetString Failed, lo siento!";
            }
        }
    }
}
