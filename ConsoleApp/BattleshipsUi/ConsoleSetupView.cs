using System;
using System.Linq;
using BattleshipsBoard;
using Renderer;
using Util;

namespace ConsoleBattleshipsUi
{
    public class ConsoleSetupView : GameSetupUi
    {
        private readonly ConsoleRenderer _renderer = new ConsoleRenderer(3, 2);

        public override void Step(GameBoard board)
        {
            ConsoleUtil.WriteBlanks();
            Console.SetCursorPosition(0, Console.WindowHeight / 2);
            Console.WriteLine(board.WhiteToMove
                ? "White to move! Press any key to continue..."
                : "Black to move! Press any key to continue...");

            Console.ReadKey();
            var layer = board.WhiteToMove ? GameBoard.BoardType.WhiteShips : GameBoard.BoardType.BlackShips;
            bool horizontal = true;
            int length = board.ShipCounts
                .Where(s =>
                {
                    var a = board.CountShipsWithSize(board.Board[(int) layer], s.Key);
                    return board.CountShipsWithSize(board.Board[(int) layer], s.Key) < s.Value;
                })
                .Max(s => s.Key);
            do
            {
                bool choosing = true;
                while (choosing) {
                    ConsoleUtil.WriteBlanks();
                    _renderer.Render(board.Board[(int) layer], length, horizontal);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("\nPress Q to quit!");
                    var input = Console.ReadKey();
                    switch (input.Key)
                    {
                        case ConsoleKey.UpArrow:
                            _renderer.HighlightY = Math.Max(0, _renderer.HighlightY - 1);
                            break;
                        case ConsoleKey.DownArrow:
                            _renderer.HighlightY = Math.Min(board.Height - (horizontal ? 1 : length),
                                _renderer.HighlightY + 1);
                            break;
                        case ConsoleKey.RightArrow:
                            _renderer.HighlightX = Math.Min(board.Width - (horizontal ? length : 1),
                                _renderer.HighlightX + 1);
                            break;
                        case ConsoleKey.LeftArrow:
                            _renderer.HighlightX = Math.Max(0, _renderer.HighlightX - 1);
                            break;
                        case ConsoleKey.Enter:
                            choosing = false;
                            break;
                        case ConsoleKey.Spacebar:
                            horizontal = !horizontal;
                            _renderer.HighlightX = Math.Min(board.Width - (horizontal ? length : 1),
                                _renderer.HighlightX);
                            _renderer.HighlightY = Math.Min(board.Height - (horizontal ? 1 : length),
                                _renderer.HighlightY);
                            break;
                        case ConsoleKey.Q:
                            ExitCallback?.Invoke();
                            return;
                    }
                }
            } while (!PlaceShipCallback?.Invoke(_renderer.HighlightY, _renderer.HighlightX, length, horizontal) ??
                     true);

            _renderer.HighlightX = 0;
            _renderer.HighlightY = 0;
        }
    }
}