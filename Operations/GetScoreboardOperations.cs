using CollegeScorePredictor.Models.EspnScoreboard;
using System.Text.Json;

namespace CollegeScorePredictor.Operations
{
    public static class GetScoreboardOperations
    {
        public static async Task<EspnScoreboardModel> GetWeekGamesAsync(string startDate)
        {
            var endDate = DateOperations.GetEndDate(startDate);
            var apiUrl = "https://site.api.espn.com/apis/site/v2/sports/football/college-football/scoreboard?dates=" + startDate + "-" + endDate + "&limit=150";
            EspnScoreboardModel? model;
            try
            {
                var client = new HttpClient();
                var data = await client.GetStringAsync(apiUrl);
                model = JsonSerializer.Deserialize<EspnScoreboardModel>(data);

                if(model != null)
                    return model;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to get week " + startDate);
               // Console.WriteLine(ex.Message);
            }

            return new EspnScoreboardModel();
        }
    }
}
