using System;

namespace ConsoleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Launching Battleships...");
            Game game = Game.GetInstance();
            game.Run();
        }
    }
}