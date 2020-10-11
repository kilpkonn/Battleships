using System;
using BattleshipsBoard;
using Renderer;
using Util;

namespace ConsoleBattleshipsUi
{
    public class ConsoleSetupView: GameSetupUi
    {
        private readonly ConsoleRenderer _renderer = new ConsoleRenderer();
        
         public override void Step(GameBoard board)
        {
            ConsoleUtil.WriteBlanks();
            Console.SetCursorPosition(0, Console.WindowHeight / 2);
            Console.WriteLine(board.WhiteToMove
                ? "White to move! Press any key to continue..."
                : "Black to move! Press any key to continue...");

            Console.ReadKey();
            var layer = board.WhiteToMove ? GameBoard.BoardType.WhiteShips : GameBoard.BoardType.BlackShips;
            do
            {
                bool choosing = true;
                while (choosing)
                {
                    ConsoleUtil.WriteBlanks();
                    _renderer.Render(board.Board[(int) layer]); ;
                    Console.Write("\nPress Q to quit!");
                    var input = Console.ReadKey();
                    switch (input.Key)
                    {
                        case ConsoleKey.UpArrow:
                            _renderer.HighlightY = Math.Max(0, _renderer.HighlightY - 1);
                            break;
                        case ConsoleKey.DownArrow:
                            _renderer.HighlightY = Math.Min(board.Height - 1, _renderer.HighlightY + 1);
                            break;
                        case ConsoleKey.RightArrow:
                            _renderer.HighlightX = Math.Min(board.Width - 1, _renderer.HighlightX + 1);
                            break;
                        case ConsoleKey.LeftArrow:
                            _renderer.HighlightX = Math.Max(0, _renderer.HighlightX - 1);
                            break;
                        case ConsoleKey.Enter:
                            choosing = false;
                            break;
                        case ConsoleKey.Q:
                            ExitCallback?.Invoke();
                            return;
                    }
                }
            } while (!PlaceShipCallback?.Invoke(_renderer.HighlightY, _renderer.HighlightX) ?? true);

            _renderer.HighlightX = 0;
            _renderer.HighlightY = 0;
        }
    }
}