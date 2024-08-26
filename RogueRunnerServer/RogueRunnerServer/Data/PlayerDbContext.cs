using Microsoft.EntityFrameworkCore;
using RogueRunnerServer.Controllers;
using RogueRunnerServer.Model;

namespace RogueRunnerServer.Data
{
    public class PlayerDbContext : DbContext
    {
        public DbSet<PlayerData> PlayerDatas { get; set; }

        public PlayerDbContext(DbContextOptions<PlayerDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerData>().ToTable("playerdata");
            modelBuilder.Entity<PlayerData>().HasKey(u => u.P_Id);
            modelBuilder.Entity<PlayerData>().Property(u => u.Stage).IsRequired();
            modelBuilder.Entity<PlayerData>().Property(u => u.SceneName).IsRequired();
            modelBuilder.Entity<PlayerData>().Property(u => u.PlayerCharacter).IsRequired();
            modelBuilder.Entity<PlayerData>().Property(u => u.HP).IsRequired();
            modelBuilder.Entity<PlayerData>().Property(u => u.Score).IsRequired();
            modelBuilder.Entity<PlayerData>().Property(u => u.Speed).IsRequired();
            modelBuilder.Entity<PlayerData>().Property(u => u.Skills).IsRequired();
        }
    }
}
