using System;
using ConsoleGame;
using Renderer;

namespace Battleships
{
    public class SetupState : BaseState
    {
        private readonly Game _game;
        private readonly ConsoleRenderer _renderer = new ConsoleRenderer();

        public SetupState(Game game)
        {
            _game = game;
        }

        public void Step()
        {
            if (_game.GameBoard == null) return;
            Util.ConsoleUtil.WriteBlanks();
            Console.SetCursorPosition(0, Console.WindowHeight / 2);
            Console.WriteLine(_game.GameBoard.WhiteToMove
                ? "White to move! Press any key to continue..."
                : "Black to move! Press any key to continue...");

            Console.ReadKey();
            
            var layer = _game.GameBoard.WhiteToMove ? GameBoard.BoardType.WhiteShips : GameBoard.BoardType.BlackShips;
            do
            {
                bool choosing = true;
                while (choosing)
                {
                    _renderer.Render(_game.GameBoard.Board[(int) layer]);
                    Console.Write("\nPress Q to quit!");
                    var input = Console.ReadKey();
                    switch (input.Key)
                    {
                        case ConsoleKey.UpArrow:
                            _renderer.HighlightY = Math.Max(0, _renderer.HighlightY - 1);
                            break;
                        case ConsoleKey.DownArrow:
                            _renderer.HighlightY = Math.Min(_game.GameBoard.Height - 1, _renderer.HighlightY + 1);
                            break;
                        case ConsoleKey.RightArrow:
                            _renderer.HighlightX = Math.Min(_game.GameBoard.Width - 1, _renderer.HighlightX + 1);
                            break;
                        case ConsoleKey.LeftArrow:
                            _renderer.HighlightX = Math.Max(0, _renderer.HighlightX - 1);
                            break;
                        case ConsoleKey.Enter:
                            choosing = false;
                            break;
                        case ConsoleKey.Q:
                            _game.PushState(Game.GameState.Menu);
                            return;
                    }
                }
            } while (!_game.GameBoard.PlaceShip(_renderer.HighlightY, _renderer.HighlightX));

            _renderer.HighlightX = 0;
            _renderer.HighlightY = 0;
        }
    }
}