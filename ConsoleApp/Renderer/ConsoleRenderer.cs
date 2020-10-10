using System;
using System.Linq;

namespace Renderer
{
    public class ConsoleRenderer
    {
        private const int CellWidth = 5;
        private const int CellHeight = 3;

        public int HighlightX { get; set; } = 0;
        public int HighlightY { get; set; } = 0;

        public void Render(bool[,] board)
        {
            for (int y = 0; y < board.GetLength(0); y++)
            {
                for (int x = 0; x < board.GetLength(1); x++)
                {
                    if (y == HighlightY && x == HighlightX) Console.ForegroundColor = ConsoleColor.Cyan;
                    else Console.ForegroundColor = ConsoleColor.White;

                    Console.SetCursorPosition(x * 5, y * 3);
                    Console.Write($"+{string.Concat(Enumerable.Repeat("-", CellWidth - 2))}+");
                    for (int i = 0; i < CellHeight; i++)
                    {
                        Console.SetCursorPosition(x * 5, y * 3 + i + 1);
                        string elem = board[y, x] ? "x" : " ";
                        Console.Write($"|{string.Concat(Enumerable.Repeat(elem, CellWidth - 2))}|");
                    }
                    Console.SetCursorPosition(x * 5, y * 3 + CellHeight - 1);

                    Console.Write($"+{string.Concat(Enumerable.Repeat("-", CellWidth - 2))}+");
                }
            }
        }
    }
}