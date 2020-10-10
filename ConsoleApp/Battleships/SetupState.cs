using System;
using ConsoleGame;
using Renderer;

namespace Battleships
{
    public class SetupState : BaseState
    {
        private readonly Game _game = Game.GetInstance();
        private readonly ConsoleRenderer _renderer = new ConsoleRenderer();

        public void Step()
        {
            if (_game.GameBoard == null) return;

            Console.WriteLine(_game.GameBoard.WhiteToMove
                ? "White to move! Press any key to continue..."
                : "Black to move! Press any key to continue...");

            Console.ReadKey();
            var layer = _game.GameBoard.WhiteToMove ? GameBoard.BoardType.WhiteShips : GameBoard.BoardType.BlackShips;
            _renderer.Render(_game.GameBoard.Board[(int) layer]);
            Console.ReadKey();
        }
    }
}