using CollegeScorePredictor.AIModels.OffensivePrediction.Models;
using CollegeScorePredictor.AIModels.OverPrediction.Database;
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
        public DbSet<OverPredictionDbo> OverPrediction { get; set; }
        public DbSet<OffensiveModelDbo> OffensiveModel { get; set; }
        public DbSet<OverPredictionConferenceDbo> OverPredictionConference { get; set; }
        public DbSet<FirstDownPredictionDbo> FirstDownPrediction { get; set; }
        public DbSet<ThirdDownPredictionDbo> ThirdDownPrediction { get; set; }
        public DbSet<FourthDownPredictionDbo> FourthDownPrediction { get; set; }
        public DbSet<PassingYardsPredictionDbo> PassingYardsPrediction { get; set; }
        public DbSet<YardsPerPassPredictionDbo> YardsPerPassPrediction { get; set; }
        public DbSet<RushingYardsPredictionDbo> RushingYardsPrediction { get; set; }
        public DbSet<FumblesPredictionDbo> FumblesPrediction { get; set; }
        public DbSet<InterceptionsPredictionDbo> InterceptionsPrediction { get; set; }
        public DbSet<BetDbo> Bet { get; set; }
        public DbSet<BetHistoryDbo> BetHistory { get; set; }
        public DbSet<GamePredictionDbo> GamePrediction { get; set; }
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
            modelBuilder.Entity<OverPredictionDbo>().ToTable("OverPrediction").HasKey("OverPredictionId");
            modelBuilder.Entity<OffensiveModelDbo>().ToTable("OffensiveModel").HasKey("OffensiveModelId");
            modelBuilder.Entity<OverPredictionConferenceDbo>().ToTable("OverPredictionConference").HasKey("OverPredictionId");
            modelBuilder.Entity<FirstDownPredictionDbo>().ToTable("FirstDownPrediction").HasKey("FirstDownPredictionId");
            modelBuilder.Entity<ThirdDownPredictionDbo>().ToTable("ThirdDownPrediction").HasKey("ThirdDownPredictionId");
            modelBuilder.Entity<FourthDownPredictionDbo>().ToTable("FourthDownPrediction").HasKey("FourthDownPredictionId");
            modelBuilder.Entity<PassingYardsPredictionDbo>().ToTable("PassingYardsPrediction").HasKey("PassingYardsPredictionId");
            modelBuilder.Entity<YardsPerPassPredictionDbo>().ToTable("YardsPerPassPrediction").HasKey("YardsPerPassPredictionId");
            modelBuilder.Entity<RushingYardsPredictionDbo>().ToTable("RushingYardsPrediction").HasKey("RushingYardsPredictionId");
            modelBuilder.Entity<FumblesPredictionDbo>().ToTable("FumblesPrediction").HasKey("FumblesPredictionId");
            modelBuilder.Entity<InterceptionsPredictionDbo>().ToTable("InterceptionsPrediction").HasKey("InterceptionsPredictionId");
            modelBuilder.Entity<BetDbo>().ToTable("Bet").HasKey("BetId");
            modelBuilder.Entity<BetHistoryDbo>().ToTable("BetHistory").HasKey("BetId");
            modelBuilder.Entity<GamePredictionDbo>().ToTable("GamePrediction").HasKey("GamePredictionId");
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
