using System;
using System.Linq;
using System.Text;

namespace Renderer
{
    public class ConsoleRenderer
    {
        private readonly int _cellWidth = 5;
        private readonly int _cellHeight = 3;
        private const string ShipSymbol = "x"; //"🚢";
        private const string EmptySymbol = "~"; //"🌊";
        private const string HorizontalBorderSymbol = "=";
        private const string VerticalBorderSymbol = "|";
        private const ConsoleColor BorderColor = ConsoleColor.Yellow;
        private const ConsoleColor LegendColor = ConsoleColor.DarkCyan;
        private const ConsoleColor ShipColor = ConsoleColor.Magenta;
        private const ConsoleColor WaterColor = ConsoleColor.Blue;
        public static ConsoleColor CrosshairColor { get; } = ConsoleColor.Cyan;

        public int HighlightX { get; set; } = 0;
        public int HighlightY { get; set; } = 0;

        public ConsoleRenderer(int cellWidth, int cellHeight)
        {
            Console.OutputEncoding = Encoding.UTF8;
            _cellWidth = cellWidth;
            _cellHeight = cellHeight;
        }

        public void Render(int[,] board, int length, bool horizontal)
        {
            RenderBorder(board.GetLength(1), board.GetLength(0));
            for (int y = 0; y < board.GetLength(0); y++)
            {
                for (int x = 0; x < board.GetLength(1); x++)
                {
                    bool isSelected = horizontal
                        ? y == HighlightY && x >= HighlightX && x < HighlightX + length
                        : x == HighlightX && y >= HighlightY && y < HighlightY + length;
                    
                    if (isSelected)
                    {
                        RenderSelectedCell(board, y, x);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.SetCursorPosition((x + 1) * (_cellWidth - 1) + _cellWidth / 2,
                            (y + 1) * (_cellHeight - 1) + _cellHeight / 2);
                        string elem = board[y, x] != 0 ? ShipSymbol : EmptySymbol;
                        Console.ForegroundColor = board[y, x] != 0 ? ShipColor : WaterColor;
                        Console.Write(elem);
                        for (int i = 0; i < (_cellHeight - 1) / 2; i++)
                        {
                            Console.SetCursorPosition((x + 1) * (_cellWidth - 1) + 1,
                                (y + 1) * (_cellHeight - 1) + _cellHeight / 2 + i + 1);
                            Console.Write(string.Concat(Enumerable.Repeat(" ", _cellWidth - 2)));
                        }
                    }
                }
            }

            if (_cellHeight == 2)
            {
                Console.SetCursorPosition(0, (_cellHeight - 1) * (2 + board.GetLength(0)) + 1);
            }
            else
            {
                Console.SetCursorPosition(0, (_cellHeight - 1) * (2 + board.GetLength(0)));
            }
        }

        private void RenderSelectedCell(int[,] board, int y, int x)
        {
            Console.ForegroundColor = board[y, x] != 0 ? ShipColor : CrosshairColor;
            if (_cellWidth == 3)
            {
                Console.SetCursorPosition((x + 1) * (_cellWidth - 1) + 1, (y + 1) * (_cellHeight - 1) + 1);
                Console.Write("+");
                return;
            }

            Console.SetCursorPosition((x + 1) * (_cellWidth - 1), (y + 1) * (_cellHeight - 1));

            Console.Write($"+{string.Concat(Enumerable.Repeat("-", _cellWidth - 2))}+");
            for (int i = 1; i < (_cellHeight - 1); i++)
            {
                Console.SetCursorPosition((x + 1) * (_cellWidth - 1), (y + 1) * (_cellHeight - 1) + i);
                string elem = board[y, x] != 0 ? ShipSymbol : EmptySymbol;
                string padding = string.Concat(Enumerable.Repeat(" ", _cellWidth / 2 - 1));
                Console.Write($"|{padding}{elem}{padding}|");
            }

            Console.SetCursorPosition((x + 1) * (_cellWidth - 1),
                (y + 1) * (_cellHeight - 1) + _cellHeight - 1);
            Console.Write($"+{string.Concat(Enumerable.Repeat("-", _cellWidth - 2))}+");
        }

        private void RenderBorder(int width, int height)
        {
            for (int i = 0; i < height; i++)
            {
                Console.ForegroundColor = LegendColor;
                // Left
                Console.SetCursorPosition(_cellWidth / 2 - 1, (i + 1) * (_cellHeight - 1) + _cellHeight / 2);
                Console.Write((char) (i + 65));
                // Right
                Console.SetCursorPosition((width + 1) * (_cellWidth - 1) + _cellWidth / 2 + 1,
                    (i + 1) * (_cellHeight - 1) + _cellHeight / 2);
                Console.Write((char) (i + 65));

                Console.ForegroundColor = BorderColor;
                for (int j = 0; j < _cellHeight; j++)
                {
                    // Left
                    Console.SetCursorPosition(_cellWidth / 2 + 1, (i + 1) * (_cellHeight - 1) + j);
                    Console.Write(VerticalBorderSymbol);
                    // Right
                    Console.SetCursorPosition((width + 1) * (_cellWidth - 1) + _cellWidth / 2 - 1,
                        (i + 1) * (_cellHeight - 1) + j);
                    Console.Write(VerticalBorderSymbol);
                }
            }

            for (int i = 0; i < width; i++)
            {
                Console.ForegroundColor = LegendColor;
                // Top
                Console.SetCursorPosition((i + 1) * (_cellWidth - 1) + _cellWidth / 2, _cellHeight / 2 - 1);
                Console.Write(i);
                // Bottom
                Console.SetCursorPosition((i + 1) * (_cellWidth - 1) + _cellWidth / 2,
                    (height + 1) * (_cellHeight - 1) + _cellHeight / 2 + 1);
                Console.Write(i);

                Console.ForegroundColor = BorderColor;
                // Top
                Console.SetCursorPosition((i + 1) * (_cellWidth - 1), _cellHeight / 2);
                Console.Write(string.Concat(Enumerable.Repeat(HorizontalBorderSymbol, _cellWidth)));
                // Bottom
                Console.SetCursorPosition((i + 1) * (_cellWidth - 1),
                    (height + 1) * (_cellHeight - 1) + _cellHeight / 2);
                Console.Write(string.Concat(Enumerable.Repeat(HorizontalBorderSymbol, _cellWidth)));
            }
        }
    }
}