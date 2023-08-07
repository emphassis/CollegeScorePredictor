using CollegeScorePredictor.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace CollegeScorePredictor.Operations
{
    public static class TeamOperations
    {
        public static async Task<bool> DoesTeamExistAsync(AppDbContext db, long teamId)
        {
            return await (from t in db.ConferenceTeam
                   where t.TeamId == teamId
                   select t).AnyAsync();
        }

        public static async Task SaveTeamAsync(AppDbContext db, ConferenceTeamDbo model)
        {
            db.ConferenceTeam.Add(model);
            await db.SaveChangesAsync();
        }

        public static long GetTeamId(string? teamId)
        {
            if (string.IsNullOrEmpty(teamId)) return 0;

            try
            {
                return Int64.Parse(teamId);
            }
            catch
            {
                return 0;
            }
        }
    }
}
