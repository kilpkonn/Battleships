using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Configuration;

namespace BattleshipsBoard
{
    public class GameBoard
    {
        public enum BoardType
        {
            WhiteShips = 0,
            BlackShips = 1,
            WhiteHits = 2,
            BlackHits = 3
        }

        public int[][,] Board { get; } = new int[4][,];
        public bool WhiteToMove { get; private set; } = true;
        public int Height { get; }
        public int Width { get; }
        public ImmutableDictionary<int, int> ShipCounts { get; }
        public TouchMode TouchMode { get; }
        private int _shipId = 1;

        public bool IsSetup { get; private set; } = true;

        public GameBoard(int width, int height, Dictionary<int, int> shipCounts, TouchMode touchMode)
        {
            for (int i = 0; i < 4; i++)
            {
                Board[i] = new int[height, width];
            }

            Height = height;
            Width = width;
            ShipCounts = shipCounts.ToImmutableDictionary(
                e => e.Key,
                e => e.Value);
            TouchMode = touchMode;
        }

        public bool IsSetupComplete()
        {
            Dictionary<int, int> whiteShips = CountShips(Board[(int) BoardType.WhiteShips]);
            Dictionary<int, int> blackShips = CountShips(Board[(int) BoardType.BlackShips]);

            var white = whiteShips.Values
                .GroupBy(s => s)
                .ToDictionary(s => s.Key, s => s.Count());
            var black = blackShips.Values
                .GroupBy(s => s)
                .ToDictionary(s => s.Key, s => s.Count());

            return ShipCounts
                .All(c => white.GetValueOrDefault(c.Key, 0) == c.Value &&
                          black.GetValueOrDefault(c.Key, 0) == c.Value);
        }

        public int CountShipsWithSize(int[,] board, int size)
        {
            var shipLengths = CountShips(board);

            return shipLengths.Values.Count(v => v == size);
        }

        private Dictionary<int, int> CountShips(int[,] board)
        {
            Dictionary<int, int> shipLengths = new Dictionary<int, int>();
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (board[y, x] != 0)
                    {
                        shipLengths[board[y, x]] = shipLengths.GetValueOrDefault(board[y, x], 0) + 1;
                    }
                }
            }

            return shipLengths;
        }

        public bool PlaceShip(int y, int x, int length, bool horizontal)
        {
            if (WhiteToMove && IsFree(Board[(int) BoardType.WhiteShips], y, x, length, horizontal, TouchMode))
            {
                for (int i = 0; i < length; i++)
                {
                    Board[(int) BoardType.WhiteShips][y + (!horizontal ? i : 0), x + (horizontal ? i : 0)] = _shipId;
                }
            }
            else if (!WhiteToMove && IsFree(Board[(int) BoardType.BlackShips], y, x, length, horizontal, TouchMode))
            {
                for (int i = 0; i < length; i++)
                {
                    Board[(int) BoardType.BlackShips][y + (!horizontal ? i : 0), x + (horizontal ? i : 0)] = _shipId;
                }
            }
            else
            {
                return false;
            }

            WhiteToMove = !WhiteToMove;
            _shipId++;
            return true;
        }

        private static bool IsFree(int[,] board, int y, int x, int length, bool horizontal,
            TouchMode touchMode = TouchMode.AllTouch)
        {
            bool isFree = true;
            for (int i = 0; i < length; i++)
            {
                switch (touchMode)
                {
                    case TouchMode.AllTouch:
                        isFree &= board[y + (!horizontal ? i : 0), x + (horizontal ? i : 0)] == 0;
                        break;
                    case TouchMode.CornersTouch:
                        isFree &= board[y + (!horizontal ? i : 0), x + (horizontal ? i : 0)] == 0;
                        isFree &= board[y + (!horizontal ? i : 0) + 1, x + (horizontal ? i : 0)] == 0;
                        isFree &= board[y + (!horizontal ? i : 0) - 1, x + (horizontal ? i : 0)] == 0;
                        isFree &= board[y + (!horizontal ? i : 0), x + (horizontal ? i : 0) + 1] == 0;
                        isFree &= board[y + (!horizontal ? i : 0), x + (horizontal ? i : 0) - 1] == 0;
                        break;
                    case TouchMode.NoTouch:
                        for (int j = -1; j < 2; j++)
                        {
                            for (int k = -1; k < 2; k++)
                            {
                                if (y + (!horizontal ? i : 0) + j >= 0 && x + (horizontal ? i : 0) + k >= 0 &&
                                    x + (horizontal ? i : 0) + k < board.GetLength(1) &&
                                    y + (!horizontal ? i : 0) + j < board.GetLength(0))
                                {
                                    isFree &= board[y + (!horizontal ? i : 0) + j, x + (horizontal ? i : 0) + k] == 0;
                                }
                            }
                        }

                        break;
                }
            }

            return isFree;
        }

        public bool PlaceBomb(int y, int x)
        {
            if (WhiteToMove)
            {
                Board[(int) BoardType.WhiteHits][y, x] = 1;
                return Board[(int) BoardType.BlackShips][y, x] != 0;
            }
            else
            {
                Board[(int) BoardType.BlackHits][y, x] = 1;
                return Board[(int) BoardType.WhiteShips][y, x] != 0;
            }
        }

        public static GameBoard? FromJsonState(JsonGameState state)
        {
            if (!state.IsInitialized)
            {
                return null;
            }

            var shipSizes = state.ShipCounts.ToDictionary(
                e => Convert.ToInt32(e.Key),
                e => e.Value);

            GameBoard board = new GameBoard(state.Width, state.Height, shipSizes, state.TouchMode);
            board.IsSetup = state.IsSetup;
            board.WhiteToMove = state.WhiteToMove;
            for (int i = 0; i < 4; i++)
            {
                if (state.Boards.ContainsKey(i.ToString()))
                {
                    board.Board[i] = Util.ArrayUtils.ConvertTo2D(state.Boards[i.ToString()]);
                }
            }

            return board;
        }
    }
}