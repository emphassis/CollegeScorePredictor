using CollegeScorePredictor.AIModels.OffensivePrediction.Models;
using CollegeScorePredictor.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace CollegeScorePredictor
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<ConferenceTeamDbo> ConferenceTeam { get; set; }
        public DbSet<GameResultDbo> GameResult { get; set; }
        public DbSet<OffensiveModelDbo> OffensiveModel { get; set; }
        public DbSet<BetDbo> Bet { get; set; }
        public DbSet<BetHistoryDbo> BetHistory { get; set; }
        public DbSet<GameLeaderDbo> GameLeader { get; set; }
        public DbSet<GamePredictionDbo> GamePrediction { get; set; }
        public DbSet<GradeModelDbo> GradeModel { get; set; }
        public DbSet<ScorePredictionDbo> ScorePrediction { get; set; }
        public DbSet<BetConferenceDbo> BetConference { get; set; }
        public DbSet<BetHistoryConferenceDbo> BetHistoryConference { get; set; }
        public DbSet<GamePredictionConferenceDbo> GamePredictionConference { get; set; }
        public DbSet<ScorePredictionConferenceDbo> ScorePredictionConference { get; set; }
        public DbSet<CombinedBetDbo> CombinedBet { get; set; }
        public DbSet<CombinedBetHistoryDbo> CombinedBetHistory { get; set; }
        public DbSet<YearlyWinDbo> YearlyWin { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConferenceTeamDbo>().ToTable("ConferenceTeam").HasKey("ConferenceTeamId");
            modelBuilder.Entity<GameResultDbo>().ToTable("GameResult").HasKey("GameResultId");
            modelBuilder.Entity<OffensiveModelDbo>().ToTable("OffensiveModel").HasKey("OffensiveModelId");
            modelBuilder.Entity<BetDbo>().ToTable("Bet").HasKey("BetId");
            modelBuilder.Entity<BetHistoryDbo>().ToTable("BetHistory").HasKey("BetId");
            modelBuilder.Entity<GameLeaderDbo>().ToTable("GameLeader").HasKey("GameLeaderId");
            modelBuilder.Entity<GamePredictionDbo>().ToTable("GamePrediction").HasKey("GamePredictionId");
            modelBuilder.Entity<GradeModelDbo>().ToTable("GradeModel").HasKey("GradeModelId");
            modelBuilder.Entity<ScorePredictionDbo>().ToTable("ScorePrediction").HasKey("ScorePredictionId");
            modelBuilder.Entity<BetConferenceDbo>().ToTable("BetConference").HasKey("BetConferenceId");
            modelBuilder.Entity<BetHistoryConferenceDbo>().ToTable("BetHistoryConference").HasKey("BetConferenceId");
            modelBuilder.Entity<GamePredictionConferenceDbo>().ToTable("GamePredictionConference").HasKey("GamePredictionConferenceId");
            modelBuilder.Entity<ScorePredictionConferenceDbo>().ToTable("ScorePredictionConference").HasKey("ScorePredictionConferenceId");
            modelBuilder.Entity<CombinedBetDbo>().ToTable("CombinedBet").HasKey("CombinedBetId");
            modelBuilder.Entity<CombinedBetHistoryDbo>().ToTable("CombinedBetHistory").HasKey("CombinedBetId");
            modelBuilder.Entity<YearlyWinDbo>().ToTable("YearlyWin").HasKey("YearlyWinId");
        }
    }
}
