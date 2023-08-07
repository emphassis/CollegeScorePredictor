namespace CollegeScorePredictor.Models.ActionNetwork
{
#pragma warning disable IDE1006 // Naming Styles
    public class ActionNetworkOddsModel
    {
        public int content_live_count { get; set; }
        public List<GamesModel> games { get; set; } = new List<GamesModel>();
        public object? league { get; set; }
    }

    public class GamesModel
    {
        public int attendance { get; set; }
        public int away_rotation_number { get; set; }
        public int away_team_id { get; set; }
        public object? boxscore { get; set; }
        public object? broadcasts { get; set; }
        public string? coverage { get; set; }
        public int home_rotation_number { get; set; }
        public int home_team_id { get; set; }
        public int id { get; set; }
        public bool is_free { get; set; }
        public object? last_play { get; set; }
        public string? league_name { get; set; }
        public object? meta { get; set; }
        public List<OddsModel> odds { get; set; } = new List<OddsModel>();
        public string? real_status { get; set; }
        public int season { get; set; }
        public string? start_time { get; set; }
        public string? status { get; set; }
        public string? status_display { get; set; }
        public List<TeamsModel> teams { get; set; } = new List<TeamsModel>();
        public bool trending { get; set; }
        public string? type { get; set; }
        public object? winning_team_id { get; set; }
    }

    public class OddsModel
    {
        public double? away_over { get; set; }
        public double? away_total { get; set; }
        public double? away_under { get; set; }
        public double? book_id { get; set; }
        public object? draw { get; set; }
        public double? home_over { get; set; }
        public double? home_total { get; set; }
        public double? home_under { get; set; }
        public string? inserted { get; set; }
        public object? line_status { get; set; }
        public object? meta { get; set; }
        public double? ml_away { get; set; }//payout
        public double? ml_away_money { get; set; }
        public double? ml_away_public { get; set; }
        public double? ml_home { get; set; }//payout
        public double? ml_home_money { get; set; }
        public double? ml_home_public { get; set; }
        public double? num_bets { get; set; }
        public double? over { get; set; }//payout -110
        public double? spread_away { get; set; }//this is the odd
        public double? spread_away_line { get; set; }//payout
        public double? spread_away_money { get; set; }
        public double? spread_away_public { get; set; }
        public double? spread_home { get; set; }//this is the odd
        public double? spread_home_line { get; set; }//payout
        public double? spread_home_money { get; set; }
        public double? spread_home_public { get; set; }
        public double? total { get; set; }//this is the odd
        public double? total_over_money { get; set; }
        public double? total_over_public { get; set; }
        public double? total_under_money { get; set; }
        public double? total_under_public { get; set; }
        public string? type { get; set; }
        public double? under { get; set; }//payout for under
    }

    public class TeamsModel
    {
        public string? abbr { get; set; }
        public string? conference_type { get; set; }
        public string? division_name { get; set; }
        public string? division_type { get; set; }
        public string? full_name { get; set; }
        public int id { get; set; }
        public string? location { get; set; }
        public string? logo { get; set; }
        public string? primary_color { get; set; }
        public string? secondary_color { get; set; }
        public string? short_name { get; set; }
        public object? standings { get; set; }
        public string? url_slug { get; set; }
    }
#pragma warning restore IDE1006 // Naming Styles
}
