using System;
using System.Linq;
using Battleships;
using DAL;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace ConsoleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Launching Battleships...");

            var dbOptions = new DbContextOptionsBuilder<AppDbContext>();
            dbOptions.UseSqlServer(@"Server=localhost,1433;Database=battleships;User=sa;Password=My.password.123;");

            using var dbCtx = new AppDbContext(dbOptions.Options);
            dbCtx.Database.Migrate();

            if (!dbCtx.Players.Any(x => x.Name == "Player White"))
            {
                Player player = new Player("Player White");
                dbCtx.Players.Add(player);
                dbCtx.SaveChanges();
            }
            
            if (!dbCtx.Players.Any(x => x.Name == "Player Black"))
            {
                Player player = new Player("Player Black");
                dbCtx.Players.Add(player);
                dbCtx.SaveChanges();
            }

            Game game = new Game(dbCtx);
            game.Run();
        }
    }
}