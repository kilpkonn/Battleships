using System;
using BattleshipsBoard;

namespace ConsoleBattleshipsUi
{
    public abstract class GamePlayUi
    {
        public abstract void Step(GameBoard board);

        public Func<int, int, bool>? DropBombCallback { get; set; }
        public Action? ExitCallback { get; set; }
    }
}