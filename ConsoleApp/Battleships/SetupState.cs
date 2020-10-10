using System;

namespace ConsoleGame
{
    public class SetupState : BaseState
    {
        private readonly Game _game = Game.GetInstance();

        public void Step()
        {
            if (_game.GameBoard == null) return;

            Console.WriteLine(_game.GameBoard.WhiteToMove
                ? "White to move! Press any key to continue..."
                : "Black to move! Press any key to continue...");
            
            
        }
    }
}