using Microsoft.EntityFrameworkCore;
using RogueRunnerServer.Controllers;
using RogueRunnerServer.Model;

namespace RogueRunnerServer.Data
{
    public class UserDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("userdata");
            modelBuilder.Entity<User>().HasKey(u => u.P_Id);
            modelBuilder.Entity<User>().Property(u => u.Id).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.PasswordHash).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Nickname).IsRequired();
        }
    }
}