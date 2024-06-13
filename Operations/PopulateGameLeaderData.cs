using CollegeScorePredictor.Models.EspnGameSummary;
using CollegeScorePredictor.Models.Record;
using CollegeScorePredictor.Services;
using System.Text.Json;

namespace CollegeScorePredictor.Operations
{
    public static class PopulateGameLeaderData
    {
        public static async Task<PopulateGameLeaderDataModel> PopulateGameLeaderDataAsync(string eventId)
        {
            try
            {
                var client = new HttpClient();
                var apiUrl = Constants.System.EspnBaseUrl + Constants.System.EspnGameSummaryApi + eventId;
                var data = await client.GetStringAsync(apiUrl);


                //JsonSerializerOptions options = new() { WriteIndented = true, UnknownTypeHandling = System.Text.Json.Serialization.JsonUnknownTypeHandling.JsonElement, NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString, DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull };
                EspnGameSummaryModel model = JsonSerializer.Deserialize<EspnGameSummaryModel>(data)!;//, options)!;

                var isModelValid = AnalyzeGameOperations.IsGameModelValid(model!);
                if (!isModelValid)
                {
                    //Console.WriteLine("game model not valid");
                }


                var gameModel = model!;

                Teams awayTeam = gameModel.boxscore.teams.First();
                Teams homeTeam = gameModel.boxscore.teams.Last();

                var leaderModel = AnalyzeGameOperations.GetLeadersModel(gameModel.leaders!, long.Parse(homeTeam.team.id!), long.Parse(awayTeam.team.id!));
                var response = new PopulateGameLeaderDataModel
                {
                    GameLeadersModel = leaderModel,
                    HomeTeamId = long.Parse(homeTeam.team.id!),
                    AwayTeamId = long.Parse(awayTeam.team.id!)
                };

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
