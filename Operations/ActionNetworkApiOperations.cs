using CollegeScorePredictor.Models.ActionNetwork;
using System.Text.Json;

namespace CollegeScorePredictor.Operations
{
    public class ActionNetworkApiOperations
    {
        public static async Task<ActionNetworkOddsModel> GetOdds(int week, int year)
        {

            var client = new HttpClient();
            var apiUrl = "https://api.actionnetwork.com/web/v1/scoreboard/ncaaf?period=game&division=FBS&week=" + week + "&season=" + year;
            var data = await client.GetStringAsync(apiUrl);

            JsonSerializerOptions options = new() { WriteIndented = true, UnknownTypeHandling = System.Text.Json.Serialization.JsonUnknownTypeHandling.JsonElement, NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString, DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull };
            ActionNetworkOddsModel model = JsonSerializer.Deserialize<ActionNetworkOddsModel>(data, options)!;
            if (model == null) return new ActionNetworkOddsModel();

            return model;
        }
    }
}
