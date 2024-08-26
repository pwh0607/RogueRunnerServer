using Microsoft.EntityFrameworkCore;
using RogueRunnerServer.Controllers;
using RogueRunnerServer.Model;

namespace RogueRunnerServer.Data
{
    //DB의 테이블과 클라이언트로 부터 받아온 값을 정리.
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