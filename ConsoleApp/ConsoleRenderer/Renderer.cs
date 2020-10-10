using System;
using System.Linq;
using Battleships;

namespace ConsoleRenderer
{
    public class Renderer
    {
        private const int CellWidth = 5;
        private const int CellHeight = 3;

        public int HighlightX { get; set; } = 0;
        public int HighlightY { get; set; } = 0;

        public void Render(GameBoard board, GameBoard.BoardType type)
        {
            for (int y = 0; y < board.Board.GetLength(1); y++)
            {
                for (int x = 0; x < board.Board.GetLength(2); x++)
                {
                    if (y == HighlightY && x == HighlightX) Console.ForegroundColor = ConsoleColor.Cyan;
                    else Console.ForegroundColor = ConsoleColor.White;

                    Console.SetCursorPosition(x * 5, y * 3);
                    Console.WriteLine($"+{string.Concat(Enumerable.Repeat("-", CellWidth - 2))}+");
                    for (int i = 0; i < CellHeight; i++)
                    {
                        string elem = board.Board[(int) type, y, x] ? "x" : " ";
                        Console.WriteLine($"+{string.Concat(Enumerable.Repeat(elem, CellWidth - 2))}+");
                    }

                    Console.WriteLine($"+{string.Concat(Enumerable.Repeat("-", CellWidth - 2))}+");
                }
            }
        }
    }
}