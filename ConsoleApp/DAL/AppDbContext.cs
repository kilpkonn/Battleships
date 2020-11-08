using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class AppDbContext : DbContext
    {
        public DbSet<Boat> Boats { get; set; } = null!;
        public DbSet<GameSession> GameSessions { get; set; } = null!;
        public DbSet<Player> Players { get; set; } = null!;

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Player>()
                .HasMany<GameSession>()
                .WithOne(x => x.PlayerWhite)
                .HasForeignKey(x => x.PlayerWhiteId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Player>()
                .HasMany<GameSession>()
                .WithOne(x => x.PlayerBlack)
                .HasForeignKey(x => x.PlayerBlackId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}