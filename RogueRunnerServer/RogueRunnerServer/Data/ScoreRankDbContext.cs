using Microsoft.EntityFrameworkCore;
using RogueRunnerServer.Controllers;
using RogueRunnerServer.Model;

namespace RogueRunnerServer.Data
{
    public class ScoreRankDbContext : DbContext
    {
        public DbSet<ScoreRank> ScoreDatas { get; set; }

        public ScoreRankDbContext(DbContextOptions<ScoreRankDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ScoreRank>().ToTable("scoreboard");
            modelBuilder.Entity<ScoreRank>().HasKey(u => u.P_Id);
            modelBuilder.Entity<ScoreRank>().HasKey(u => u.Nickname);
            modelBuilder.Entity<ScoreRank>().HasKey(u => u.Score);
        }
    }
}
