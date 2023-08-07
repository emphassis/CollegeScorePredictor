using CollegeScorePredictor.Models.BettingData;
using System.Text.Json;

namespace CollegeScorePredictor.Operations
{
    public static class OddsOperations
    {

        public static async Task<BettingDataOddsModel> GetWeeklyOddsModel(int week, int year)
        {
            var requestModel = new BettingDataRequestModel();

            if (week != 0)
            {
                requestModel.filters.week = week;
            }

            if (year != 0)
            {
                requestModel.filters.season = year;
            }

            var client = new HttpClient();
            var apiUrl = "https://bettingdata.com/CFB_Odds/Odds_Read";

            var data = await client.PostAsJsonAsync(apiUrl, requestModel);

            var jsonResponse = await data.Content.ReadAsStringAsync();

            BettingDataOddsModel? model;

            try
            {
                model = JsonSerializer.Deserialize<BettingDataOddsModel>(jsonResponse);
                if (model == null)
                {
                    return new BettingDataOddsModel();
                }
            }
            catch { return new BettingDataOddsModel(); }

            return model;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private class BettingDataRequestModel
        {
            public BettingDataFilterModel filters { get; set; } = new BettingDataFilterModel();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private class BettingDataFilterModel
        {
            public int client { get; set; } = 1;
            public string? conference { get; set; } = null;
            public string? data { get; set; } = null;
            public string? exportType { get; set; } = null;
            public string? geo_state { get; set; } = null;
            public string league { get; set; } = "ncaa-football";
            public int scope { get; set; } = 1;
            public int season { get; set; } = 2022;
            public int seasontype { get; set; } = 1;
            public bool show_no_odds { get; set; } = false;
            public string state { get; set; } = "WORLD";
            public int subscope { get; set; } = 2;
            public string? team { get; set; } = null;
            public string teamkey { get; set; } = "ABCHR";
            public int week { get; set; } = 1;
            public int widget_scope { get; set; } = 1;
        }
    }
}
