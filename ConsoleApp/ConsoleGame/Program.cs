using System;
using Battleships;
using DAL;
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

            using (var dbCtx = new AppDbContext(dbOptions.Options))
            {
                dbCtx.Database.Migrate();
            }
            
            Game game = Game.GetInstance();
            game.Run();
        }
    }
}