using System;
using Battleships;

namespace ConsoleBattleshipsUi
{
    public abstract class GameMenuUi
    {
        public readonly int MinBoardWidth;
        public readonly int MinBoardHeight;
        public readonly int MaxBoardWidth;
        public readonly int MaxBoardHeight;

        public GameMenuUi(int minBoardWidth, int minBoardHeight, int maxBoardWidth, int maxBoardHeight)
        {
            MinBoardWidth = minBoardWidth;
            MinBoardHeight = minBoardHeight;
            MaxBoardWidth = maxBoardWidth;
            MaxBoardHeight = maxBoardHeight;
        }

        public Action<Configuration>? StartGameCallback { get; set; }
        public Action<string>? SaveCallback { get; set; }
        public Action? ExitCallback { get; set; }
        public Func<string, bool>? LoadGameCallback { get; set; }

        public abstract void Step();
    }
}