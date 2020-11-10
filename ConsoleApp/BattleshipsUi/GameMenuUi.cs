using System;
using System.Collections;
using System.Collections.Generic;

namespace ConsoleBattleshipsUi
{
    public abstract class GameMenuUi
    {
        public readonly int MinBoardWidth;
        public readonly int MinBoardHeight;
        public readonly int MaxBoardWidth;
        public readonly int MaxBoardHeight;

        protected Configuration.Configuration Configuration { get; } = new Configuration.Configuration(10, 10);
        protected Func<List<string>> LoadDbSessions { get; set; }


        public GameMenuUi(int minBoardWidth, int minBoardHeight, int maxBoardWidth, int maxBoardHeight,
            Func<List<string>> loadDbSessions)
        {
            MinBoardWidth = minBoardWidth;
            MinBoardHeight = minBoardHeight;
            MaxBoardWidth = maxBoardWidth;
            MaxBoardHeight = maxBoardHeight;
            LoadDbSessions = loadDbSessions;
        }

        protected int BoardWidth
        {
            get => Configuration.BoardWidth;
            set => Configuration.BoardWidth = Math.Clamp(value, MinBoardWidth, MaxBoardWidth);
        }

        protected int BoardHeight
        {
            get => Configuration.BoardHeight;
            set => Configuration.BoardHeight = Math.Clamp(value, MinBoardHeight, MaxBoardHeight);
        }

        public Action<Configuration.Configuration>? StartGameCallback { get; set; }
        public Action<string>? SaveCallback { get; set; }
        public Action? ExitCallback { get; set; }
        public Func<string, bool>? LoadGameCallback { get; set; }

        public abstract void Step();
    }
}