using System;
using BattleshipsBoard;

namespace ConsoleBattleshipsUi
{
    public abstract class GameSetupUi
    {
        public abstract void Step(GameBoard board);
        
        public Action? ExitCallback { get; set; }
        public Func<int, int, bool>? PlaceShipCallback { get; set; }
    }
}