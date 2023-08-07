using CollegeScorePredictor.Models.Database;
using CollegeScorePredictor.Models.EspnScoreboard;
using CollegeScorePredictor.Models.Record;
using CollegeScorePredictor.Operations;
using Microsoft.EntityFrameworkCore;

namespace CollegeScorePredictor.Services
{
    public class RecordGamesService
    {
        private IDbContextFactory<AppDbContext> factory;
        public RecordGamesService(IDbContextFactory<AppDbContext> _factory)
        {
            factory = _factory;
        }

        public async Task AddConferenceTeamsToGames(int year)
        {
            using(var db = await factory.CreateDbContextAsync())
            {
                var games = await(from g in db.GameResult
                                  //where g.Year == year
                                  select g).ToListAsync();

                foreach(var game in games)
                {
                    var homeTeamConference = await (from c in db.ConferenceTeam
                                                    where c.TeamId == game.HomeTeamId
                                                    select c).FirstOrDefaultAsync();
                    var awayTeamConference = await (from c in db.ConferenceTeam
                                                    where c.TeamId == game.AwayTeamId
                                                    select c).FirstOrDefaultAsync();

                    if(homeTeamConference == null)
                    {
                        homeTeamConference = new ConferenceTeamDbo();
                        homeTeamConference.TeamConference = 100;
                    }

                    if (awayTeamConference == null)
                    {  
                        awayTeamConference = new ConferenceTeamDbo();
                        awayTeamConference.TeamConference = 100;
                    }

                    game.HomeTeamConference = homeTeamConference.TeamConference;
                    game.AwayTeamConference = awayTeamConference.TeamConference;
                }
                Console.WriteLine("Saving Games");
                await db.SaveChangesAsync();
            }
        }

        public async Task RecordAllGames()
        {
            //var weeks = DateOperations.Get2002StartDateList(); These two are too old. The model is different and I'm not about to write a new model to match.
            //var weeks = DateOperations.Get2003StartDateList(); These two are too old. The model is different and I'm not about to write a new model to match.
            //var weeks = DateOperations.Get2004StartDateList();
            //var weeks = DateOperations.Get2005StartDateList();
            //var weeks = DateOperations.Get2006StartDateList();
            //var weeks = DateOperations.Get2007StartDateList();
            //var weeks = DateOperations.Get2008StartDateList();
            //var weeks = DateOperations.Get2009StartDateList();
            //var weeks = DateOperations.Get2010StartDateList();
            //var weeks = DateOperations.Get2011StartDateList();
            //var weeks = DateOperations.Get2012StartDateList();
            //var weeks = DateOperations.Get2013StartDateList();
            //var weeks = DateOperations.Get2014StartDateList();
            //var weeks = DateOperations.Get2015StartDateList();
            //var weeks = DateOperations.Get2016StartDateList();
            //var weeks = DateOperations.Get2017StartDateList();
            //var weeks = DateOperations.Get2018StartDateList();
            //var weeks = DateOperations.Get2019StartDateList();
            //var weeks = DateOperations.Get2020StartDateList();
            //still need to run 2021 and 2022, just don't make OffensiveModels out of them
            //var weeks = DateOperations.Get2021StartDateList();
            var weeks = DateOperations.Get2022StartDateList();
            // var weeks = DateOperations.Get2023StartDateList();

            foreach (var week in weeks)
            {
                Console.WriteLine("Week: " + week);
                var games = await GetScoreboardOperations.GetWeekGamesAsync(week);
                await LoopThroughWeekGames(games);
            }
        }

        public async Task LoopThroughWeekGames(EspnScoreboardModel games)
        {
            using (var db = await factory.CreateDbContextAsync())
            {
                foreach (var game in games.events)
                {
                    var eventId = Int64.Parse(game.id!);

                    if (eventId < 1)
                    {
                        Console.WriteLine("Failed to get EventId");
                        continue;
                    }

                    //get the game data
                    var dto = await AnalyzeGameOperations.AnalyzeGame(db, eventId);
                    if (dto != null)
                    {
                        await AddGame(db, dto);
                    }
                }

                //save game data to main table to use for other tables
                await SaveGames(db);
            }
        }


        public async Task AddGame(AppDbContext db, GameResultDbo game)
        {
            //Console.WriteLine("adding game " + game.AwayTeamName + " at " + game.HomeTeamName);
            await db.GameResult.AddAsync(game);
        }

        public async Task SaveGames(AppDbContext db)
        {
            Console.WriteLine("saving games");
            await db.SaveChangesAsync();
        }


        public async Task GetGameData(GetGameRequestModel requestModel)
        {
            //Was going somewhere else with this. Not sure I'll ever need this model, but this is how you get it from the scoreboare api
            //var model = new GetGameRequestModel
            //{
            //    EventId = game.id!,
            //    Week = game.week.number,
            //    Year = game.season.year,
            //    PlayByPlayAvailable = game.competitions.First().playByPlayAvailable,
            //    HomeTeamId = Int64.Parse(game.competitions.First().competitors.Where(x => x.homeAway == "home").First().id!),
            //    AwayTeamId = Int64.Parse(game.competitions.First().competitors.Where(x => x.homeAway == "away").First().id!)
            //};
        }
    }
}
