using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Domain;
using Util;

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

        public int[][,] Board { get; private set; } = new int[4][,];
        public bool WhiteToMove { get; private set; } = true;
        public int Height { get; }
        public int Width { get; }
        public ImmutableDictionary<int, int> ShipCounts { get; }
        public TouchMode TouchMode { get; }
        public bool BackToBackHits { get; set; }
        private int _shipId = 1;

        public List<BoardState> BoardHistory { get; } = new();

        public bool IsSetup { get; private set; } = true;

        public GameBoard(int width, int height, Dictionary<int, int> shipCounts, TouchMode touchMode,
            bool backToBackHits)
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
            BackToBackHits = backToBackHits;
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
            Dictionary<int, int> shipLengths = new();
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

        public bool GenerateBoard()
        {
            if (IsSetupComplete()) return true;
            Board[(int) BoardType.WhiteShips] = new int[Height, Width];
            Board[(int) BoardType.BlackShips] = new int[Height, Width];

            var rnd = new Random();
            foreach (var (length, count) in ShipCounts.Reverse())
            {
                for (int c = 0; c < count * 2; c++)
                {
                    HashSet<Tuple<int, int>> optionsY = new();
                    HashSet<Tuple<int, int>> optionsX = new();
                    for (int y = 0; y < Height; y++)
                    {
                        for (int x = 0; x < Width; x++)
                        {
                            bool okY = true;
                            bool okX = true;
                            if (WhiteToMove)
                            {
                                okX &= x + length < Width && IsFree(Board[(int) BoardType.WhiteShips], y, x, length, true, TouchMode);
                                okY &= y + length < Height && IsFree(Board[(int) BoardType.WhiteShips], y, x, length, false, TouchMode);
                            }
                            else
                            {
                                okX &= x + length < Width && IsFree(Board[(int) BoardType.BlackShips], y, x, length, true, TouchMode);
                                okY &= y + length < Height && IsFree(Board[(int) BoardType.BlackShips], y, x, length, false, TouchMode);
                            }


                            if (okX) optionsX.Add(new Tuple<int, int>(y, x));
                            if (okY) optionsY.Add(new Tuple<int, int>(y, x));
                        }
                    }

                    if (optionsX.Count == 0 && optionsY.Count == 0) return false;

                    if (rnd.Next() % 2 == 0 && optionsX.Count > 0 || optionsY.Count <= 0)
                    {
                        var option = optionsX.ElementAt(rnd.Next(optionsX.Count));
                        PlaceShip(option.Item1, option.Item2, length, true);
                    }
                    else
                    {
                        var option = optionsY.ElementAt(rnd.Next(optionsY.Count));
                        PlaceShip(option.Item1, option.Item2, length, false);
                    }
                }
            }

            return IsSetupComplete();
        }

        public bool PlaceShip(int y, int x, int length, bool horizontal)
        {
            if (y < 0 || x < 0 || y + length >= Height && !horizontal || x + length >= Width && horizontal)
            {
                return false;
            }

            if (!ShipCounts.ContainsKey(length) ||
                WhiteToMove && CountShipsWithSize(Board[(int) BoardType.WhiteShips], length) >= ShipCounts[length] ||
                !WhiteToMove && CountShipsWithSize(Board[(int) BoardType.BlackShips], length) >= ShipCounts[length])
            {
                return false;
            }

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
            UpdateBoardHistory();
            return true;
        }

        public bool? DropBomb(int y, int x)
        {
            if (WhiteToMove)
            {
                if (Board[(int) BoardType.WhiteHits][y, x] != 0)
                {
                    return null;
                }

                Board[(int) BoardType.WhiteHits][y, x] = 1;
                WhiteToMove = BackToBackHits ? Board[(int) BoardType.BlackShips][y, x] != 0 : !WhiteToMove;
                UpdateBoardHistory();
                return Board[(int) BoardType.BlackShips][y, x] != 0;
            }
            else
            {
                if (Board[(int) BoardType.BlackHits][y, x] != 0)
                {
                    return null;
                }

                Board[(int) BoardType.BlackHits][y, x] = 1;
                WhiteToMove = BackToBackHits ? Board[(int) BoardType.WhiteShips][y, x] == 0 : !WhiteToMove;
                UpdateBoardHistory();
                return Board[(int) BoardType.WhiteShips][y, x] != 0;
            }
        }

        public bool? GameResult()
        {
            bool whiteWon = true;
            bool blackWon = true;

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (Board[(int) BoardType.BlackShips][y, x] != 0)
                    {
                        whiteWon &= Board[(int) BoardType.WhiteHits][y, x] != 0;
                    }

                    if (Board[(int) BoardType.WhiteShips][y, x] != 0)
                    {
                        blackWon &= Board[(int) BoardType.BlackHits][y, x] != 0;
                    }
                }
            }

            if (!whiteWon && !blackWon) return null;
            return whiteWon;
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

        private void UpdateBoardHistory()
        {
            BoardHistory.Add(new BoardState(ArrayUtils.Clone(Board), WhiteToMove));
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

            GameBoard board = new GameBoard(
                state.Width,
                state.Height,
                shipSizes,
                state.TouchMode,
                state.BackToBackMovesOnHit
            );
            board.IsSetup = state.IsSetup;
            board.WhiteToMove = state.WhiteToMove;
            for (int i = 0; i < 4; i++)
            {
                if (state.Boards.ContainsKey(i.ToString()))
                {
                    board.Board[i] = ArrayUtils.ConvertTo2D(state.Boards[i.ToString()]);
                }
            }

            return board;
        }

        public static GameBoard FromGameSession(GameSession session)
        {
            var shipSizes = session.Boats
                .ToDictionary(x => x.Lenght, x => x.Amount);
            GameBoard board = new GameBoard(
                session.BoardWidth,
                session.BoardHeight,
                shipSizes,
                session.TouchMode,
                session.BackToBackMovesOnHit
            );

            foreach (var state in session.BoardStates.OrderBy(x => x.BoardStateId))
            {
                var b = new int[4][,];
                b[(int) BoardType.WhiteShips] = new int[board.Height, board.Width];
                b[(int) BoardType.BlackShips] = new int[board.Height, board.Width];
                b[(int) BoardType.WhiteHits] = new int[board.Height, board.Width];
                b[(int) BoardType.BlackHits] = new int[board.Height, board.Width];

                foreach (var tile in state.BoardTiles)
                {
                    b[(int) BoardType.WhiteShips][tile.CoordY, tile.CoordX] = tile.TileWhiteShips;
                    b[(int) BoardType.BlackShips][tile.CoordY, tile.CoordX] = tile.TileBlackShips;
                    b[(int) BoardType.WhiteHits][tile.CoordY, tile.CoordX] = tile.TileWhiteHits;
                    b[(int) BoardType.BlackHits][tile.CoordY, tile.CoordX] = tile.TileBlackHits;
                }

                board.BoardHistory.Add(new BoardState(b, state.WhiteToMove));
            }

            board.WhiteToMove = board.BoardHistory.Last().WhiteToMove;
            board.Board = ArrayUtils.Clone(board.BoardHistory.Last().Board);

            var whiteShipIds = board.CountShips(board.Board[(int) BoardType.WhiteShips]).Keys;
            var blackShipIds = board.CountShips(board.Board[(int) BoardType.BlackShips]).Keys;
            if (whiteShipIds.Count == 0 && blackShipIds.Count == 0)
            {
                board._shipId = 1;
            }
            else if (whiteShipIds.Count != 0 && blackShipIds.Count != 0)
            {
                board._shipId = Math.Max(whiteShipIds.Max(), blackShipIds.Max()) + 1;
            }
            else
            {
                board._shipId = (whiteShipIds.Count == 0 ? blackShipIds.Max() : whiteShipIds.Max()) + 1;
            }

            return board;
        }
    }
}