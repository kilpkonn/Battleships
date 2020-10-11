using System;
using System.Linq;
using System.Text;

namespace Renderer
{
    public class ConsoleRenderer
    {
        private const int CellWidth = 5;
        private const int CellHeight = 3;
        private const string ShipSymbol = "x"; //"🚢";
        private const string EmptySymbol = "~"; //"🌊";
        private const string HorizontalBorderSymbol = "=";
        private const string VerticalBorderSymbol = "|";
        private const ConsoleColor BorderColor = ConsoleColor.Yellow;
        private const ConsoleColor LegendColor = ConsoleColor.DarkCyan;

        public int HighlightX { get; set; } = 0;
        public int HighlightY { get; set; } = 0;

        public ConsoleRenderer()
        {
            Console.OutputEncoding = Encoding.UTF8;
        }

        public void Render(bool[,] board)
        {
            RenderBorder(board.GetLength(1), board.GetLength(0));
            for (int y = 0; y < board.GetLength(0); y++)
            {
                for (int x = 0; x < board.GetLength(1); x++)
                {
                    bool isSelected = y == HighlightY && x == HighlightX;

                    if (isSelected)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.SetCursorPosition((x+ 1) * (CellWidth - 1), (y + 1) * (CellHeight - 1));
                        Console.Write($"+{string.Concat(Enumerable.Repeat("-", CellWidth - 2))}+");
                        for (int i = 1; i < (CellHeight - 1); i++)
                        {
                            Console.SetCursorPosition((x + 1) * (CellWidth - 1), (y + 1) * (CellHeight - 1) + i);
                            string elem = board[y, x] ? ShipSymbol : EmptySymbol;
                            string padding = string.Concat(Enumerable.Repeat(" ", CellWidth / 2 - 1));
                            Console.Write($"|{padding}{elem}{padding}|");
                        }
                        Console.SetCursorPosition((x + 1) * (CellWidth - 1), (y + 1) * (CellHeight - 1) + CellHeight - 1);
                        Console.Write($"+{string.Concat(Enumerable.Repeat("-", CellWidth - 2))}+");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.SetCursorPosition((x + 1) * (CellWidth - 1) + CellWidth / 2, (y + 1) * (CellHeight - 1) + CellHeight / 2);
                        string elem = board[y, x] ? ShipSymbol : EmptySymbol;
                        Console.Write(elem);
                        for (int i = 0; i < (CellHeight - 1) / 2; i++)
                        {
                            Console.SetCursorPosition((x + 1)* (CellWidth - 1) + 1, (y + 1) * (CellHeight - 1) + CellHeight / 2 + i + 1);
                            Console.Write(string.Concat(Enumerable.Repeat(" ", CellWidth - 2)));
                        }
                    }
                }
            }
        }

        private void RenderBorder(int width,int height)
        {
            for (int i = 0; i < height; i++)
            {
                Console.ForegroundColor = LegendColor;
                // Left
                Console.SetCursorPosition(CellWidth / 2 - 1, (i + 1) * (CellHeight - 1) + CellHeight / 2);
                Console.Write((char) (i + 65));
                // Right
                Console.SetCursorPosition((width + 1) * (CellWidth - 1) + CellWidth / 2 + 1, (i + 1) * (CellHeight - 1) + CellHeight / 2);
                Console.Write((char) (i + 65));
                
                Console.ForegroundColor = BorderColor;
                for (int j = 0; j < CellHeight; j++)
                {
                    // Left
                    Console.SetCursorPosition(CellWidth / 2 + 1, (i + 1) * (CellHeight - 1) + j);
                    Console.Write(VerticalBorderSymbol);
                    // Right
                    Console.SetCursorPosition((width + 1) * (CellWidth - 1) + CellWidth / 2 - 1, (i + 1) * (CellHeight - 1) + j);
                    Console.Write(VerticalBorderSymbol); 
                }
            }
            
            for (int i = 0; i < width; i++)
            {
                Console.ForegroundColor = LegendColor;
                // Top
                Console.SetCursorPosition((i + 1) * (CellWidth - 1) + CellWidth / 2, CellHeight / 2 - 1);
                Console.Write(i);
                // Bottom
                Console.SetCursorPosition((i + 1) * (CellWidth - 1) + CellWidth / 2, (height + 1) * (CellHeight - 1) + CellHeight / 2 + 1);
                Console.Write(i);
                
                Console.ForegroundColor = BorderColor;
                // Top
                Console.SetCursorPosition((i + 1) * (CellWidth - 1), CellHeight / 2);
                Console.Write(string.Concat(Enumerable.Repeat(HorizontalBorderSymbol, CellWidth)));
                // Bottom
                Console.SetCursorPosition((i + 1) * (CellWidth - 1), (height + 1) * (CellHeight - 1) + CellHeight / 2);
                Console.Write(string.Concat(Enumerable.Repeat(HorizontalBorderSymbol, CellWidth)));
            }
        }
    }
}