using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Battleships;

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

        public bool[][,] Board { get; } = new bool[4][,];
        public bool WhiteToMove { get; private set; } = true;
        public int Height { get; }
        public int Width { get; }
        public ImmutableDictionary<int, int> ShipCounts { get; }
        public TouchMode TouchMode { get;  } 

        public bool IsSetup { get; private set; } = true;

        public GameBoard(int width, int height, Dictionary<int, int> shipCounts, TouchMode touchMode)
        {
            for (int i = 0; i < 4; i++)
            {
                Board[i] = new bool[height, width];
            }

            Height = height;
            Width = width;
            ShipCounts = shipCounts.ToImmutableDictionary(
                e => e.Key,
                e => e.Value);
            TouchMode = touchMode;
        }

        public bool PlaceShip(int y, int x)
        {
            if (WhiteToMove && !Board[(int) BoardType.WhiteShips][y, x])
                Board[(int) BoardType.WhiteShips][y, x] = true;
            else if (!WhiteToMove && !Board[(int) BoardType.BlackShips][y, x])
                Board[(int) BoardType.BlackShips][y, x] = true;
            else return false;

            WhiteToMove = !WhiteToMove;
            return true;
        }

        public bool PlaceBomb(int y, int x)
        {
            if (WhiteToMove)
            {
                Board[(int) BoardType.WhiteHits][y, x] = true;
                return Board[(int) BoardType.BlackShips][y, x];
            }
            else
            {
                Board[(int) BoardType.BlackHits][y, x] = true;
                return Board[(int) BoardType.WhiteShips][y, x];
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